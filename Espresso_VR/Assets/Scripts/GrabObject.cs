using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private bool objetoTocado = false;
    private Transform objetoInteractuableTransform;
    private Collider objetoInteractuableCollider;

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("interactable")) // Verificar si el otro objeto tiene el tag "interactable"
        {
            
            objetoInteractuableTransform = other.transform; // Almacenar la referencia al objeto interactuable y su collider
            objetoInteractuableCollider = other.GetComponent<Collider>();

            objetoTocado = true;

            
            objetoInteractuableTransform.SetParent(transform); // Hacer que el objeto interactuable sea hijo del objeto original
            Debug.Log("Hola :D");
            
            if (objetoInteractuableCollider != null)
            {
                objetoInteractuableCollider.enabled = false; // Desactivar el collider del objeto interactuable mientras se está tocando
            }

            // Puedes realizar más acciones aquí si lo necesitas
        }

        if (other.CompareTag("basurero") && objetoInteractuableTransform != null)
    {
        
        Destroy(objetoInteractuableTransform.gameObject); // Destruir el objeto interactuable al tocar el basurero
        Debug.Log("Adiós :'(");
        objetoTocado = false; // Restablecer el estado
        objetoInteractuableTransform = null;
        objetoInteractuableCollider = null;
    }
    }

}
   

