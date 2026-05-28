using UnityEngine;
using TMPro;

public class SelvaController : MonoBehaviour
{
    public string nombreEscena = "Selva";
    public GameObject victoriaPanel;
    public GameObject resumenPanel;
    public TMP_Text contadorRecolectablesText;
    public TMP_Text resumenText;

    private bool juegoTerminado = false;

    void Start()
    {
        if (victoriaPanel != null)
        {
            victoriaPanel.SetActive(false);
        }

        if (resumenPanel != null)
        {
            resumenPanel.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlaySelvaMusic();
            GameManager.Instance.contadorRecolectablesText = contadorRecolectablesText;
            GameManager.Instance.ActualizarTextoConteo();
            OcultarObjetosRecogidos();
        }
    }

    public void RecogerObjeto(GameObject objeto)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegistrarObjetoRecogido(nombreEscena, objeto.name);
            GameManager.Instance.PlayPickUp();
        }

        objeto.SetActive(false);
    }

    void OcultarObjetosRecogidos()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Recolectable");

        foreach (GameObject objeto in objetos)
        {
            if (GameManager.Instance.ObjetoYaRecogido(nombreEscena, objeto.name))
            {
                objeto.SetActive(false);
            }
        }
    }

    public void TerminarSelva()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompletarZona(nombreEscena);
            GameManager.Instance.Cargar();
        }

        if (victoriaPanel != null)
        {
            victoriaPanel.SetActive(true);
        }

        if (resumenPanel != null)
        {
            resumenPanel.SetActive(true);
        }

        if (resumenText != null && GameManager.Instance != null)
        {
            resumenText.text = GameManager.Instance.ObtenerResumenDatos();
        }
    }
}
