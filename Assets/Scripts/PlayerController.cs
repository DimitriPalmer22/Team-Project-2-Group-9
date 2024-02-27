using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    // Update is called once per frame
    void Update()
    {
        // Get the player's movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create a vector to move the player
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Move the player
        controller.Move(move * speed * Time.deltaTime);
    }
}