using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public int puntuacion = 0;
    public TextMeshProUGUI textoPuntos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizarPuntuación(int puntos)
    {
        puntuacion += puntos;
        textoPuntos.text = "Reputación: " + puntuacion;
    }
}
