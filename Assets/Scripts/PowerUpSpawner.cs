using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab del PowerUp de vida")]
    public GameObject powerUpPrefab;

    [Header("Posiciones posibles de aparición")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Configuración de spawn")]
    public float spawnIntervalMin = 5f;  // Tiempo mínimo entre spawns
    public float spawnIntervalMax = 10f; // Tiempo máximo entre spawns
    public int maxPowerUpsInScene = 3;   // Cuántos pueden existir al mismo tiempo

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnPowerUp();
            ScheduleNextSpawn();
        }
    }

    void TrySpawnPowerUp()
    {
        if (powerUpPrefab == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("⚠️ Falta asignar prefab o posiciones de spawn en el PowerUpSpawner.");
            return;
        }

        // Verifica cuántos powerups hay en escena
        int currentPowerUps = GameObject.FindGameObjectsWithTag("PowerUp").Length;
        if (currentPowerUps >= maxPowerUpsInScene)
            return;

        // Selecciona una posición aleatoria de la lista
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Instancia el powerup
        Instantiate(powerUpPrefab, spawnPoint.position, Quaternion.identity);
    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
    }
}
