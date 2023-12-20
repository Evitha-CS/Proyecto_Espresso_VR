using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Image cargaImage;
    public GameObject punteroImage;
    public float tiempoCarga = 2f;
    private float tiempoTranscurrido = 0f;
    private bool jugar = false;

    public GameObject[] textos;
    private int indiceTextoActual = 0;

    void Update()
    {
        if (jugar)
        {
            tiempoTranscurrido += Time.deltaTime;
            punteroImage.SetActive(false);
            cargaImage.fillAmount = tiempoTranscurrido / tiempoCarga;

            if (tiempoTranscurrido >= tiempoCarga)
            {
                CambiarEscena("SampleScene");
                punteroImage.SetActive(true);
                cargaImage.fillAmount = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("flecha_1"))
        {
            if (indiceTextoActual > 0)
            {
                MostrarTextoAnterior();
            }
        }
        else if (other.CompareTag("flecha_2"))
        {
            if (indiceTextoActual < textos.Length - 1)
            {
                MostrarTextoSiguiente();
            }
        }
        else if (other.CompareTag("interactable"))
        {
            jugar = true;
            //CambiarEscena("SampleScene");
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            jugar = false;
            punteroImage.SetActive(true);
            tiempoTranscurrido = 0f;
            cargaImage.fillAmount = 0f;
        }
    }

    public void MostrarTextoSiguiente()
    {
        textos[indiceTextoActual].SetActive(false);
        indiceTextoActual = Mathf.Min(indiceTextoActual + 1, textos.Length - 1);
        textos[indiceTextoActual].SetActive(true);
    }

    public void MostrarTextoAnterior()
    {
        textos[indiceTextoActual].SetActive(false);
        indiceTextoActual = Mathf.Max(indiceTextoActual - 1, 0);
        textos[indiceTextoActual].SetActive(true);
    }

    void CambiarEscena(string Escena)
    {
        SceneManager.LoadScene(Escena);
    }
}
