using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int playerSpeed;
    public float rotationSpeed; // Nueva variable para controlar la velocidad de rotación
    public Transform vrCamera;
    public float toggleAngle;
    
    public bool moveForward;

    private Rigidbody rb;
    public Transform playerModel; // Referencia al modelo 3D

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

        // Calcula la rotación en el plano Y (vertical) de la cámara
        float yRotation = vrCamera.eulerAngles.y;

        // Aplica la rotación al objeto hijo (modelo 3D) 
        playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.Euler(0, yRotation, 0), rotationSpeed * Time.deltaTime);

    }

}
