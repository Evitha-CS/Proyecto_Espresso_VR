using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cliente : MonoBehaviour
{
    public string objetoCorrecto;  // Define el objeto correcto que el cliente espera recibir

    void Start()
    {
        ElegirObjeto();  // Al inicio, el cliente elige un objeto aleatorio
    }

    void ElegirObjeto()
    {
        // Lista de posibles objetos que el cliente puede elegir
        string[] objetosDisponibles = { "donut", "té", "cupcake", "croissant" };

        // Elegir un objeto aleatorio de la lista
        int indiceObjeto = Random.Range(0, objetosDisponibles.Length);
        string objetoElegido = objetosDisponibles[indiceObjeto];

        // Asignar el objeto elegido como el objeto correcto
        objetoCorrecto = objetoElegido;

        // Mostrar mensaje en consola
        Debug.Log("Cliente quiere: " + objetoCorrecto);
    }

    // Método llamado cuando el jugador entrega un objeto
    public void EntregarObjeto(string objetoEntregado)
    {
        // Verificar si el objeto entregado es el correcto
        if (objetoEntregado == objetoCorrecto)
        {
            Debug.Log("¡Entrega exitosa! El cliente recibió el objeto correcto: " + objetoCorrecto);
        }
        else
        {
            Debug.Log("Entrega fallida. El cliente esperaba: " + objetoCorrecto);
        }

        // Después de la entrega, el cliente elige otro objeto
        ElegirObjeto();
    }
}

