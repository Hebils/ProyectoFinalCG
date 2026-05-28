using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CaveGameManager : MonoBehaviour
{
    public static CaveGameManager instance;

    public int crystalsNeeded = 5;
    public int currentCrystals = 0;

    public float timeLeft = 60f;

    public TMP_Text crystalText;
    public TMP_Text timerText;

    public GameObject losePanel;

    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(gameEnded) return;

        timeLeft -= Time.deltaTime;

        timerText.text = "Tiempo: " + Mathf.Ceil(timeLeft);

        if(timeLeft <= 0)
        {
            LoseGame();
        }
    }

    public void AddCrystal(int amount)
    {
        currentCrystals += amount;

        crystalText.text =
        currentCrystals + " / " + crystalsNeeded;

        if(currentCrystals >= crystalsNeeded)
        {
            Debug.Log("Ya puedes escapar");
        }
    }

    void LoseGame()
    {
        gameEnded = true;

        losePanel.SetActive(true);

        Invoke("RestartScene", 3f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}