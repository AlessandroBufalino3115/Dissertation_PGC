using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    

    
    
    private void Update()
    {

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) 
        {
            movement += Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += Vector3.back * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector3.left * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            movement += Vector3.up * speed;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement += Vector3.down * speed;
        }


        float mouseMovementY = Input.GetAxis("Mouse Y");
        float mouseMovementX = Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.Mouse1)) 
        {
            movement += Vector3.up * mouseMovementY * -speed ;
            movement += Vector3.right * mouseMovementX * -speed ;
        }



        if (Input.GetKey(KeyCode.Mouse2))
        {
            transform.RotateAround(transform.position, transform.right, mouseMovementY * -3.5f);
            transform.RotateAround(transform.position, Vector3.up, mouseMovementX * 3.5f);
        }


        transform.Translate(movement);
    }





}
