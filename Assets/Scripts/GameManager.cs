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

    [Header("UI")]
    public Text rescuedCitizensText;

    public GameObject transition;
    public GameObject door;
    public Animator camAnim;
    public bool isCutscenePlayed;

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
        isCutscenePlayed = false;
        transition.SetActive(true);
    }

    private void Update()
    {
        if (rescuedCitizens >= 4)
        {
            InGameDialogue.Instance.TriggerDialogue("D_8", "Aún no me conoces, bombero cualquiera... pero ya lo harás... je je...", "Enemy");
        }

        // Si alcanzó la cantidad mínima, puede salir
        if (rescuedCitizens >= minCitizens)
        {
            door.gameObject.SetActive(false);

            if (!isCutscenePlayed)
            {
                camAnim.SetBool("Cutscene1", true);
                Invoke(nameof(StopCutscene), 5f);
                isCutscenePlayed = true;
            }
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

    public void StopCutscene()
    {
        if (isCutscenePlayed)
            camAnim.SetBool("Cutscene1", false);
        InGameDialogue.Instance.TriggerDialogue("D_7", "Así que quieres la tarjeta de crédito, ¿eh? Te estoy esperando... bombero cualquiera...", "Enemy");
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
    }
}
