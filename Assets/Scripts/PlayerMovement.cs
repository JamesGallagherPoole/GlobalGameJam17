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
    private float currentMovementSpeed;

    private int currentGameState;

    private bool triggerTrampolineJump;

    public MechanicsManager mechanicsManager;
    public GameStateManager gameStateManager;

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

        gameStateManager.gameStateChangeEvent.AddListener(UpdateState);

        usedHeliJump = false;
        usedSlideOfConfidence = false;
        isSliding = false;
    }

    void OnDestroy()
    {
        gameStateManager.gameStateChangeEvent.RemoveListener(UpdateState);
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollisions();

        CheckCurrentAbilities();

        // Walky Left
        if (Input.GetKey("d") || Input.GetKey("right")) {
            if (!isSliding || !isGrounded) {
                // On Slope Movement
                if (isOnSlope && isGrounded)
                    rigidbody.velocity = new Vector2(currentMovementSpeed/1.5f, rigidbody.velocity.y);
                // Everywhere Else Movement
                else
                    rigidbody.velocity = new Vector2(currentMovementSpeed, rigidbody.velocity.y);
            }
            spriteRenderer.flipX = false;
            //TODO Animate run right
        }

        // Walky Right
        else if (Input.GetKey("a") || Input.GetKey("left")) {
            if (!isSliding || !isGrounded) {
                // On Slope Movement
                if (isOnSlope && isGrounded)
                    rigidbody.velocity = new Vector2(-currentMovementSpeed/1.5f, rigidbody.velocity.y);
                // Everywhere Else Movement
                else
                    rigidbody.velocity = new Vector2(-currentMovementSpeed, rigidbody.velocity.y);
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
        if (collider.gameObject.tag == "Trampoline")
        {
            triggerTrampolineJump = true;
        }

        // Trigger a pickup!
        if (collider.gameObject.tag == "HeliPickup")
        {
            mechanicsManager.hasHeliHat = true;
            collider.gameObject.SetActive(false);
            Debug.Log("Picked Up Hat!");
        }

        // Trigger a task done!
        if (collider.gameObject.tag == "TaskCheckmark")
        {
            TaskCheckmark task = collider.gameObject.GetComponent<TaskCheckmark>();
            task.CheckOffTask();
        }

        // Trigger Game State UP!
        if (collider.gameObject.tag == "GameStateIncrease")
        {
            gameStateManager.GameStateIncrease();
        }

        // Trigger Game State Down!
        if (collider.gameObject.tag == "GameStateDecrease")
        {
            gameStateManager.GameStateDecrease();
        }

        if (collider.gameObject.tag == "ShoesPickup")
        {
            mechanicsManager.hasSuperShoes = true;
            collider.gameObject.SetActive(false);
            Debug.Log("Picked Up Shoes!");
        }

    }

    private void UpdateState(int newGameState)
    {
        currentGameState = newGameState;
    }

    private void CheckCurrentAbilities()
    {
        // Check if we have changed state and need to lose some items
        if (currentGameState < 2 && mechanicsManager.hasHeliHat == true) 
        {
            // Lose the heli hat :(
            mechanicsManager.hasHeliHat = false;
            Debug.Log("You lost your hat! :(");
            // TODO Trigger hat fall off animation!
        }
        else if (currentGameState < 1 && mechanicsManager.hasSuperShoes == true)
        {
            // Lose the Super Shoes :(
            mechanicsManager.hasSuperShoes = false;
            Debug.Log("You lost your shoes! :(");
            // TODO Trigger shoes fall off
        }

        // Adjust Movement Speed
        if (currentGameState == 2)
            currentMovementSpeed = movementSpeed;
        else if (currentGameState == 1)
            currentMovementSpeed = movementSpeed - 1;
        else if (currentGameState == 0)
            currentMovementSpeed = movementSpeed - 2;
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