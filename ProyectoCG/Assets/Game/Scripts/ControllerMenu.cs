using UnityEngine;

public class ControllerMenu : MonoBehaviour
{

    public GameObject continueButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Nivel_1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
