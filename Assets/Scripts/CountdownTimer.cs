using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    [Header("Tiempo")]
    [Tooltip("Tiempo inicial en segundos")]
    public float startTime = 60f;

    [Header("UI")]
    public Text timerText;      // Texto donde mostrar el tiempo
    public Button resetButton;  // Botón para reiniciar

    private float currentTime;
    private bool timerActive = true;

    public GameObject player;

    void Start()
    {
        currentTime = startTime;

        // Ocultar botón al inicio
        if (resetButton != null)
            resetButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!timerActive) return;

        // Decrementar tiempo
        currentTime -= Time.deltaTime;

        // Mostrar en UI
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Tiempo agotado
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            timerActive = false;

            Destroy(player);

            // Mostrar botón de reset
            if (resetButton != null)
            {
                EnableResetButton();
            }
        }
    }

    public void EnableResetButton()
    {
        resetButton.gameObject.SetActive(true);
        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(ResetScene);
    }

    public void ResetScene()
    {
        GameManager.Instance.ReiniciarDatos();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

