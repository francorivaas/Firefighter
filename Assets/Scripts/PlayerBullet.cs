using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage;
    public float lifeTime = 3f; // tiempo antes de destruirse automáticamente
    public string enemyTag = "Enemy"; // etiqueta de los enemigos

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }

        // Destruir la bala después de un tiempo
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto golpeado tiene la etiqueta del enemigo
        if (collision.gameObject.CompareTag(enemyTag))
        {
            EnemyCoverShooter enemy = collision.gameObject.GetComponent<EnemyCoverShooter>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Cover"))
        {
            Destroy(gameObject);
        }
    }
}
