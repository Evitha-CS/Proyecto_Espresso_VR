using UnityEngine;
using UnityEngine.AI;

public class MovimientoCliente : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EstablecerNuevoDestino();
    }

    public void EstablecerNuevoDestino()
    {
        // Establecer el nuevo destino para el NavMeshAgent
        navMeshAgent.SetDestination((new Vector3(-5.3f, 0f, 2.3f)));
    }
}
