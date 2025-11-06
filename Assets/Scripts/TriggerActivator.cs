using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public EnemyCoverShooter enemyToActivate;
    public GameObject enemyLifeBar;
    public AudioSource bcMusic;
    public AudioSource bossMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyToActivate != null)
        {
            enemyToActivate.ActivateEnemy();
            bcMusic.Stop();
            bossMusic.Play();
            enemyLifeBar.gameObject.SetActive(true);
            Destroy(gameObject); // opcional: destruye el trigger tras activarlo una vez
        }
    }
}