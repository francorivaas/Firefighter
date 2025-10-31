using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RescueMinigame : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform barra;
    public RectTransform zonaVerde;
    public RectTransform aguja;

    [Header("Parámetros de juego")]
    public float velocidad = 200f;
    public float aumentoVelocidad = 100f;
    public float tiempoReinicio = 1.5f;

    [Header("Dificultad dinámica")]
    public float greenZoneShrinkAmount = 20f; // cuánto se reduce por rescate exitoso
    public float minGreenZoneWidth = 40f;     // tamaño mínimo de la zona verde
    private float initialGreenZoneWidth;

    [HideInInspector] public Citizen currentCitizen;

    private bool moviendoDerecha = true;
    private bool puedeJugar = true;
    private Vector3 posicionInicialAguja;

    public AudioSource rescuedCitizenSFX;
    public AudioSource deadCitizenSFX;
    
    public int citizens;

    void Start()
    {
        citizens = 0;
        posicionInicialAguja = aguja.localPosition;

        initialGreenZoneWidth = zonaVerde.sizeDelta.x; // guardamos tamaño inicial
    }

    void Update()
    {
        if (!puedeJugar) return;

        MoverAguja();

        if (Input.GetMouseButtonDown(0))
        {
            VerificarResultado();
        }

        
    }

    void MoverAguja()
    {
        float limiteIzquierdo = barra.rect.xMin + barra.localPosition.x;
        float limiteDerecho = barra.rect.xMax + barra.localPosition.x;

        Vector3 pos = aguja.localPosition;
        float movimiento = velocidad * Time.deltaTime * (moviendoDerecha ? 1 : -1);
        pos.x += movimiento;
        aguja.localPosition = pos;

        if (pos.x >= limiteDerecho) moviendoDerecha = false;
        else if (pos.x <= limiteIzquierdo) moviendoDerecha = true;
    }

    void VerificarResultado()
    {
        float xAguja = aguja.position.x;
        float xVerdeMin = zonaVerde.position.x - zonaVerde.rect.width / 2;
        float xVerdeMax = zonaVerde.position.x + zonaVerde.rect.width / 2;

        if (xAguja >= xVerdeMin && xAguja <= xVerdeMax)
        {
            citizens++;

            // Reducir tamaño de zona verde (más difícil)
            float nuevoAncho = Mathf.Max(zonaVerde.sizeDelta.x - greenZoneShrinkAmount, minGreenZoneWidth);
            zonaVerde.sizeDelta = new Vector2(nuevoAncho, zonaVerde.sizeDelta.y);
            velocidad += aumentoVelocidad;
            rescuedCitizenSFX.Play();
            GameManager.Instance.rescuedCitizens++;
        }
        else
        {
            deadCitizenSFX.Play();
            GameManager.Instance.deadCitizens++;
        }

        puedeJugar = false;
        StartCoroutine(FinalizarMinijuego());
    }

    IEnumerator FinalizarMinijuego()
    {
        yield return new WaitForSeconds(tiempoReinicio);

        aguja.localPosition = posicionInicialAguja;
        moviendoDerecha = Random.value > 0.5f;
        puedeJugar = true;

        if (currentCitizen != null)
        {
            currentCitizen.EndRescue();
            currentCitizen = null;
        }
    }
}
