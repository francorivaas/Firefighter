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

    private AudioSource audioSrc;
    public AudioClip burstSfx;
    public AudioClip extinguishSfx;

    public GameObject light;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentTimeToDamage = 0;
        canCount = false;
        canDamage = true;
        audioSrc.PlayOneShot(burstSfx);
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
        audioSrc.PlayOneShot(extinguishSfx);
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("Extinguish", true);
        GetComponent<Collider2D>().enabled = false;
        light.SetActive(false);
        Invoke(nameof(Reignite), reigniteDelay);
    }

    void Reignite()
    {
        isExtinguished = false;
        audioSrc.PlayOneShot(burstSfx);
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("Extinguish", false);
        light.SetActive(true);
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

