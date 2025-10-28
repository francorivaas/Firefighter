using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Referencia al script de salud del personaje")]
    public Health targetHealth;

    [Header("Imagen de relleno de la barra")]
    public Image fillImage;

    void Start()
    {
        if (targetHealth != null)
        {
            // Suscribirse al evento
            targetHealth.onHealthChanged.AddListener(UpdateHealthBar);
        }
    }

    void UpdateHealthBar(float current, float max)
    {
        if (fillImage != null)
        {
            float targetFill = current / max;
            StopAllCoroutines();
            StartCoroutine(SmoothFill(targetFill));
        }
    }

    IEnumerator SmoothFill(float target)
    {
        float duration = 0.3f;
        float start = fillImage.fillAmount;
        float time = 0f;

        while (time < duration)
        {
            fillImage.fillAmount = Mathf.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fillImage.fillAmount = target;
    }

}

