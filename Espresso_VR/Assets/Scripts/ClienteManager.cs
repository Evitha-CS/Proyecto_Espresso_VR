using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClienteManager : MonoBehaviour
{
    public GameObject[] prefabsClientes;
    public Transform[] puntosSpawn;
    public ScoreManager player;
    public float tiempoEntreClientes = 5f;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<ScoreManager>();
        StartCoroutine(GenerarClientes());
    }

    IEnumerator GenerarClientes()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreClientes);

            // Verificar si hay menos de maxClientes en la escena con el tag "Cliente"
            if (CountClientesEnEscena() < MaxClientes() )
            {
                Debug.Log("Spawneando cliente");
                GameObject clientePrefab = prefabsClientes[Random.Range(0, prefabsClientes.Length)];
                Transform puntoSpawn = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
                Instantiate(clientePrefab, puntoSpawn.position, puntoSpawn.rotation);
            }
            else
            {
                //Debug.Log("Hay demasiados clientes!");
            }
        }
    }

    int CountClientesEnEscena()
    {
        // Contar cuántos objetos en la escena tienen el tag "cliente"
        //Debug.Log("Contando clientes");
        GameObject[] clientes = GameObject.FindGameObjectsWithTag("cliente");
        return clientes.Length;
    }

    int MaxClientes()
    {
        if(player.puntuacion<6)
        {
            return 2;
        }
        else
        {
            if(player.puntuacion<=20)
            {
                return (player.puntuacion/2);
            }
            else
            {
                return 10;
            }
        }
    }
}

