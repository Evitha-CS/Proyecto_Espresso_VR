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
    private bool objetoAgarrado = false;
    private Transform objetoInteractuableTransform;
    private Collider objetoInteractuableCollider;
    private string nombreObjeto;
    public Cliente cliente;
    public GameObject referenciaComida;
    private GameObject prefabObjeto = null;

    void Update()
    {
        if (objetoTocado && !objetoAgarrado)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Actualizar la barra de carga
            cargaImage.fillAmount = tiempoTranscurrido / tiempoCarga;

            if (tiempoTranscurrido >= tiempoCarga)
            {
                AgarrarObjeto();
                punteroImage.SetActive(false);
                cargaImage.fillAmount = 0f;
                objetoAgarrado = true;
                tiempoTranscurrido = 0f;
            }
            
        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("interactable") && (!objetoTocado || !objetoAgarrado)) // Verificar si el otro objeto tiene el tag "interactable"
        {
            objetoInteractuableTransform = other.transform; // Almacenar la referencia al objeto interactuable y su collider
            objetoInteractuableCollider = other.GetComponent<Collider>();
            objetoTocado = true;

        }
        if (other.CompareTag("basurero"))
        {
        
            if (objetoInteractuableTransform != null)
            {

                StartCoroutine(Respawn());
                Destroy(objetoInteractuableTransform.gameObject);
                punteroImage.SetActive(true);
                RestablecerEstado();
                RestablecerReferencias();
                cliente.EntregarObjeto(nombreObjeto);
                
            }
        }
    }

    

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            if(!objetoAgarrado){
                // Restablecer el estado si se deja de colisionar con el objeto interactuable
                RestablecerEstado();
                RestablecerReferencias();
            }
            objetoTocado = false;
            
        }
        
        
    }

    void AgarrarObjeto()
    {
        objetoInteractuableTransform.SetParent(transform); // Hacer que el objeto interactuable sea hijo del objeto original
        nombreObjeto = objetoInteractuableTransform.name;
        Debug.Log("Has agarrado un " + nombreObjeto);

    }
    void RestablecerEstado()
    {
        Debug.Log("RESTABLECIENDO ESTADOS!");

        objetoTocado = false;
        objetoAgarrado = false;
        tiempoTranscurrido = 0f;
        cargaImage.fillAmount = 0f;
    }
    void RestablecerReferencias()
    {
        Debug.Log("RESTABLECIENDO REFERENCIAS!");
        objetoInteractuableTransform = null;
        objetoInteractuableCollider = null;
    }

    // Buscar objeto hijo
    string GetHijo(){
        if(transform.Find("croissant"))
            return "croissant";
        if(transform.Find("donut"))
            return "donut";
        if(transform.Find("cupcake"))
            return "cupcake";
        if(transform.Find("te"))
            return "té";
        return null;
    }

    //Esperar y respawnear objetos
IEnumerator Respawn()
{
    Debug.Log("Esperando para respawnear: " + nombreObjeto);
    yield return new WaitForSeconds(1);

    GameObject nuevoObjeto = null;

    switch (nombreObjeto)
    {
        case "croissant":
            prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/croissant");
            nuevoObjeto = Instantiate(prefabObjeto, new Vector3(7.961983f, 1.282f, -1.3f), Quaternion.identity, referenciaComida.transform);
            break;

        case "donut":
            prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut");
            nuevoObjeto = Instantiate(prefabObjeto, new Vector3(7.122385f, 1.185131f, -3.023491f), Quaternion.identity, referenciaComida.transform);
            break;

        case "cupcake":
            prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake");
            nuevoObjeto = Instantiate(prefabObjeto, new Vector3(7.554285f, 1.185016f, -3.023879f), Quaternion.identity, referenciaComida.transform);
            break;

        case "té":
            prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/te");
            nuevoObjeto = Instantiate(prefabObjeto, new Vector3(6.679826f, 1.285f, -1.342f), Quaternion.identity, referenciaComida.transform);
            break;
    }

    if (nuevoObjeto != null)
    {
        Debug.Log("Respawneando " + prefabObjeto.name);
        // Instanciar el nuevo objeto con la posición y rotación almacenadas
        //GameObject nuevoObjeto = Instantiate(prefabObjeto, new Vector3(1f, 1f, 0f), Quaternion.identity, referenciaComida.transform);
        nuevoObjeto.name = nombreObjeto;
    }
    else
    {
        Debug.LogError("No se pudo instanciar el objeto con el nombre " + nombreObjeto);
    }
}
}