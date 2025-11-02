using UnityEngine;

public class Knife : MonoBehaviour
{
    public RescueMinigame minigame;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            minigame.hasKnife = true;
            Destroy(gameObject);
        }
    }
}
