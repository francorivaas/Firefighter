using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Datos del progreso")]
    public int rescuedCitizens = 0;   // Cuántos fueron rescatados
    public int totalCitizens = 0;     // Cuántos había en total
    public int minCitizens = 0;       // Cuántos se necesitan rescatar para ganar
    public int deadCitizens = 0;      // Cuántos murieron

    [Header("Tiempo total")]
    public float totalTime = 0f;

    [Header("UI")]
    public Text rescuedCitizensText;

    public GameObject transition;

    private void Awake()
    {
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

    private void Start()
    {
        transition.SetActive(true);
    }

    private void Update()
    {
        // Aumenta el tiempo solo si no estás en la escena de victoria
        if (!SceneManager.GetActiveScene().name.Contains("Victoria"))
        {
            totalTime += Time.deltaTime;
        }

        // Si alcanzó la cantidad mínima, puede salir
        if (rescuedCitizens >= minCitizens)
        {
            print("✅ Ya puedes ir a la salida para completar el nivel");
        }

        // 🔥 NUEVA LÓGICA DE DERROTA 🔥
        // Si el número de ciudadanos restantes vivos no alcanza para llegar al mínimo → derrota
        int remainingCitizens = totalCitizens - (rescuedCitizens + deadCitizens);
        if (rescuedCitizens + remainingCitizens < minCitizens)
        {
            print("❌ Ya no puedes alcanzar el mínimo. Derrota.");
            SceneManager.LoadScene(2); // o el nombre que uses para la escena de derrota
        }

        // Actualizar UI
        if (rescuedCitizensText != null)
        {
            rescuedCitizensText.text = "Ciudadanos: " + rescuedCitizens + "/" + totalCitizens;
        }
    }

    // Llamá a este método cuando un ciudadano sea rescatado
    public void CitizenRescued()
    {
        rescuedCitizens++;
    }

    // Llamá a este método cuando un ciudadano muera
    public void CitizenDead()
    {
        deadCitizens++;
    }

    // Reiniciar datos al volver a jugar
    public void ReiniciarDatos()
    {
        rescuedCitizens = 0;
        deadCitizens = 0;
        totalTime = 0f;
    }
}
