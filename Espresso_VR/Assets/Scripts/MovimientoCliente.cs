using UnityEngine;
using UnityEngine.AI;

public class MovimientoCliente : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EstablecerNuevoDestino((new Vector3(-5.3f, 0f, 2.3f)));
    }

    public void EstablecerNuevoDestino(Vector3 posicion)
    {
        // Establecer el nuevo destino para el NavMeshAgent
        navMeshAgent.SetDestination(posicion);
    }
}
