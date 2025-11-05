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
        Cursor.visible = false;
        isCutscenePlayed = false;
        transition.SetActive(true);
    }

    private void Update()
    {
        if (rescuedCitizens >= 4)
        {
            InGameDialogue.Instance.TriggerDialogue("D_8", "Aún no me conoces, bombero cualquiera... pero ya lo harás... je je...", "Enemy");
        }

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

        int remainingCitizens = totalCitizens - (rescuedCitizens + deadCitizens);
        if (rescuedCitizens + remainingCitizens < minCitizens)
        {
            print("❌ Ya no puedes alcanzar el mínimo. Derrota.");
            SceneManager.LoadScene(2); // o el nombre que uses para la escena de derrota
        }

        if (rescuedCitizens == totalCitizens)
        {
            InGameDialogue.Instance.TriggerDialogue("D_11", "¡Los has rescatado a todos!", "Jefe");
        }

        if (rescuedCitizensText != null)
        {
            rescuedCitizensText.text = "Ciudadanos: " + rescuedCitizens + "/" + totalCitizens;
        }
    }

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

    public void CitizenDead()
    {
        deadCitizens++;
    }

    public void ReiniciarDatos()
    {
        rescuedCitizens = 0;
        deadCitizens = 0;
    }
}
