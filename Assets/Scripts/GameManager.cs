using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int rescuedCitizens = 0;
    public int totalCitizens = 0;
    public int minCitizens = 0;
    public float tiempoTotal = 0f;

    public Text rescuedCitizensText;

    private void Awake()
    {
        // Hacer que el GameManager no se destruya al cambiar de escena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Aumentar el tiempo solo si estamos en el nivel (no en la pantalla de victoria)
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Victoria"))
        {
            tiempoTotal += Time.deltaTime;
        }

        if (rescuedCitizens >= minCitizens)
        {
            print("ya puedes ir a la salida");
        }

        else if (totalCitizens < minCitizens)
        {
            print("derrota");
        }

        rescuedCitizensText.text = "Ciudadanos: " + GameManager.Instance.rescuedCitizens + "/" + GameManager.Instance.minCitizens;
    }

    public void ReiniciarDatos()
    {
        rescuedCitizens = 0;
        tiempoTotal = 0f;
    }
}
