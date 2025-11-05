using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public float heal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            print("heal");
            playerHealth.Heal(heal);
            Destroy(gameObject);
        }
    }
}
