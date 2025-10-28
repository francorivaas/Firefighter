using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Salud")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    // Evento opcional para avisar a la barra
    public UnityEvent<float, float> onHealthChanged;
    public CountdownTimer timer;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            timer.EnableResetButton();
        }    
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);
    }
}
