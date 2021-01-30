using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1;
    public float jumpForce = 1;
    public float heliHatJumpForce = 1;

    private bool usedHeliJump;

    public MechanicsManager mechanicsManager;

    private bool isGrounded;
    private bool isOnSlope;

    public Transform groundCheck;
    public Transform slopeCheck;

    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        usedHeliJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isOnSlope);

        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Physics2D.Linecast(transform.position, slopeCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isOnSlope = false;
        }
        else
        {
            isOnSlope = true;
        }

        // Walky left and right
        if (Input.GetKey("d") || Input.GetKey("right")) {
            rigidbody.velocity = new Vector2(movementSpeed, rigidbody.velocity.y);
            spriteRenderer.flipX = false;
            //TODO Animate run right
        }
        else if (Input.GetKey("a") || Input.GetKey("left")) {
            rigidbody.velocity = new Vector2(-movementSpeed, rigidbody.velocity.y);
            spriteRenderer.flipX = true; 
            //TODO Animate run left 
        }
        else {
            //TODO Animate idle
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);


        }

        // Jumpy Uppy
        if (Input.GetKeyDown("space")) {
            // Normal Jump
            if (isGrounded) {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            }
            // Helicopter Hat Jump
            else if (!isGrounded && mechanicsManager.hasHeliHat && !usedHeliJump) {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, heliHatJumpForce);
                usedHeliJump = true;
            }
        }

        if (isGrounded) {
            usedHeliJump = false;
        }
    }
}
