using UnityEngine;

public class Citizen : MonoBehaviour
{
    public RescueMinigame rescueMinigame;
    public GameObject rescueBar;

    private bool isBeingRescued = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FirefighterPlayer player = collision.gameObject.GetComponent<FirefighterPlayer>();
        if (player != null && !isBeingRescued)
        {
            isBeingRescued = true;
            rescueMinigame.currentCitizen = this; // le decimos al minijuego quién es
            rescueMinigame.gameObject.SetActive(true);
            rescueBar.SetActive(true);
        }
    }

    public void EndRescue()
    {
        // Oculta barra y minijuego
        rescueMinigame.gameObject.SetActive(false);
        rescueBar.SetActive(false);

        // Destruye o desactiva al ciudadano
        Destroy(gameObject);
    }
}
