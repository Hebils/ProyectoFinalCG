using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{

    public GameObject continueButton;
    public GameObject settingsPanel;
    public GameObject videoConfigPanel;
    public GameObject audioConfigPanel;
    public GameObject controlsConfigPanel;
    public Animator animator;

    void Awake()
    {
        animator.SetBool("Inicio", true);
    }
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

    IEnumerator OpenSettingsWithDelay(float delay)
    {
        animator.SetBool("SettingsOn", true);
        animator.SetBool("Inicio", false);
        yield return new WaitForSeconds(delay);
        settingsPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        StartCoroutine(OpenSettingsWithDelay(1.5f));

    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        animator.SetBool("SettingsOn", false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Playa");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
