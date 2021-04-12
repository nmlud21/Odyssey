using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 90f;
    private float xRotation = 0f;

    public Transform playerBody;
    
    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused == false)
        {
            //Gets which axis your mouse is moving on
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //decrease rotation based on mouse Y
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); //limits rotation to 180 to avoid looking behind the player
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //Applies the rotation
            //rotates body on x-axis
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
