using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cliente : MonoBehaviour
{
    public string objetoCorrecto;  // Define el objeto correcto que el cliente espera recibir
    private string objetoAnterior; //Para evitar que se elija el mismo objeto consecutivamente
    private TextMeshProUGUI ObjetoSolicitado;
    public TextMeshProUGUI ResultadoEntrega;
    public float tiempoInterfaz;

    void Start()
    {
        Transform canvas = transform.Find("Canvas");
        Transform tmp = canvas.Find("Text (TMP)");
        ObjetoSolicitado = tmp.GetComponent<TextMeshProUGUI>();
        ElegirObjeto();  // Al inicio, el cliente elige un objeto aleatorio

    }

    void ElegirObjeto()
    {
        // Lista de posibles objetos que el cliente puede elegir
        string[] objetosDisponibles = { 
            "limonada",
            "jugo de naranja",
            "galletas",
            "galletas de chocolate",
            "donut crema", 
            "donut chocolate", 
            "donut strawberry", 
            "cupcake", 
            "cupcake redvelvet", 
            "cupcake oreo", 
            "cupcake cherry", 
            "croissant", 
            "pack sandwich", 
            "frenchtoast con aceitunas", 
            "frenchtoast con huevo", 
            "frenchtoast de naranja", 
            "frenchtoast tomate cherry",
            "cheesecake blueberry",
            "cheesecake chocolate",
            "cheesecake limon",
            "cheesecake strawberry",
            "espresso"
            };

        // Elegir un objeto diferente al anteriormente seleccionado
        do
        {
            int indiceObjeto = Random.Range(0, objetosDisponibles.Length);
            objetoCorrecto = objetosDisponibles[indiceObjeto];
        } while (objetoCorrecto == objetoAnterior);

        // Actualizar el objeto anterior con el nuevo objeto seleccionado
        objetoAnterior = objetoCorrecto;

        // Mostrar mensaje en consola
        Debug.Log("Cliente quiere: " + objetoCorrecto);
        ObjetoSolicitado.text = objetoCorrecto;
    }

    // Método llamado cuando el jugador entrega un objeto
    public void EntregarObjeto(string objetoEntregado)
    {
        // Verificar si el objeto entregado es el correcto
        if (objetoEntregado == objetoCorrecto)
        {
            Debug.Log("¡Entrega exitosa! El cliente recibió el objeto correcto: " + objetoCorrecto);
            ResultadoEntrega.text = "¡Entrega exitosa! El cliente recibió el objeto correcto: " + objetoCorrecto;
        }
        else
        {
            Debug.Log("Entrega fallida. El cliente esperaba: " + objetoCorrecto + ", pero recibió: " + objetoEntregado);
            ResultadoEntrega.text = "Entrega fallida. El cliente esperaba: " + objetoCorrecto + ", pero recibió: " + objetoEntregado;
        }
        ResultadoEntrega.gameObject.SetActive(true);
        StartCoroutine(DesaparecerDespuesDeTiempo(tiempoInterfaz));
        Debug.Log("Estoy aquí");
        // Después de la entrega, el cliente elige otro objeto
        ElegirObjeto();
    }

    IEnumerator DesaparecerDespuesDeTiempo(float tiempo)
    {   
        Debug.Log("Esperando para desaparecer interfaz");
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(tiempo);
        Debug.Log("Desapareciendo interfaz");
        // Desactivar el objeto del texto o establecer el texto en una cadena vacía
        ResultadoEntrega.gameObject.SetActive(false);
    }
}

