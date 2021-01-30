using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1;
    public float jumpForce = 1;

    public Rigidbody2D playerRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement * movementSpeed * Time.deltaTime, 0, 0);
    
        if (Input.GetButtonDown("Jump") && Mathf.Abs(playerRigidBody.velocity.y) < 0.001f) {
            playerRigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}
