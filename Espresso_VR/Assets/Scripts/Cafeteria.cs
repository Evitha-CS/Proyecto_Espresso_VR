using UnityEngine;

public class Cafeteria : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // El jugador ha chocado con la casa, puedes hacer algo aquí si es necesario.
            // Por ejemplo, detener el movimiento del jugador.
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("Ha llegado el jugador");
        }
    }
}
