using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    [Header("Puntos donde puede aparecer el fuego")]
    public Transform[] spawnPoints; // Lugares asignables desde el inspector

    [Header("Prefab del fuego")]
    public GameObject firePrefab;

    [Header("Configuración de tiempo")]
    public float spawnInterval = 10f; // tiempo entre apariciones
    private float timer;

    [Header("Control de aparición")]
    public bool spawnSequentially = false; // si true, recorre los puntos en orden; si false, aleatorio
    private int currentIndex = 0;

    public AudioSource fireBurstSfx;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnFire();
            timer = spawnInterval; // reinicia el contador
        }
    }

    void SpawnFire()
    {
        if (spawnPoints.Length == 0 || firePrefab == null)
        {
            Debug.LogWarning("FireSpawner: faltan puntos de spawn o prefab asignado.");
            return;
        }

        Transform spawnPoint;

        if (spawnSequentially)
        {
            spawnPoint = spawnPoints[currentIndex];
            currentIndex = (currentIndex + 1) % spawnPoints.Length;
        }
        else
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            spawnPoint = spawnPoints[randomIndex];
        }

        Instantiate(firePrefab, spawnPoint.position, Quaternion.identity);
    }
}
