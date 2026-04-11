using UnityEngine;

public class ControllerMenu : MonoBehaviour
{

    public GameObject continueButton;
    public GameObject settingsPanel;
    public GameObject videoConfigPanel;
    public GameObject audioConfigPanel;
    public GameObject controlsConfigPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenVideoConfig()
    {
        videoConfigPanel.SetActive(true);
        audioConfigPanel.SetActive(false);
        controlsConfigPanel.SetActive(false);
    }

    public void OpenAudioConfig()
    {
        videoConfigPanel.SetActive(false);
        audioConfigPanel.SetActive(true);
        controlsConfigPanel.SetActive(false);
    }

    public void OpenControlsConfig()
    {
        videoConfigPanel.SetActive(false);
        audioConfigPanel.SetActive(false);
        controlsConfigPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
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
