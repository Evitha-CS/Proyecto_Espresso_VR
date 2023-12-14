using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed;
    public Transform vrCamera;
    public float toggleAngle;

    public bool moveForward;

    private Rigidbody rb;

    private GrabObject grabObject;
    private bool basurero = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabObject = FindObjectOfType<GrabObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (basurero)
        {
            moveForward = false;
        }

        else

        if (vrCamera.eulerAngles.x >= toggleAngle && vrCamera.eulerAngles.x < 90.0f && (grabObject.objetoTocado == false || grabObject.objetoAgarrado))
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

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que ha entrado tiene el tag deseado
        if (other.CompareTag("basurero") && grabObject.objetoAgarrado == true)
        {
            basurero = true;
            Debug.Log("tocando basurero");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("basurero"))
        {
            basurero = false;
            Debug.Log("ya no tocas el basurero");
        }
    }


}

