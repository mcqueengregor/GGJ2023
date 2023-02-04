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

    [Header("Jumping:")]
    [SerializeField]
    private float jumpStrength = 1.0f;  // Multiplier for below 'jumpHeight' value.
    const float jumpHeight = 5.0f;
    [SerializeField] [Range(0.0f, 5.0f)]
    private float gravityStrength = 1.0f;

    [Header("Recoil:")]
    [SerializeField] [Range(0.0f, 5.0f)]
    private float recoilStrength = 1.0f;    // The strength of the force that moves the
                                            // player when they collide with an enemy.

    // Misc:
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Camera mainCam;

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

        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;   // Assume user isn't pressing movement keys until we check for it.

        // Movement logic (block while recoiling):
        if (playerState != PlayerState.RECOIL)
        {
            if (Input.GetKey(KeyCode.A))
                MovePlayer(-accelRate);
            if (Input.GetKey(KeyCode.D))
                MovePlayer(accelRate);
        }

        // Deccelerate player's horizontal speed, if user isn't pressing movement keys:
        if (!isMoving)
            DeceleratePlayer();

        // Jump logic (block while recoiling):
        if (Input.GetKeyDown(KeyCode.Space) && playerState != PlayerState.RECOIL)
            Jump();

        // If falling and not recoiling from attack, adjust gravity's strength:
        if (rb.velocity.y < -0.1f && playerState != PlayerState.RECOIL)
        {
            ChangePlayerState(PlayerState.FALLING);
            rb.gravityScale = gravityStrength;
        }

        // Make camera follow the player:
        Vector3 newCamPos = mainCam.transform.position;
        newCamPos.x = transform.position.x;
        mainCam.transform.position = newCamPos;
    }

    private void MovePlayer(float horiVelocityChange)
    {
        rb.velocity += new Vector2(horiVelocityChange, 0.0f);
        
        // Prevent player's speed from exceeding max value:
        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
            rb.velocity.y);

        // Mark player as moving in this frame:
        isMoving = true;
        ChangePlayerState(PlayerState.MOVING);
    }

    private void DeceleratePlayer()
    {
        // Only decelerate player if they're actually moving, otherwise early-out:
        if (FloatWithinRange(rb.velocity.x, -horiSpeedDeadZone, horiSpeedDeadZone))
        {
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

        // Prevent player from jumping if they aren't on the ground (block if recoiling):
        if (Physics2D.Raycast(o, Vector2.down, rayLength) && playerState != PlayerState.RECOIL)
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
        playerState = newState;
        // TODO: Switch active sprite animation here!
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player collides with an enemy, make player recoil:
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ChangePlayerState(PlayerState.RECOIL);

            Vector2 forceDir = new Vector2(1.0f, 0.25f);
            forceDir.Normalize();

            rb.AddForce(forceDir * recoilStrength, ForceMode2D.Impulse); ;
        }
    }
}
