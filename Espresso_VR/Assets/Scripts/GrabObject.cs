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
    public float tiempoRestanteEliminar = 3f;
    public float tiempoRestanteEntregar = 1f;
    private float tiempoTranscurrido = 0f;
    private float tiempoTranscurridoEliminar = 0f;
    private bool objetoTocado = false;
    private bool objetoAgarrado = false;
    private bool objetoAEliminar = false;
    private bool objetoAEntregar = false;
    private Transform objetoInteractuableTransform;
    private Collider objetoInteractuableCollider;
    private string nombreObjeto;
    public Cliente cliente;
    public GameObject referenciaComida;
    private GameObject prefabObjeto = null;

    void Update()
    {
        if (objetoTocado)
        {

            tiempoTranscurrido += Time.deltaTime;
            punteroImage.SetActive(false);
            // Actualizar la barra de carga
            cargaImage.fillAmount = tiempoTranscurrido / tiempoCarga;
            //Debug.Log("Tocando Objeto");

            if (tiempoTranscurrido >= tiempoCarga)
            {
                AgarrarObjeto();
                punteroImage.SetActive(true);
                cargaImage.fillAmount = 0f;
                objetoAgarrado = true;
            }

        }
        else
        {
            punteroImage.SetActive(true);
        }

        if (objetoAEliminar)
        {

            tiempoTranscurridoEliminar += Time.deltaTime;
            punteroImage.SetActive(false);
            cargaImage.fillAmount = tiempoTranscurridoEliminar / tiempoRestanteEliminar;
            //Debug.Log("Eliminando Objeto");

            if (tiempoTranscurridoEliminar >= tiempoRestanteEliminar)
            {
                StartCoroutine(Respawn());
                EliminarObjeto();
                cargaImage.fillAmount = 0f;
                objetoAEliminar = false;
            }
        }

        if (objetoAEntregar)
        {

            tiempoTranscurridoEliminar += Time.deltaTime;
            punteroImage.SetActive(false);
            cargaImage.fillAmount = tiempoTranscurridoEliminar / tiempoRestanteEntregar;
            //Debug.Log("Eliminando Objeto");

            if (tiempoTranscurridoEliminar >= tiempoRestanteEntregar)
            {
                StartCoroutine(Respawn());
                Destroy(objetoInteractuableTransform.gameObject);
                punteroImage.SetActive(true);
                RestablecerEstado();
                RestablecerReferencias();
                cliente.EntregarObjeto(nombreObjeto);
                objetoAEntregar = false;
            }



        }
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("interactable") && !objetoAgarrado && objetoInteractuableTransform == null) // Verificar si el otro objeto tiene el tag "interactable"
        {
            objetoInteractuableTransform = other.transform; // Almacenar la referencia al objeto interactuable y su collider
            objetoInteractuableCollider = other.GetComponent<Collider>();
            objetoTocado = true;

        }
        if (other.CompareTag("basurero") && (!objetoAEliminar))
        {

            if (objetoInteractuableTransform != null)
            {
                objetoAEliminar = true;
            }
        }
        if (other.CompareTag("cliente") && (!objetoAEntregar))
        {

            if (objetoInteractuableTransform != null)
            {
                objetoAEntregar = true;
            }
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            if (!objetoAgarrado)
            {
                // Restablecer el estado si se deja de colisionar con el objeto interactuable
                RestablecerEstado();
                RestablecerReferencias();
            }
            objetoTocado = false;
        }

        if (other.CompareTag("basurero"))
        {
            // Restablecer el estado si se deja de colisionar con el objeto basurero
            objetoAEliminar = false;

            if (!objetoAEliminar)
            {
                punteroImage.SetActive(true);
                tiempoTranscurrido = 0f;
                cargaImage.fillAmount = 0f;
                tiempoTranscurridoEliminar = 0f;

            }
            objetoTocado = false;

        }

        if (other.CompareTag("cliente"))
        {
            // Restablecer el estado si se deja de colisionar con el objeto basurero
            objetoAEntregar = false;

            if (!objetoAEntregar)
            {
                punteroImage.SetActive(true);
                tiempoTranscurrido = 0f;
                cargaImage.fillAmount = 0f;
                tiempoTranscurridoEliminar = 0f;

            }
            objetoTocado = false;

        }

    }

    void AgarrarObjeto()
    {
        objetoInteractuableTransform.SetParent(transform); // Hacer que el objeto interactuable sea hijo del objeto original
        nombreObjeto = objetoInteractuableTransform.name;
        //Debug.Log("Has agarrado un " + nombreObjeto);

    }

    void EliminarObjeto()
    {
        //Debug.Log("Estoy en basurero y soy " + objetoInteractuableTransform);

        Destroy(objetoInteractuableTransform.gameObject);
        punteroImage.SetActive(true);
        RestablecerEstado();
    }
    void RestablecerEstado()
    {
        //Debug.Log("RESTABLECIENDO ESTADOS!");

        objetoTocado = false;
        objetoAgarrado = false;
        tiempoTranscurrido = 0f;
        cargaImage.fillAmount = 0f;
    }
    void RestablecerReferencias()
    {
        //Debug.Log("RESTABLECIENDO REFERENCIAS!");
        objetoInteractuableTransform = null;
        objetoInteractuableCollider = null;
    }

    // Buscar objeto hijo
    /*string GetHijo()
    {
        if (transform.Find("croissant"))
            return "croissant";
        if (transform.Find("donut"))
            return "donut";
        if (transform.Find("cupcake"))
            return "cupcake";
        if (transform.Find("te"))
            return "té";
        return null;
    }*/

    //Esperar y respawnear objetos
    IEnumerator Respawn()
    {
        Debug.Log("Esperando para respawnear: " + nombreObjeto);
        yield return new WaitForSeconds(1);

        GameObject nuevoObjeto = null;

        switch (nombreObjeto)
        {
            case "limonada":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/limonada");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-0.06508446f, -0.9267671f, 0.206365f), Quaternion.identity, referenciaComida.transform);
                break;

            case "jugo de naranja":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/jugo de naranja");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-0.06524086f, -0.9265893f, 0.6109225f), Quaternion.identity, referenciaComida.transform);
                break;

            case "galletas":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/galletas");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(7.145f, -0.996f, 1.85f), Quaternion.identity, referenciaComida.transform);
                break;

            case "galletas de chocolate":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/galletas de chocolate");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(6.625f, -0.996f, 1.85f), Quaternion.identity, referenciaComida.transform);
                break;

            case "donut crema":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut crema");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-3.330679f, 0.554322f, 5.076561f), Quaternion.identity, referenciaComida.transform);
                break;

            case "donut chocolate":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut chocolate");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-3.671754f, 0.5537694f, 5.086428f), Quaternion.identity, referenciaComida.transform);
                break;

            case "donut strawberry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut strawberry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-4.037201f, 0.5541945f, 5.098f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cupcake":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-3.297068f, 0.8456686f, 5f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cupcake redvelvet":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake redvelvet");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-3.826f, 0.8460448f, 5f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cupcake oreo":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake oreo");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-3.57f, 0.8466511f, 5f), Quaternion.identity, referenciaComida.transform);
                break;
            case "cupcake cherry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake cherry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-4.099f, 0.8451772f, 5f), Quaternion.identity, referenciaComida.transform);
                break;

            case "croissant":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/croissant");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-0.42f, -0.1f, 5.097f), Quaternion.identity, referenciaComida.transform);
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