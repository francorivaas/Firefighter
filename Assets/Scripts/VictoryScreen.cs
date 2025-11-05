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
        ciudadanosText.text = "Ciudadanos rescatados: " + ciudadanos;
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