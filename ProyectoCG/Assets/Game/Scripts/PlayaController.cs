using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayaController : MonoBehaviour
{
    public string nombreEscena = "Playa";
    public string siguienteEscena = "Selva";
    public TMP_Text tutorialText;
    public TMP_Text contadorRecolectablesText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayBeachMusic();
            GameManager.Instance.contadorRecolectablesText = contadorRecolectablesText;
            GameManager.Instance.ActualizarTextoConteo();
            OcultarObjetosRecogidos();
        }

        if (tutorialText != null)
        {
            tutorialText.text = "Explora la playa, aprende los controles y recoge la caja para continuar.";
        }
    }

    public void RecogerObjeto(GameObject objeto)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegistrarObjetoRecogido(nombreEscena, objeto.name);
            GameManager.Instance.PlayPickUp();
            SceneManager.LoadScene(siguienteEscena);
        }

        Destroy(objeto);
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

    public void TerminarPlaya()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompletarZona(nombreEscena);
        }

        SceneManager.LoadScene(siguienteEscena);
    }
}
