using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   public int playerSpeed;
   public Transform vrCamera;
   public float toggleAngle;
   public bool moveForward;

    // Update is called once per frame
    void Update()
    {

        if(vrCamera.eulerAngles.x >= toggleAngle && vrCamera.eulerAngles.x < 90.00f)
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;
        }
        if (moveForward)
        {
            transform.position = transform.position + Camera.main.transform.forward * playerSpeed * Time.deltaTime;
        }

        
    }
}
