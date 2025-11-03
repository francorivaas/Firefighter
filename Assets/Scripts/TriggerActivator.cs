using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public EnemyCoverShooter enemyToActivate;
    public GameObject enemyLifeBar;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyToActivate != null)
        {
            enemyToActivate.ActivateEnemy();
            enemyLifeBar.gameObject.SetActive(true);
            Destroy(gameObject); // opcional: destruye el trigger tras activarlo una vez
        }
    }
}