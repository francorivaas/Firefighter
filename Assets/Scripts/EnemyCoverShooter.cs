using UnityEngine;
using System.Collections;

public class EnemyCoverShooter : MonoBehaviour
{
    [Header("Coberturas y movimiento")]
    public Transform[] coverPoints;       // Lugares donde el enemigo puede cubrirse
    public float moveSpeed = 3f;
    private Transform currentCover;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minShootInterval = 2f;   // Tiempo mínimo entre ráfagas
    public float maxShootInterval = 5f;   // Tiempo máximo entre ráfagas
    public int minBulletsPerBurst = 1;    // Balas mínimas por ráfaga
    public int maxBulletsPerBurst = 5;    // Balas máximas por ráfaga
    public float fireRate = 0.2f;         // Tiempo entre cada bala en una ráfaga
    public float bulletSpeed = 10f;       // Velocidad configurable de la bala

    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth;

    private bool isCovered = true;
    private bool isShooting = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Inicia la rutina de moverse entre coberturas y disparar
        StartCoroutine(EnemyBehavior());
    }

    IEnumerator EnemyBehavior()
    {
        while (true)
        {
            // Cambia de cobertura aleatoriamente
            if (coverPoints.Length > 0)
            {
                Transform targetCover = coverPoints[Random.Range(0, coverPoints.Length)];
                yield return MoveToCover(targetCover);
            }

            // Sale de la cobertura
            isCovered = false;

            // Dispara ráfaga aleatoria
            yield return StartCoroutine(ShootBurst());

            // Vuelve a cobertura
            isCovered = true;

            // Espera un tiempo aleatorio antes de volver a actuar
            float wait = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator MoveToCover(Transform target)
    {
        while (Vector2.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        currentCover = target;
    }

    IEnumerator ShootBurst()
    {
        if (isShooting) yield break;
        isShooting = true;

        int bulletsToShoot = Random.Range(minBulletsPerBurst, maxBulletsPerBurst + 1);

        for (int i = 0; i < bulletsToShoot; i++)
        {
            if (isCovered) break; // si se cubre antes, detiene la ráfaga

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

            // Calcula dirección hacia el jugador
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && bulletScript != null)
            {
                Vector2 direction = (player.transform.position - firePoint.position).normalized;
                bulletScript.SetDirection(direction);
                bulletScript.speed = bulletSpeed;
            }

            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
