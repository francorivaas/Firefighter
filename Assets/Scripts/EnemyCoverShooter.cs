using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyCoverShooter : MonoBehaviour
{
    [Header("Coberturas y movimiento")]
    public Transform[] coverPoints;
    public float moveSpeed = 3f;
    private Transform currentCover;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minShootInterval = 2f;
    public float maxShootInterval = 5f;
    public int minBulletsPerBurst = 1;
    public int maxBulletsPerBurst = 5;
    public float fireRate = 0.2f;
    public float bulletSpeed = 10f;

    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth;

    [System.Serializable]
    public class HealthChangedEvent : UnityEvent<float, float> { }
    [HideInInspector] public HealthChangedEvent onHealthChanged = new HealthChangedEvent();

    private bool isCovered = true;
    private bool isShooting = false;
    private bool isActive = false; // ⬅️ Nuevo: el enemigo no actúa hasta ser activado

    public GameObject lifebar;
    public AudioSource audioSrc;
    public AudioClip shootingSFX;
    public AudioClip getDamageSFX;

    private SpriteRenderer spriteRenderer;
    public Sprite sprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void ActivateEnemy()
    {
        if (isActive) return;
        isActive = true;
        StartCoroutine(EnemyBehavior());
    }

    IEnumerator EnemyBehavior()
    {
        while (isActive)
        {
            if (coverPoints.Length > 0)
            {
                Transform targetCover = coverPoints[Random.Range(0, coverPoints.Length)];
                yield return MoveToCover(targetCover);
            }

            isCovered = false;
            yield return StartCoroutine(ShootBurst());
            isCovered = true;

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
        audioSrc.PlayOneShot(shootingSFX);
        int bulletsToShoot = Random.Range(minBulletsPerBurst, maxBulletsPerBurst + 1);

        for (int i = 0; i < bulletsToShoot; i++)
        {
            if (isCovered) break;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);
        audioSrc.PlayOneShot(getDamageSFX);
        audioSrc.volume = 1f;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [System.Obsolete]
    void Die()
    {
        spriteRenderer.sprite = sprite;
        InGameDialogue.Instance.TriggerDialogue("D_12", "Es increible que me hayas derrotado... siendo.. un... bombero cualquiera...", "DeadEnemy");
        Destroy(lifebar);
        StopAllCoroutines();

        // Buscar el script del arma del jugador y "guardarla"
        GunShooter playerGun = FindObjectOfType<GunShooter>();
        if (playerGun != null)
        {
            playerGun.RemoveGun();
        }

        isActive = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }
}
