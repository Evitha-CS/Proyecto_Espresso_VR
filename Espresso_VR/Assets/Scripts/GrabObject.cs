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
            cliente = other.GetComponent<Cliente>();
            if (objetoInteractuableTransform != null)
            {
                Debug.Log("Cliente ahora es " + cliente.name);
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
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.330084f, 0.5366147f, -0.7604402f), Quaternion.identity, referenciaComida.transform);
                break;

            case "jugo de naranja":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/jugo de naranja");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.330241f, 0.5367925f, -0.3558828f), Quaternion.identity, referenciaComida.transform);
                break;

            case "galletas":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/galletas");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(0.8800001f, 0.4673818f, 0.8831947f), Quaternion.identity, referenciaComida.transform);
                break;

            case "galletas de chocolate":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/galletas de chocolate");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(0.3600001f, 0.4673818f, 0.8831947f), Quaternion.identity, referenciaComida.transform);
                break;

            case "donut crema":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut crema");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-9.595679f, 2.017704f, 4.109756f), Quaternion.identity, referenciaComida.transform);
                break;

            case "donut chocolate":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut chocolate");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-9.936754f, 2.017151f, 4.119623f), Quaternion.identity, referenciaComida.transform);
                break;

            case "donut strawberry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/donut strawberry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-10.3022f, 2.017576f, 4.131195f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cupcake":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-9.562068f, 2.30905f, 4.033195f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cupcake redvelvet":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake redvelvet");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-10.091f, 2.309427f, 4.033195f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cupcake oreo":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake oreo");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-9.835f, 2.310033f, 4.033195f), Quaternion.identity, referenciaComida.transform);
                break;
            case "cupcake cherry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cupcake cherry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-10.364f, 2.308559f, 4.033195f), Quaternion.identity, referenciaComida.transform);
                break;

            case "croissant":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/croissant");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.685f, 1.363382f, 4.130195f), Quaternion.identity, referenciaComida.transform);
                break;

            case "pack sandwich":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/pack sandwich");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.186f, 1.2f, 0.628f), Quaternion.identity, referenciaComida.transform);
                break;

            case "frenchtoast con aceitunas":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/frenchtoast con aceitunas");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.444f, 1.187f, -0.268f), Quaternion.identity, referenciaComida.transform);
                break;

            case "frenchtoast con huevo":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/frenchtoast con huevo");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.444f, 1.187f, -0.74f), Quaternion.identity, referenciaComida.transform);
                break;

            case "frenchtoast de naranja":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/frenchtoast de naranja");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.007f, 1.187f, -0.74f), Quaternion.identity, referenciaComida.transform);
                break;

            case "frenchtoast tomate cherry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/frenchtoast tomate cherry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.007f, 1.187f, -0.268f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cheesecake blueberry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cheesecake blueberry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.071f, 0.57f, 0.468f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cheesecake chocolate":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cheesecake chocolate");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.088f, 0.57f, 0.835f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cheesecake limon":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cheesecake limon");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.42f, 0.57f, 0.756f), Quaternion.identity, referenciaComida.transform);
                break;

            case "cheesecake strawberry":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/cheesecake strawberry");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-6.356f, 0.57f, 0.469f), Quaternion.identity, referenciaComida.transform);
                break;

            case "espresso":
                prefabObjeto = Resources.Load<GameObject>("CoffeeShopStarterPack/Prefabs/espresso");
                nuevoObjeto = Instantiate(prefabObjeto, new Vector3(-10.532f, 1.403f, -0.305f), Quaternion.identity, referenciaComida.transform);
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