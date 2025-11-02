using UnityEngine;

public class GunGrab : MonoBehaviour
{
    public GunShooter playerShooter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerShooter.hasGun = true;
            playerShooter.GetComponent<Animator>().SetTrigger("GrabGun");
            Destroy(gameObject);
        }
    }
}
