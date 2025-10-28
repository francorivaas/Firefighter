using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int ciudadanosRescatados = 0;
    public float tiempoTotal = 0f;

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
    }

    public void ReiniciarDatos()
    {
        ciudadanosRescatados = 0;
        tiempoTotal = 0f;
    }
}
