using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cliente : MonoBehaviour
{
    public string objetoCorrecto;  // Define el objeto correcto que el cliente espera recibir
    private string objetoAnterior; //Para evitar que se elija el mismo objeto consecutivamente
    public TextMeshProUGUI ObjetoSolicitado;
    public TextMeshProUGUI ResultadoEntrega;
    public ScoreManager player;
    public float tiempoInterfaz;
    private MovimientoCliente scriptMovimiento;
    private Transform canvas;

    void Start()
    {
        canvas = transform.Find("Canvas");
        Transform tmp = canvas.Find("Text (TMP)");
        ObjetoSolicitado = tmp.GetComponent<TextMeshProUGUI>();
        ResultadoEntrega = GameObject.Find("Text (TMP) (1)").GetComponent<TextMeshProUGUI>();
        GameObject playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<ScoreManager>();
        scriptMovimiento = GetComponent<MovimientoCliente>();
        ElegirObjeto();  // Al inicio, el cliente elige un objeto aleatorio

    }

    void Update()
    {
        GirarHaciaCamara(canvas, Camera.main.transform);
    }

    public void GirarHaciaCamara(Transform objeto, Transform camara)
    {
        // Obtener la dirección desde el objeto hacia la cámara
        Vector3 direccionCamara = camara.position - objeto.position;

        // Ignorar la rotación en Y para que el objeto gire solo en su eje vertical
        direccionCamara.y = 0;

        // Obtener la rotación necesaria para que el objeto apunte hacia la cámara
        Quaternion rotacionObjeto = Quaternion.LookRotation(direccionCamara);

        // Aplicar la rotación al objeto
        objeto.rotation = rotacionObjeto;
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
            // Aumentar puntuación
            player.ActualizarPuntuación(1);
        }
        else
        {
            Debug.Log("Entrega fallida. El cliente esperaba: " + objetoCorrecto + ", pero recibió: " + objetoEntregado);
            ResultadoEntrega.text = "Entrega fallida. El cliente esperaba: " + objetoCorrecto + ", pero recibió: " + objetoEntregado;
            // Disminuir puntuación
            player.ActualizarPuntuación(-1);
        }
        // Mostrar el mensaje de éxito y luego desaparecer la interfaz
        ResultadoEntrega.gameObject.SetActive(true);
        StartCoroutine(DesaparecerDespuesDeTiempo(tiempoInterfaz));
        // Hacer que el cliente se vaya
        scriptMovimiento.EstablecerNuevoDestino((new Vector3(-2.762f, 0f, 6.256f)));
        StartCoroutine(DespawnearCliente(5f));

    }

    IEnumerator DesaparecerDespuesDeTiempo(float tiempo)
    {   
        Debug.Log("Esperando para desaparecer interfaz");
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(tiempo);
        Debug.Log("Desapareciendo interfaz");
        // Desactivar el objeto del texto o establecer el texto en una cadena vacía
        ResultadoEntrega.text = "";
    }

    IEnumerator DespawnearCliente(float tiempo)
    {
        Debug.Log("NO QUIERO MORIR!!!");
        //Se le quita la referencia al TMP para que no se destruya
        //ResultadoEntrega=null;
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(tiempo);
        Debug.Log("*muerto*");
        // Destruir al cliente
        Destroy(gameObject);
    }
}

