using UnityEngine;
using UnityEngine.UI;

public class Citizen : MonoBehaviour
{
    [Header("Referencias")]
    public RescueMinigame rescueMinigame;
    public GameObject rescueBar;
    public Text interactionText;

    [Header("Puntaje por rescate")]
    public int pointsIfCountsForMin = 100;  // Puntos si este rescate cuenta dentro del mínimo
    public int pointsIfOptional = 200;      // Puntos si este rescate es adicional (más de los necesarios)

    private bool playerNearby = false;
    private bool isBeingRescued = false;

    private void Update()
    {
        if (playerNearby && !isBeingRescued && rescueMinigame.hasKnife && Input.GetKeyDown(KeyCode.E))
        {
            StartRescue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FirefighterPlayer player = collision.gameObject.GetComponent<FirefighterPlayer>();
        if (player != null)
        {
            playerNearby = true;

            if (rescueMinigame.hasKnife)
            {
                interactionText.text = "Presiona E para rescatar";
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FirefighterPlayer player = collision.gameObject.GetComponent<FirefighterPlayer>();
        if (player != null)
        {
            playerNearby = false;
            interactionText.gameObject.SetActive(false);
        }
    }

    private void StartRescue()
    {
        isBeingRescued = true;
        interactionText.gameObject.SetActive(false);

        rescueMinigame.currentCitizen = this;
        rescueMinigame.gameObject.SetActive(true);
        rescueBar.SetActive(true);
    }

    // Este método se llama cuando el rescate termina correctamente
    public void EndRescue()
    {
        rescueMinigame.gameObject.SetActive(false);
        rescueBar.SetActive(false);

        // Determinar si el rescate cuenta dentro del mínimo o es extra
        bool countsForMin = false;
        if (GameManager.Instance != null)
        {
            int rescuedSoFar = GameManager.Instance.rescuedCitizens;
            countsForMin = rescuedSoFar < GameManager.Instance.minCitizens;
        }

        // Otorgar puntos en base a la categoría del rescate
        int points = countsForMin ? pointsIfCountsForMin : pointsIfOptional;

        //if (ScoreManager.Instance != null)
        //{
        //    ScoreManager.Instance.AddPoints(points);
        //}

        // Notificar al GameManager que se rescató un ciudadano
        //if (GameManager.Instance != null)
        //{
        //    GameManager.Instance.CitizenRescued();
        //}

        // Eliminar al ciudadano
        Destroy(gameObject);
    }
}