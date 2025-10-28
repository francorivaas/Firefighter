using UnityEngine;
using UnityEngine.UI;

public class Citizen : MonoBehaviour
{
    public RescueMinigame rescueMinigame;
    public GameObject rescueBar;
    public Text interactionText;

    private bool playerNearby = false;
    private bool isBeingRescued = false;

    private void Update()
    {
        if (playerNearby && !isBeingRescued && Input.GetKeyDown(KeyCode.E))
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
            interactionText.text = "Presiona E para rescatar";
            interactionText.gameObject.SetActive(true);
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

    public void EndRescue()
    {
        rescueMinigame.gameObject.SetActive(false);
        rescueBar.SetActive(false);
        Destroy(gameObject);
    }
}
