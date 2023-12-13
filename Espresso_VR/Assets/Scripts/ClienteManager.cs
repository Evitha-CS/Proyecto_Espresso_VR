using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClienteManager : MonoBehaviour
{
    public GameObject[] prefabsClientes;
    public Transform[] puntosSpawn;
    public float tiempoEntreClientes = 5f;
    public int maxClientes = 4;

    void Start()
    {
        StartCoroutine(GenerarClientes());
    }

    IEnumerator GenerarClientes()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreClientes);

            // Verificar si hay menos de maxClientes en la escena con el tag "Cliente"
            if (CountClientesEnEscena() < maxClientes)
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
}

