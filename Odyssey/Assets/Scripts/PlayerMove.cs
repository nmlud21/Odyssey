using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance = 0.4f; //radius of sphere using to check ground collisions
    public LayerMask groundMask; //controls what objects the sphere checks for

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    private bool isGrounded;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isSprinting;

    private Vector3 velocity;
    
    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused == false || CrystalInteract.gameOver == false) //if game is not paused
        {
            //checks for being on ground
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //gets axis for movement
            Vector3 move = transform.right * x + transform.forward * z;

            if (Input.GetKey(KeyCode.LeftShift) && isWalking)
            {
                speed = 22;
                isSprinting = true;
            }
            else
            {
                speed = 12;
                isSprinting = false;
            }
        
            if (x != 0 || z != 0)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            controller.Move(move * speed * Time.deltaTime);
        
            //Jump if spacebar is pressed
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            //Applies gravity to the player
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}