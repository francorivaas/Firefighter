using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Configuración")]
    public int points = 100;
    public bool destroyOnCollect = true; // si querés desactivar en vez de destruir, podés cambiarlo

    [Header("Opcional: sonido / efecto")]
    private AudioSource audioSrc;
    private SpriteRenderer sprites;
    private Collider2D col;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider2D>();
        sprites = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSrc.Play();
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(points);

            }
            sprites.enabled = false;
            col.enabled = false;
        }
    }
}
