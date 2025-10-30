using UnityEngine;

public class FireTarget : MonoBehaviour
{
    [Header("Ajustes del fuego")]
    public float extinguishTime = 3f;        // Tiempo necesario bajo el chorro para apagarse
    public float reigniteDelay = 5f;         // Tiempo para volver a encenderse

    private float currentExtinguishProgress = 0f;
    private bool isExtinguished = false;

    private SpriteRenderer sr;

    public float timeToDamage;
    public float currentTimeToDamage;
    public float damage;
    
    public bool canCount;
    public bool canDamage;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentTimeToDamage = 0;
        canCount = false;
        canDamage = true;
    }

    public void ApplyWater(float amount)
    {
        if (isExtinguished) return;

        currentExtinguishProgress += amount * Time.deltaTime;

        if (currentExtinguishProgress >= extinguishTime)
        {
            Extinguish();
        }
    }

    void Extinguish()
    {
        isExtinguished = true;
        currentExtinguishProgress = 0f;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("Extinguish", true);
        GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(Reignite), reigniteDelay);
    }

    void Reignite()
    {
        isExtinguished = false;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("Extinguish", false);
        GetComponent<Collider2D>().enabled = true;
    }

    private void Update()
    {
        if (canCount)
        {
            currentTimeToDamage += Time.deltaTime;
            if (currentTimeToDamage >= timeToDamage)
                canDamage = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Health player = collision.gameObject.GetComponent<Health>();
        if (player != null)
        {
            if (canDamage)
            {
                player.TakeDamage(damage);
                canDamage = false;
                canCount = true;
                currentTimeToDamage = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health player = collision.gameObject.GetComponent<Health>();
        if (player != null)
        {
            if (canDamage)
            {
                player.TakeDamage(damage);
                canDamage = false;
                canCount = true;
                currentTimeToDamage = 0;
            }
        }
    }
}

