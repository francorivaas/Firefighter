using UnityEngine;
using System.Collections.Generic;

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

    [Header("Opciones")]
    [Tooltip("Si true, al intentar spawnear y no hay puntos libres, no hace nada. Si false, reinicia el contador y seguirá intentando.")]
    public bool skipIfNoFreePoints = true;

    // Array para llevar la referencia de la llama que está ocupando cada spawnPoint (null si está libre)
    private GameObject[] spawnedFires;

    void Start()
    {
        timer = spawnInterval;

        // Inicializar array de ocupación con el mismo tamaño que spawnPoints
        if (spawnPoints != null && spawnPoints.Length > 0)
            spawnedFires = new GameObject[spawnPoints.Length];
        else
            spawnedFires = new GameObject[0];
    }

    void Update()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || firePrefab == null) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            bool spawned = SpawnFire();
            // Reiniciar contador solo si se spawneó o si skipIfNoFreePoints == false (intentarlo de nuevo tras intervalo)
            if (spawned || !skipIfNoFreePoints)
                timer = spawnInterval;
            else
                // si no se spawnéo y skipIfNoFreePoints == true, esperar hasta que haya un lugar libre
                timer = 0.1f; // pequeño retry rápido — esto mantiene el spawner reactivo cuando se liberan puntos
        }
    }

    /// <summary>
    /// Intenta spawnear una llama en un punto libre.
    /// Devuelve true si spawneó con éxito, false si no había puntos libres.
    /// </summary>
    bool SpawnFire()
    {
        if (spawnPoints.Length == 0 || firePrefab == null)
        {
            Debug.LogWarning("FireSpawner: faltan puntos de spawn o prefab asignado.");
            return false;
        }

        // Construir lista de índices libres
        List<int> freeIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnedFires[i] == null) freeIndices.Add(i);
        }

        if (freeIndices.Count == 0)
        {
            // No hay puntos libres
            // Debug.Log("FireSpawner: no hay spawn points libres actualmente.");
            return false;
        }

        int chosenIndex = -1;

        if (spawnSequentially)
        {
            // Buscar el siguiente índice libre a partir de currentIndex
            for (int offset = 0; offset < spawnPoints.Length; offset++)
            {
                int idx = (currentIndex + offset) % spawnPoints.Length;
                if (spawnedFires[idx] == null)
                {
                    chosenIndex = idx;
                    currentIndex = (idx + 1) % spawnPoints.Length; // la próxima vez continuará desde el siguiente
                    break;
                }
            }
        }
        else
        {
            // Elegir aleatorio entre los libres
            int randomPick = Random.Range(0, freeIndices.Count);
            chosenIndex = freeIndices[randomPick];
        }

        if (chosenIndex == -1)
        {
            // de forma segura, no se encontró índice libre (aunque freeIndices > 0 esto no debería ocurrir)
            return false;
        }

        // Instanciar la llama en la posición del spawnPoint
        Transform spawnPoint = spawnPoints[chosenIndex];
        GameObject instance = Instantiate(firePrefab, spawnPoint.position, Quaternion.identity);

        // Registrar referencia para marcar el punto como ocupado
        spawnedFires[chosenIndex] = instance;

        // Añadir un componente runtime para notificar al spawner cuando la llama sea destruida
        var notifier = instance.AddComponent<FireOccupier>();
        notifier.Initialize(this, chosenIndex);

        return true;
    }

    /// <summary>
    /// Método llamado por FireOccupier cuando la llama es destruida.
    /// Marca el spawnPoint como libre nuevamente.
    /// </summary>
    /// <param name="index">índice del spawnPoint liberado</param>
    public void MarkPointFree(int index)
    {
        if (index < 0 || index >= spawnedFires.Length) return;

        // Asegurarnos de limpiar la referencia solo si coincide o es null
        if (spawnedFires[index] == null)
        {
            // ya estaba limpio
            return;
        }

        // Si la referencia existe pero el objeto fue destruido, Unity devuelve null al compararlo, así que validamos:
        if (spawnedFires[index] == null || spawnedFires[index].Equals(null))
        {
            spawnedFires[index] = null;
            return;
        }

        // Si aún existe (no destruido), limpiamos (esto normalmente no sucede desde OnDestroy)
        spawnedFires[index] = null;
    }

    /// <summary>
    /// Clase auxiliar que se añade a cada instancia de fuego para notificar al spawner cuando esa instancia
    /// es destruida (OnDestroy).
    /// </summary>
    private class FireOccupier : MonoBehaviour
    {
        private FireSpawner spawner;
        private int index = -1;

        public void Initialize(FireSpawner spawner, int index)
        {
            this.spawner = spawner;
            this.index = index;
        }

        void OnDestroy()
        {
            // Cuando esta instancia es destruida, avisamos al spawner
            if (spawner != null)
            {
                spawner.MarkPointFree(index);
            }
        }
    }

    // Método público para reiniciar/limpiar el estado (por si reinicias la escena)
    public void ResetSpawnerState()
    {
        if (spawnedFires != null)
        {
            for (int i = 0; i < spawnedFires.Length; i++)
                spawnedFires[i] = null;
        }
        timer = spawnInterval;
        currentIndex = 0;
    }
}
