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
            hudScoreText.text = "$" + currentScore.ToString();
    }

    public int GetFinalScore()
    {
        return currentScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateHUD();
    }
}
