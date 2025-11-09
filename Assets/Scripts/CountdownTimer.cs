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

    public GameObject player;

    private AudioSource audioSrc;
    public AudioClip tickSfx;

    // último segundo entero mostrado (ej: 59, 58, 57...)
    private int lastSecondDisplayed;

    void Start()
    {
        currentTime = startTime;
        audioSrc = GetComponent<AudioSource>();

        if (resetButton != null)
            resetButton.gameObject.SetActive(false);

        // Mostrar tiempo inicial
        UpdateTimerText();

        // Inicializar referencia del primer segundo entero
        lastSecondDisplayed = Mathf.CeilToInt(currentTime);

        // 🔊 Reproducir primer tic inmediatamente
        PlayTick();
    }

    void Update()
    {
        if (!timerActive) return;

        currentTime -= Time.deltaTime;
        if (currentTime < 0f) currentTime = 0f;

        UpdateTimerText();

        // Detectar cambio de segundo entero
        int currentSecond = Mathf.CeilToInt(currentTime);
        if (currentSecond != lastSecondDisplayed)
        {
            lastSecondDisplayed = currentSecond;
            PlayTick();
        }

        // Cuando termina el tiempo
        if (currentTime <= 0f)
        {
            timerActive = false;
            SceneManager.LoadScene(2);
            Destroy(player);
        }
    }

    private void PlayTick()
    {
        if (tickSfx != null && audioSrc != null)
            audioSrc.PlayOneShot(tickSfx);
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
