using UnityEngine;
using Unity.Cinemachine;

public class GunGrab : MonoBehaviour
{
    public GunShooter playerShooter;
    public AudioSource audioSrc;
    public AudioClip pickUpSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSrc.PlayOneShot(pickUpSFX);
            playerShooter.hasGun = true;
            playerShooter.GetComponent<Animator>().SetTrigger("GrabGun");
            Destroy(gameObject, .3f);
        }
    }
}
