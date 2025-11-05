using UnityEngine;

public class Knife : MonoBehaviour
{
    public RescueMinigame minigame;
    public AudioSource audioSrc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            minigame.hasKnife = true;
            audioSrc.Play();        
            Destroy(gameObject, .3f);
        }
    }
}
