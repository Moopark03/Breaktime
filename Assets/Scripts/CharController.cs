using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    Vector3 forward, right;

    // Start is called before the first frame update
    void Start()
    {
        forward.x = 1; //gets the forward direction from the camera perspective
        forward.y = 0;
        forward = Vector3.Normalize(forward); //normalize changes the magnitude of forward to be 1 towards the forward direction
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        //forward gets the vector pointing towards the camera forward. The quaternion.eulur with y = 90 gives us a 90 degree turn
        // around the Y axis. Multiplying the euler vector going x=0, y=90, z = 0 with the forward vector 7, 0, 7, gives the perpendicular
        // vector to the right of what is considered forward
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)//need Jump and Swing
        {
            Move();
        }
    }

    private void Move()
    {
        
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey"); //can be negative
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey"); //can be negative
        //these make sense if you understand forward and right vector, then multiply by speed and directional key that will be negative or positive

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        
        transform.forward = heading; //changes the forward direction (facing) without heading it is strafing
        transform.position += rightMovement;
        transform.position += upMovement;
        
        
    }

    
}
