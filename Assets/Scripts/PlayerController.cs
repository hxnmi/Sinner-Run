using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private CharacterController controller;
    private Vector3 direction;
    private Vector3 lastDie;
    private float speedRun;

    [Header("Lane Movement")]
    [SerializeField] private int desiredLane = 1;
    [SerializeField] private float laneDistance = 4;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float Gravity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        direction.z = speed;
        

        if(controller.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else{
            direction.y += Gravity * Time.deltaTime;
        }
        

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if(desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            desiredLane--;
            if(desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if(desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if(transform.position == targetPosition)
            return;
        
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        
        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void FixedUpdate() 
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if(hit.transform.tag == "Obstacle")
    //     {
    //         PlayerManager.gameOver = true;
    //         Time.timeScale = 0;
    //     }else if(hit.transform.tag == "Boost")
    //     {
    //         GetComponent<Collider>().enabled = false;
    //         speed = 100;
    //         isInvisible = true;
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            Time.timeScale = 0;
        }
    }

}
