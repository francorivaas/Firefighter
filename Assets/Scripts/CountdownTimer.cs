using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    [Header("Tiempo")]
    [Tooltip("Tiempo inicial en segundos")]
    public float startTime = 60f;

    [Header("UI")]
    public Text timerText;
    public Button resetButton;

    private float currentTime;
    private bool timerActive = true;
    private int lastSecondDisplayed;

    public GameObject player;

    // 🔹 Variable estática para guardar el tiempo final
    public static float finalTimeTaken = 0f;

    void Start()
    {
        currentTime = startTime;

        if (resetButton != null)
            resetButton.gameObject.SetActive(false);

        UpdateTimerText();
        lastSecondDisplayed = Mathf.CeilToInt(currentTime);
    }

    void Update()
    {
        if (!timerActive) return;

        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        UpdateTimerText();

        int currentSecond = Mathf.CeilToInt(currentTime);
        if (currentSecond != lastSecondDisplayed)
        {
            lastSecondDisplayed = currentSecond;
        }

        if (currentTime <= 0f)
        {
            timerActive = false;

            // 🔹 Guardamos cuánto tiempo tardó (tiempo inicial - tiempo restante)
            finalTimeTaken = startTime - currentTime;

            SceneManager.LoadScene(2);
            Destroy(player);
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
