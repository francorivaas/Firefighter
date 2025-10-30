using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public Text ciudadanosText;
    public Text tiempoText;

    private void Start()
    {
        int ciudadanos = GameManager.Instance.rescuedCitizens;
        float tiempo = GameManager.Instance.totalTime;

        ciudadanosText.text = "Ciudadanos rescatados: " + ciudadanos;

        // Formateamos el tiempo a minutos:segundos
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        tiempoText.text = $"Tiempo total: {minutos:00}:{segundos:00}";
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}