using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("GAMEPLAY ATTRIBUTES")]
    [Header("Movement:")]
    [SerializeField]
    private float accelRate = 1.0f;
    [SerializeField]
    private float decelRate = 1.0f;
    [SerializeField]
    private float maxHorizontalSpeed = 3.0f;
    [SerializeField] [Range(0.0f, 1.0f)] 
    private float horiSpeedDeadZone = 0.1f; // Quickly stop player from moving if their speed is
                                            // between this range (prevents jittering while stationary).
    [HideInInspector]
    public float beltPush = 0.0f;           // How much the player is being pushed by a conveyor belt
                                            // (adjusted by external conveyor belt script).
    const float beltEpsilon = 0.1f;         // Used if beltPush ~= accelRate, so that the player can
                                            // eventually overpower the belt they're moving on.

    [Header("Jumping:")]
    [SerializeField]
    private float jumpStrength = 1.0f;      // Scalar for below 'jumpHeight' value.
    const float jumpHeight = 5.0f;
    [SerializeField] [Range(0.0f, 5.0f)]
    private float gravityStrength = 1.0f;   // Rigid body's gravity strength, set while falling.

    [Header("Recoil:")]
    [SerializeField]
    private float recoilStrength = 50.0f;   // The strength of the force that moves the
                                            // player when they collide with an enemy.
    [SerializeField] [Range(0.1f, 2.0f)]
    private float recoilDuration = 1.5f;    // The amount of time the player recoils for, in seconds.
    private float recoilTimer = 0.0f;       // Tracks how long the player has recoiled for, in seconds.

    // Misc:
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;
    private Vector2 originalPos;
    private Animator animator;

    private bool isMoving = false;  // 'true' if user is pressing A or S
                                    // to move left or right, 'false' otherwise.

    private enum PlayerState { 
        IDLE    = 0,
        MOVING  = 1,
        JUMPING = 2,
        FALLING = 3,
        RECOIL  = 4     // When player has collided with enemy (blocks input and getting damaged more).
    } 
    PlayerState playerState = PlayerState.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        mainCam = Camera.main;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;   // Assume user isn't pressing movement keys until we check for it.

        // If player falls off level, teleport them back to start:
        // TODO: Make this go to "GAME OVER" screen!
        if (transform.position.y < -4.0f)
        {
            ChangePlayerState(PlayerState.IDLE);
            rb.velocity = Vector2.zero;
            transform.position = originalPos;
        }

        // Make camera follow the player:
        Vector3 newCamPos = mainCam.transform.position;
        newCamPos.x = transform.position.x;
        mainCam.transform.position = newCamPos;

        // If player is recoiling, check if they should stop:
        if (playerState == PlayerState.RECOIL)
        {
            // If player has recoiled for long enough, make them stop recoiling:
            if (recoilTimer >= recoilDuration)
            {
                ChangePlayerState(PlayerState.IDLE);
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
            }
            else
            {
                // If player should still recoil, increment timer and skip below logic:
                recoilTimer += Time.deltaTime;
                Debug.Log("Player recoiling (" + recoilTimer.ToString("F2") + "/"
                    + recoilDuration.ToString("F2") + ")...");
                return;
            }
        }

        // Movement logic (blocked if recoiling):
        if (Input.GetKey(KeyCode.A))
            MovePlayer(-accelRate);
        if (Input.GetKey(KeyCode.D))
            MovePlayer(accelRate);

        // Deccelerate player's horizontal speed, if user isn't pressing movement keys:
        if (!isMoving)
            DeceleratePlayer();

        // Take the conveyor belt's influence on the player into account:
        // NOTE: This could make the player unable to move in one direction and
        // incredibly fast in the other direction, be careful!
        rb.velocity += new Vector2(beltPush, 0.0f);

        // Jump logic (blocked if recoiling):
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // If falling and not recoiling from attack, adjust gravity's strength:
        if (rb.velocity.y < -0.1f)
        {
            ChangePlayerState(PlayerState.FALLING);
            rb.gravityScale = gravityStrength;
        }
        // If player was previously falling but has now landed, reset state:
        else if (playerState == PlayerState.FALLING)
            ChangePlayerState(PlayerState.IDLE);
    }

    private void MovePlayer(float horiVelocityChange)
    {
        // If beltPush and horiVelocity change are close enough to equal, make
        // horiVelocityChange a little higher so that player can outrun belt:
        if (FloatEqual(beltPush, -horiVelocityChange, 0.1f))
        {
            float epsilon = horiVelocityChange > 0f ? beltEpsilon : -beltEpsilon;
            horiVelocityChange += epsilon;
        }

        rb.velocity += new Vector2(horiVelocityChange, 0.0f);

        // Prevent player's speed from exceeding max value:
        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
            rb.velocity.y);

        // Mark player as moving in this frame:
        isMoving = true;
        ChangePlayerState(PlayerState.MOVING);

        // Flip rabbit sprite to face the right way:
        spriteRenderer.flipX = horiVelocityChange < 0f;
    }

    private void DeceleratePlayer()
    {
        // Only decelerate player if they're actually moving, otherwise early-out:
        if (FloatWithinRange(rb.velocity.x, -horiSpeedDeadZone, horiSpeedDeadZone))
        {
            if (!FloatEqual(beltPush, 0.0f))
                rb.velocity = new Vector2(0.0f, rb.velocity.y);

            // If player isn't jumping, falling or recoiling, update state to IDLE:
            if (playerState == PlayerState.MOVING)
                ChangePlayerState(PlayerState.IDLE);

            return;
        }

        // Figure out decel speed based on whether player is moving left or right:
        float decelSpeed = rb.velocity.x > 0.0f ? -decelRate : decelRate;
        rb.velocity += new Vector2(decelSpeed, 0.0f);
    }

    private void Jump()
    {
        // Ray origin (set to bottom of player's box collider):
        Vector2 o = new Vector2(boxCollider.transform.position.x, boxCollider.transform.position.y)
            + new Vector2(0.0f, (-boxCollider.size.y / 2.0f) * transform.localScale.y);
        const float rayLength = 0.1f;

        // Prevent player from jumping if they aren't on the ground (blocked if recoiling)::
        if (Physics2D.Raycast(o, Vector2.down, rayLength))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * jumpStrength);
            
            // Reset gravity's strength while going up:
            rb.gravityScale = 1.0f;
            ChangePlayerState(PlayerState.JUMPING);
        }

        Debug.DrawLine(o, o + Vector2.down * rayLength, Color.green, 0.25f);
    }

    // Determine if two floating-point numbers are close enough to equal or not:
    bool FloatEqual(float a, float b, float e = 1e-8f)
    {
        return Mathf.Abs(a - b) < e;
    }

    bool FloatWithinRange(float val, float rangeMin, float rangeMax)
    {
        return val >= rangeMin && val <= rangeMax;
    }

    void ChangePlayerState(PlayerState newState)
    {
        // Avoid overprinting to console:
        if (playerState != newState)
        {
            playerState = newState;
            Debug.Log("Changed " + name + "'s state to " + playerState + "!");

            switch (playerState)
            {
                case PlayerState.IDLE:
                    animator.SetBool("isFalling", false);           // Handles FALL -> IDLE
                    animator.SetBool("currentlyRecoiling", false);  // Handles HIT -> IDLE
                    animator.SetBool("isMoving", false);            // Handles RUN -> IDLE
                    break;
                case PlayerState.MOVING:
                    animator.SetBool("isMoving", true);
                    animator.SetBool("isFalling", false);
                    animator.SetBool("isJumping", false);
                    break;
                case PlayerState.JUMPING:
                    animator.SetBool("isJumping", true);
                    break;
                case PlayerState.FALLING:
                    animator.SetBool("isFalling", true);
                    animator.SetBool("isJumping", false);
                    break;
                case PlayerState.RECOIL:
                    animator.SetBool("currentlyRecoiling", true);
                    break;
                default:
                    Debug.LogWarning("Switch statement shouldn't have reached this point :thonk:");
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player collides with an enemy, make player recoil:
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangePlayerState(PlayerState.RECOIL);
            recoilTimer = 0.0f;

            Vector2 forceDir = new Vector2(-0.25f, 1.0f);
            forceDir.Normalize();

            rb.AddForce(forceDir * recoilStrength, ForceMode2D.Impulse);

            Debug.Log("Player hit enemy!");
        }
    }
}
