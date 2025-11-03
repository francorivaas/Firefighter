using UnityEngine;
using Unity.Cinemachine;

public class GunShooter : MonoBehaviour
{
    [Header("Configuración del arma")]
    public bool hasGun = false;           // El jugador solo dispara si tiene el arma
    public float fireRate = 0.25f;        // Tiempo entre disparos
    public float bulletSpeed = 10f;       // Velocidad de la bala
    public int damage = 20;               // Daño que inflige cada bala

    [Header("Referencias")]
    public Transform firePoint;           // Lugar desde donde se origina el disparo
    public GameObject bulletPrefab;       // Prefab de la bala del jugador

    [Header("Efectos opcionales")]
    public ParticleSystem muzzleFlash;    // Efecto visual de disparo
    public AudioSource gunSound;          // Sonido del disparo

    private float nextFireTime = 0f;
    private CinemachineImpulseSource source;

    private void Start()
    {
        source = GetComponent<CinemachineImpulseSource>();        
    }

    void Update()
    {
        if (!hasGun) return;

        // Dispara con click izquierdo y respeta la cadencia de fuego
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Efectos
        if (muzzleFlash != null) muzzleFlash.Play();
        if (gunSound != null) gunSound.Play();
        CameraShakeManager.instance.CameraShake(source);

        // Instancia la bala
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
            print("shoot");

            // Configura daño y velocidad
            if (bulletScript != null)
            {
                bulletScript.damage = damage;
                bulletScript.speed = bulletSpeed;
            }
        }
        else
        {
            Debug.LogWarning("Falta asignar el bulletPrefab o firePoint en GunShooter.");
        }
    }

    public void PickupGun()
    {
        hasGun = true;
        Debug.Log("El jugador ha recogido el arma.");
    }

    // Método auxiliar para otros scripts
    public bool IsGunEquipped()
    {
        return hasGun;
    }

    public void RemoveGun()
    {
        hasGun = false;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("GrabManguera", true);
        }
        Debug.Log("El jugador guardó el arma y volvió a usar la manguera.");
    }
}
