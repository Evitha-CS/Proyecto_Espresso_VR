using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabObject : MonoBehaviour
{
    public Image cargaImage; // Referencia al objeto Image que representa la barra de carga
    public GameObject punteroImage;
    public float tiempoCarga = 2f;
    private float tiempoTranscurrido = 0f;
    private bool objetoTocado = false;
    private Transform objetoInteractuableTransform;
    private Collider objetoInteractuableCollider;


    void Update()
    {
        if (objetoTocado)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Actualizar la barra de carga
            cargaImage.fillAmount = tiempoTranscurrido / tiempoCarga;

            if (tiempoTranscurrido >= tiempoCarga)
            {
                AgarrarObjeto();

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("interactable")) // Verificar si el otro objeto tiene el tag "interactable"
        {

            objetoInteractuableTransform = other.transform; // Almacenar la referencia al objeto interactuable y su collider
            objetoInteractuableCollider = other.GetComponent<Collider>();

            objetoTocado = true;
            punteroImage.SetActive(false);

        }

        
    }

    void OnTriggerDelete(Collider other)
    {
        if (other.CompareTag("basurero") && objetoInteractuableTransform != null)
        {
            Debug.Log("Estoy en basurero");
            Destroy(objetoInteractuableTransform.gameObject); // Destruir el objeto interactuable al tocar el basurero
            Debug.Log("Adiós :'(");
            RestablecerEstado();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            // Restablecer el estado si se deja de colisionar con el objeto interactuable
            RestablecerEstado();
        }
    }

    void AgarrarObjeto()
    {
        // Lógica para agarrar el objeto después de la carga completa
        Debug.Log("Objeto agarrado!");
        objetoInteractuableTransform.SetParent(transform); // Hacer que el objeto interactuable sea hijo del objeto original
        Debug.Log("Hola :D");

        if (objetoInteractuableCollider != null)
        {
            objetoInteractuableCollider.enabled = false; // Desactivar el collider del objeto interactuable mientras se está tocando
        }
        RestablecerEstado();
    }

    void RestablecerEstado()
    {
        punteroImage.SetActive(true);
        objetoTocado = false;
        objetoInteractuableTransform = null;
        objetoInteractuableCollider = null;
        tiempoTranscurrido = 0f;
        cargaImage.fillAmount = 0f;
    }

}


