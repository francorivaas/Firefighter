using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public Text ciudadanosText;
    public Text tiempoText;
    public Text moneyText;

    private void Start()
    {
        int ciudadanos = GameManager.Instance.rescuedCitizens;
        int money = ScoreManager.Instance.currentScore;

        // 🔹 Mostrar datos de puntuación
        ciudadanosText.text = "Ciudadanos rescatados: " + ciudadanos;
        moneyText.text = "Dinero obtenido: " + money;

        // 🔹 Mostrar tiempo tardado
        float finalTime = CountdownTimer.finalTimeTaken;
        int minutes = Mathf.FloorToInt(finalTime / 60f);
        int seconds = Mathf.FloorToInt(finalTime % 60f);
        tiempoText.text = $"Tiempo total: {minutes:00}:{seconds:00}";
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
