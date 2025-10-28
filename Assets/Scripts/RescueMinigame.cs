using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RescueMinigame : MonoBehaviour
{
    [Header("Referencias")]
    public RectTransform barra;
    public RectTransform zonaVerde;
    public RectTransform aguja;
    public Text resultadoTexto;

    [Header("Parámetros")]
    public float velocidad = 200f;
    public float tiempoReinicio = 1.5f;

    [HideInInspector] public Citizen currentCitizen;

    private bool moviendoDerecha = true;
    private bool puedeJugar = true;

    private Vector3 posicionInicialAguja;

    public Text rescuedCitizens;
    public int citizens;

    void Start()
    {
        citizens = 0;
        posicionInicialAguja = aguja.localPosition;
        resultadoTexto.text = "";
    }

    void Update()
    {
        if (!puedeJugar) return;

        MoverAguja();

        if (Input.GetMouseButtonDown(0))
        {
            VerificarResultado();
        }

        rescuedCitizens.text = "Ciudadanos:" + citizens;
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
            resultadoTexto.color = Color.green;
        }
        else
        {
            resultadoTexto.color = Color.red;
        }

        puedeJugar = false;
        StartCoroutine(FinalizarMinijuego());
    }

    IEnumerator FinalizarMinijuego()
    {
        yield return new WaitForSeconds(tiempoReinicio);

        resultadoTexto.text = "";
        aguja.localPosition = posicionInicialAguja;
        moviendoDerecha = Random.value > 0.5f;
        puedeJugar = true;

        // 👇 Se avisa al ciudadano actual que terminó el rescate
        if (currentCitizen != null)
        {
            currentCitizen.EndRescue();
            currentCitizen = null;
        }
    }
}
