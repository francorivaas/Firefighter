using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.Cinemachine;

public class Health : MonoBehaviour
{
    [Header("Salud")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Audio de daño")]
    public AudioSource audioSource;          // Fuente de audio para reproducir los sonidos
    public AudioClip[] damageSounds;         // Lista de gritos o sonidos de daño
    [Range(0f, 1f)] public float volume = 0.8f;

    // Evento opcional para avisar a la barra
    public UnityEvent<float, float> onHealthChanged;
    public CountdownTimer timer;

    private CinemachineImpulseSource source;

    void Start()
    {
        currentHealth = maxHealth;
        source = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.ReiniciarDatos();
            SceneManager.LoadScene(2); // Escena de derrota
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);
        CameraShakeManager.instance.CameraShake(source);

        PlayRandomDamageSound();
    }

    void PlayRandomDamageSound()
    {
        if (damageSounds.Length == 0 || audioSource == null) return;

        // Elegir un sonido al azar
        int randomIndex = Random.Range(0, damageSounds.Length);
        AudioClip clip = damageSounds[randomIndex];

        // Reproducir el sonido
        audioSource.PlayOneShot(clip, volume);
    }
}
