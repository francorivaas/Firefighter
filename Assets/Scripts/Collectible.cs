using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Configuración")]
    public int points = 100;
    public bool destroyOnCollect = true; // si querés desactivar en vez de destruir, podés cambiarlo

    [Header("Opcional: sonido / efecto")]
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Sumar puntos
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddPoints(points);

        audioSrc.Play();

        // Destruir u ocultar
        if (destroyOnCollect)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
