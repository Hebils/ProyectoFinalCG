using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CaveController : MonoBehaviour
{
    public static CaveController instance;

    [Header("Cristales")]
    public int crystalsNeeded = 5;
    public int currentCrystals = 0;

    [Header("Tiempo")]
    public float maxTime = 60f;
    private float currentTime;

    [Header("UI")]
    public TMP_Text crystalText;
    public TMP_Text timerText;
    public TMP_Text objectiveText;

    [Header("Panels")]
    public GameObject losePanel;

    private bool timerStarted = false;
    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentTime = maxTime;

        UpdateCrystalUI();

        objectiveText.text =
        "Recolecta los 5 cristales";
    }

    void Update()
    {
        if (!timerStarted || gameEnded)
            return;

        currentTime -= Time.deltaTime;

        timerText.text =
        "Tiempo: " + Mathf.Ceil(currentTime);

        if(currentTime <= 0)
        {
            LoseGame();
        }
    }

    public void AddCrystal(int amount)
    {
        if(gameEnded) return;

        if(!timerStarted)
        {
            timerStarted = true;
        }

        currentCrystals += amount;

        Debug.Log("Cristales: " + currentCrystals);

        UpdateCrystalUI();

        PlayerPrefs.SetInt("Crystals", currentCrystals);

        if(currentCrystals >= crystalsNeeded)
        {
            objectiveText.text =
            "Dirigete a la zona de escape";
        }
    }

    void UpdateCrystalUI()
    {
        crystalText.text =
        currentCrystals + " / " + crystalsNeeded;
    }

    void LoseGame()
    {
        gameEnded = true;

        losePanel.SetActive(true);

        Invoke(nameof(RestartScene), 3f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(
        SceneManager.GetActiveScene().buildIndex);
    }
}