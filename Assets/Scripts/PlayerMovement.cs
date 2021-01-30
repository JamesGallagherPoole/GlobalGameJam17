using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float jumpForce;
    public float heliHatJumpForce;
    public float slideBoostForce;
    public float trampolineJumpForce;

    private bool usedHeliJump;
    private bool usedSlideOfConfidence;
    private bool isSliding;

    private bool triggerTrampolineJump;

    public MechanicsManager mechanicsManager;

    private bool isGrounded;
    private bool isOnSlope;
    private bool isUnderCeiling;

    public Transform groundCheck;
    public Transform slopeCheck;
    public Transform ceilingCheck;

    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        usedHeliJump = false;
        usedSlideOfConfidence = false;
        isSliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollisions();

        // Walky Left
        if (Input.GetKey("d") || Input.GetKey("right")) {
            if (!isSliding || !isGrounded) {
                // On Slope Movement
                if (isOnSlope && isGrounded)
                    rigidbody.velocity = new Vector2(movementSpeed/1.5f, rigidbody.velocity.y);
                // Everywhere Else Movement
                else
                    rigidbody.velocity = new Vector2(movementSpeed, rigidbody.velocity.y);
            }
            spriteRenderer.flipX = false;
            //TODO Animate run right
        }

        // Walky Right
        else if (Input.GetKey("a") || Input.GetKey("left")) {
            if (!isSliding || !isGrounded) {
                // On Slope Movement
                if (isOnSlope && isGrounded)
                    rigidbody.velocity = new Vector2(-movementSpeed/1.5f, rigidbody.velocity.y);
                // Everywhere Else Movement
                else
                    rigidbody.velocity = new Vector2(-movementSpeed, rigidbody.velocity.y);
            }

            spriteRenderer.flipX = true; 
            //TODO Animate run left 
        }
        else {
            //TODO Animate idle
        }

        // Floaty after Helicopter Hat
        if (Input.GetKey("space") && mechanicsManager.hasHeliHat && rigidbody.velocity.y < 0)
            rigidbody.drag = 5f; 
            // TODO Animate floating
        else
            rigidbody.drag = 0f;

        // Jumpy Uppy
        if (Input.GetKeyDown("space") && !isSliding) {
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

        // Slide of self confidence
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !usedSlideOfConfidence && mechanicsManager.hasSuperShoes) {
            usedSlideOfConfidence = true;
            isSliding = true;

            if (rigidbody.velocity.x > 0f)
                rigidbody.AddForce(new Vector2(slideBoostForce, 0));
            else if (rigidbody.velocity.x < 0f)
                rigidbody.AddForce(new Vector2(-slideBoostForce, 0));

            boxCollider.offset = new Vector2(0f, -0.5f);
            boxCollider.size = new Vector2(1f, 1f);
        }

        // Reset heli jump once you have landed
        if (isGrounded) {
            usedHeliJump = false;
        }

        // Exit out of a confident slide once you go under a certain speed
        if (isSliding && rigidbody.velocity.x < 3f && rigidbody.velocity.x > -3f || triggerTrampolineJump) {
            boxCollider.offset = new Vector2(0f, -.05f);
            boxCollider.size = new Vector2(1f, 1.9f);
            usedSlideOfConfidence = false;
            isSliding = false;
        }

        // If a trampoline jump has been triggered, do the jump silly!
        if (triggerTrampolineJump) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, trampolineJumpForce);
            triggerTrampolineJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Trigger a trampoline jump!
        if(collider.gameObject.tag == "Trampoline")
        {
            triggerTrampolineJump = true;
        }
    }

    private void DetectCollisions()
    {
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

        if (Physics2D.Linecast(transform.position, ceilingCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isUnderCeiling = true;
        }
        else
        {
            isUnderCeiling = false;
        }
    }
}