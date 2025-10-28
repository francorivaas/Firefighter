using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public Text ciudadanosText;
    public Text tiempoText;

    private void Start()
    {
        int ciudadanos = GameManager.Instance.ciudadanosRescatados;
        float tiempo = GameManager.Instance.tiempoTotal;

        ciudadanosText.text = "Ciudadanos rescatados: " + ciudadanos;

        // Formateamos el tiempo a minutos:segundos
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        tiempoText.text = $"Tiempo total: {minutos:00}:{segundos:00}";
    }
}