using System.Collections;
using System.Collections.Generic;
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
    private float maxSpeed = 3.0f;

    private Rigidbody2D rb;

    private bool isGrounded = true;
    private bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            rb.velocity = new Vector2(-1.0f, rb.velocity.y);
        if (Input.GetKey(KeyCode.D))
            rb.velocity = new Vector2( 1.0f, rb.velocity.y);

    }
}
