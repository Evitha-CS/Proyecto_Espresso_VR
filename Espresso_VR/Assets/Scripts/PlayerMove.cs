using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int playerSpeed;
    public Transform vrCamera;
    public float toggleAngle;
    
    public bool moveForward;

    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vrCamera.eulerAngles.x >= toggleAngle && vrCamera.eulerAngles.x < 90.00f)
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;
        }

        // Dirección hacia adelante de la cámara
        Vector3 forward = vrCamera.TransformDirection(Vector3.forward);

        if (moveForward)
        {
            // Aplica el movimiento al Rigidbody usando MovePosition
            rb.MovePosition(rb.position + forward * playerSpeed * Time.deltaTime);
        }

    }

}

