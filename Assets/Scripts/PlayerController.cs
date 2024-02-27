using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// The speed at which the player moves
    /// </summary>
    [Header("Player Controller Fields")] public float speed = 12f;

    // Reference to the character controller 
    private CharacterController _controller;

    [Header("Mouse Fields")] public float mouseSensitivity;

    private Transform _playerTransform;

    private float _xRotation;

    private Transform _cameraTransform;

    #endregion Fields

    private void Start()
    {
        // Set the character controller to the attached character controller
        _controller = GetComponent<CharacterController>();

        // Set the player body to the player's body
        Cursor.lockState = CursorLockMode.Locked;

        // Set the player body to the player's body
        _playerTransform = transform;

        // Set the player's camera to the camera child
        _cameraTransform = transform.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player's camera
        MouseMovement();

        // Move the player
        PlayerMovement();
    }

    /// <summary>
    /// Move the player
    /// </summary>
    private void PlayerMovement()
    {
        // Get the player's movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create a vector to move the player
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player
        _controller.Move(move * speed * Time.deltaTime);
    }

    /// <summary>
    /// Move the player's camera
    /// </summary>
    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerTransform.Rotate(Vector3.up * mouseX);
    }
}