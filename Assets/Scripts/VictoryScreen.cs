using UnityEngine;
using TMPro;
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
        ciudadanosText.text = "Ciudadanos rescatados: " + ciudadanos;
        moneyText.text = "Dinero obtenido: " + money;
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