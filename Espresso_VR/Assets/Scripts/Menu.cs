using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public Image cargaImage; // Referencia al objeto Image que representa la barra de carga
    public GameObject punteroImage;
    public float tiempoCarga = 2f;
    private float tiempoTranscurrido = 0f;
    private bool jugar = false;
    public Button flecha_2;
    public Button flecha_3;
    public TextMeshProUGUI texto_1;
    public TextMeshProUGUI texto_2;
     public TextMeshProUGUI texto_3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (jugar)
        {
            tiempoTranscurrido += Time.deltaTime;
            punteroImage.SetActive(false);
            // Actualizar la barra de carga
            cargaImage.fillAmount = tiempoTranscurrido / tiempoCarga;
            Debug.Log("Tocando Objeto");

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
        if (other.CompareTag("flecha_1") && texto_2.gameObject.activeSelf)
        {
            texto_1.gameObject.SetActive(true);
            texto_2.gameObject.SetActive(false);
        }
         if (other.CompareTag("flecha_1") && texto_3.gameObject.activeSelf)
        {
            texto_2.gameObject.SetActive(true);
            texto_3.gameObject.SetActive(false);
            flecha_3.gameObject.SetActive(false);
            flecha_2.gameObject.SetActive(true);
        }
        if (other.CompareTag("flecha_2") && texto_1.gameObject.activeSelf)
        {
            texto_1.gameObject.SetActive(false);
            texto_2.gameObject.SetActive(true);
        }
        if (other.CompareTag("flecha_2") && texto_2.gameObject.activeSelf)
        {
            texto_2.gameObject.SetActive(false);
            texto_3.gameObject.SetActive(true);
            flecha_2.gameObject.SetActive(false);
            flecha_3.gameObject.SetActive(true);
        }
        if (other.CompareTag("flecha_3") && texto_2.gameObject.activeSelf)
        {
            texto_2.gameObject.SetActive(false);
            texto_3.gameObject.SetActive(true);
        }
        if (other.CompareTag("interactable"))
        {
            CambiarEscena("SampleScene");
        }

        jugar = false;
    }

    void CambiarEscena(string Escena)
    {
        SceneManager.LoadScene(Escena);
    }
}
