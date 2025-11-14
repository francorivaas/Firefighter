using UnityEngine;
using Unity.Cinemachine;

public class FirefighterPlayer : MonoBehaviour
{
    [Header("Ajustes de disparo de agua")]
    public float waterRange = 5f;
    public float sprayRate = 0.2f;
    public float waterPower = 1f;
    public LayerMask fireLayer;

    private float nextSprayTime;
    public GameObject water;

    [Header("Referencias")]
    public GunShooter gunShooter;

    [Header("Audio")]
    public AudioSource hoseAudio;   
    public AudioClip hoseClip;      

    private void Start()
    {
        if (hoseAudio != null)
        {
            hoseAudio.playOnAwake = false;
            hoseAudio.loop = false;   // Lo manejamos manualmente
        }
    }

    void Update()
    {
        if (gunShooter != null && gunShooter.IsGunEquipped())
        {
            water.gameObject.SetActive(false);
            StopHoseSound();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nextSprayTime)
            {
                ShootWater();
                nextSprayTime = Time.time + sprayRate;
                water.gameObject.SetActive(true);

                PlayHoseSound();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            water.gameObject.SetActive(false);
            StopHoseSound();
        }
    }

    void ShootWater()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, waterRange, fireLayer);

        if (hit.collider != null)
        {
            FireTarget fire = hit.collider.GetComponent<FireTarget>();
            if (fire != null)
            {
                fire.ApplyWater(waterPower);
            }
        }

        Debug.DrawRay(transform.position, direction * waterRange, Color.cyan, 0.1f);
    }

    // -----------------------------
    // 🔊 AUDIO DE LA MANGUERA
    // -----------------------------
    void PlayHoseSound()
    {
        if (hoseAudio == null || hoseClip == null) return;

        // Si no está sonando, reproducimos desde el inicio
        if (!hoseAudio.isPlaying)
        {
            hoseAudio.clip = hoseClip;
            hoseAudio.time = 0f; // reinicia el sonido
            hoseAudio.Play();
        }
    }

    void StopHoseSound()
    {
        if (hoseAudio == null) return;

        // Detenemos y reiniciamos
        hoseAudio.Stop();
        hoseAudio.time = 0f;
    }
}
