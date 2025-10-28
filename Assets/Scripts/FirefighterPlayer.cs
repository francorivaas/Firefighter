// FirefighterPlayer.cs
using UnityEngine;

public class FirefighterPlayer : MonoBehaviour
{
    [Header("Ajustes de disparo")]
    public float waterRange = 5f;             // Distancia máxima del chorro de agua
    public float sprayRate = 0.2f;            // Intervalo entre cada "pulso" de agua
    public float waterPower = 1f;             // Cuánto "daño de agua" se aplica por pulso
    public LayerMask fireLayer;               // Capa de los fuegos

    private float nextSprayTime;

    public GameObject water;

    void Update()
    {
        if (Input.GetMouseButton(0)) // click izquierdo
        {
            if (Time.time >= nextSprayTime)
            {
                ShootWater();
                nextSprayTime = Time.time + sprayRate;
                water.gameObject.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            water.gameObject.SetActive(false);
        }
    }

    void ShootWater()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        // Raycast para detectar el fuego
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, waterRange, fireLayer);

        

        if (hit.collider != null)
        {
            print("hit fire");
            FireTarget fire = hit.collider.GetComponent<FireTarget>();
            if (fire != null)
            {
                fire.ApplyWater(waterPower);
                print("apply water");
            }
        }

        Debug.DrawRay(transform.position, direction * waterRange, Color.cyan, 0.1f);
    }
}

