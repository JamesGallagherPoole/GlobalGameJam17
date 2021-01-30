using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1;
    public float jumpForce = 1;
    public float heliHatJumpForce = 1;

    private bool isGrounded;

    public Transform groundCheck;

    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetKeyDown("space"));
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
        }

        // Jumpy Uppy
        if (Input.GetKeyDown("space") && isGrounded) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }
    }
}
