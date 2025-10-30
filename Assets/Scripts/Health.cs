using UnityEngine;
using UnityEngine.SceneManagement;
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
            
            GameManager.Instance.ReiniciarDatos();
            SceneManager.LoadScene(2);
        }    
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged.Invoke(currentHealth, maxHealth);
        print("receiving damage");
    }

    //public void Heal(float amount)
    //{
    //    currentHealth += amount;
    //    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //    onHealthChanged.Invoke(currentHealth, maxHealth);
    //}
}
