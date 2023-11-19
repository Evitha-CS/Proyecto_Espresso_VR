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
                punteroImage.SetActive(false);
                cargaImage.fillAmount = 0f;
            }
            
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("interactable") && !objetoTocado) // Verificar si el otro objeto tiene el tag "interactable"
        {
            objetoInteractuableTransform = other.transform; // Almacenar la referencia al objeto interactuable y su collider
            objetoInteractuableCollider = other.GetComponent<Collider>();

            objetoTocado = true;

        }
        if (other.CompareTag("basurero"))
        {
        

        if (objetoInteractuableTransform != null)
        {

            Debug.Log("Estoy en basurero y soy " + objetoInteractuableTransform);
            Destroy(objetoInteractuableTransform.gameObject);
            punteroImage.SetActive(true);
            RestablecerEstado();
            RestablecerReferencias();
            
        }else{
            Debug.Log("Parece que aquí no hay nada.");
        }
        }

    }
/*
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            // Restablecer el estado si se deja de colisionar con el objeto interactuable
            RestablecerEstado();
        }
    }*/

    void AgarrarObjeto()
    {
        // Lógica para agarrar el objeto después de la carga completa
        Debug.Log("Objeto agarrado!");
        objetoInteractuableTransform.SetParent(transform); // Hacer que el objeto interactuable sea hijo del objeto original
        Debug.Log("Hola :D. Soy " + objetoInteractuableTransform);
        
    }
    void RestablecerEstado()
    {
        Debug.Log("RESTABLECIENDO ESTADOS!");

        objetoTocado = false;
        tiempoTranscurrido = 0f;
        cargaImage.fillAmount = 0f;
    }
    void RestablecerReferencias()
    {
        Debug.Log("RESTABLECIENDO REFERENCIAS!");
        objetoInteractuableTransform = null;
        objetoInteractuableCollider = null;
    }

    
}

    
       

