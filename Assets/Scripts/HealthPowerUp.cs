using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public float heal;
    private AudioSource audioSrc;
    public AudioClip healSound;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            audioSrc.PlayOneShot(healSound);
            playerHealth.Heal(heal);
            Destroy(gameObject, 0.3f);
        }
    }
}
