using UnityEngine;
using UnityEngine.AI;

public class MovimientoCliente : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Vector3 areaDestinoCenter = (new Vector3(-5.25f, 0f, 2.5f));
    public Vector2 areaDestinoSize = (new Vector2(0.25f, 1f));

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EstablecerNuevoDestinoEnArea();
    }

    public void EstablecerNuevoDestinoEnArea()
    {
        // Generar un punto aleatorio dentro del área definida
        Vector3 puntoAleatorio = RandomPointEnArea(areaDestinoCenter, areaDestinoSize);

        // Establecer el nuevo destino para el NavMeshAgent
        navMeshAgent.SetDestination(puntoAleatorio);
    }
    public void EstablecerNuevoDestino(Vector3 posicion)
    {
        // Establecer el nuevo destino para el NavMeshAgent
        navMeshAgent.SetDestination(posicion);
    }
    Vector3 RandomPointEnArea(Vector3 center, Vector2 size)
    {
        // Generar un punto aleatorio en un rango rectangular
        Vector3 puntoAleatorio = new Vector3(
            center.x + Random.Range(-size.x / 2f, size.x / 2f),
            center.y,
            center.z + Random.Range(-size.y / 2f, size.y / 2f)
        );

        return puntoAleatorio;
    }
}
