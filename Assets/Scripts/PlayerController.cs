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

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private bool isGrounded = true;
    private bool isFalling = false;

    private bool isMoving = false;  // 'true' if user is pressing A or S
                                    // to move left or right, 'false' otherwise.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;   // Assume user isn't pressing movement keys until we check for it.

        // Movement logic:
        if (Input.GetKey(KeyCode.A))
            MovePlayer(-accelRate);
        if (Input.GetKey(KeyCode.D))
            MovePlayer(accelRate);

        // Deccelerate player's horizontal speed, if user isn't pressing movement keys:
        if (!isMoving)
            DeceleratePlayer();

        // Jump logic:
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
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
    }

    private void DeceleratePlayer()
    {
        // Only decelerate player if they're actually moving:
        if (FloatWithinRange(rb.velocity.x, -horiSpeedDeadZone, horiSpeedDeadZone))
            return;

        // Figure out decel speed based on whether player is moving left or right:
        float decelSpeed = rb.velocity.x > 0.0f ? -decelRate : decelRate;
        rb.velocity += new Vector2(decelSpeed, 0.0f);
    }

    private void Jump()
    {
        // Ray origin (set to bottom of player's box collider):
        Vector2 o = new Vector2(boxCollider.transform.position.x, boxCollider.transform.position.y)
            + new Vector2(0.0f, -boxCollider.size.y / 2.0f);
        const float rayLength = 0.1f;
        
        // Prevent player from jumping if they aren't on the ground:
        if (Physics2D.Raycast(o, Vector2.down, rayLength))
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * jumpStrength);

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
}
