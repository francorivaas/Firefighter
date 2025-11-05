using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Puntaje")]
    public int currentScore = 0;

    [Header("UI")]
    public Text hudScoreText;                // si usás Unity UI Text

    public Animator moneyCounterAnimator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // No DontDestroyOnLoad por defecto; si querés persistir entre escenas, activalo
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateHUD();
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        moneyCounterAnimator.SetTrigger("MoneyGained");
        UpdateHUD();
    }

    public void SubtractPoints(int points)
    {
        currentScore = Mathf.Max(0, currentScore - points);
        UpdateHUD();
    }

    void UpdateHUD()
    {
        
        if (hudScoreText != null)
            hudScoreText.text = "Dinero extra: $" + currentScore.ToString();
    }

    // Método útil para que VictoryScreen lea el puntaje final
    public int GetFinalScore()
    {
        return currentScore;
    }

    // Reseteo (ej.: al reiniciar nivel)
    public void ResetScore()
    {
        currentScore = 0;
        UpdateHUD();
    }
}
