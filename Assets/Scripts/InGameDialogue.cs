using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameDialogue : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject dialoguePanel;
    public Image speakerPortrait;
    public Text dialogueText;
    public float typingSpeed = 0.03f;

    [Header("Retratos disponibles")]
    [Tooltip("Lista de retratos posibles. Usa el mismo nombre que el 'speakerName' que pases.")]
    public List<SpeakerPortrait> portraits = new List<SpeakerPortrait>();

    [Header("Control del jugador")]
    public MonoBehaviour playerController;

    // 🔹 Registro de diálogos ya mostrados
    private static HashSet<string> shownDialogues = new HashSet<string>();

    private bool isDialogueActive = false;
    private bool isTyping = false;

    public static InGameDialogue Instance;

    public bool isEnemy;
    public GameObject lifeBar;
    public GameObject citizenCounter;
    public GameObject moneyCounter;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive && !isTyping)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                CloseDialogue();
            }
        }
    }

    /// <summary>
    /// Muestra un diálogo con un retrato específico según el nombre del interlocutor.
    /// </summary>
    public void TriggerDialogue(string dialogueID, string text, string speakerName)
    {
        if (shownDialogues.Contains(dialogueID))
            return; // Ya mostrado

        shownDialogues.Add(dialogueID);

        if (isDialogueActive) return; // Evita superposición

        if (playerController != null)
            playerController.enabled = false;

        // 🔹 Buscar el retrato según el interlocutor
        Sprite foundPortrait = GetPortraitByName(speakerName);
        if (foundPortrait != null)
            speakerPortrait.sprite = foundPortrait;

        dialoguePanel.SetActive(true);
        isDialogueActive = true;

        StartCoroutine(TypeText(text));


        lifeBar.SetActive(false);
        citizenCounter.SetActive(false);
        moneyCounter.SetActive(false);
        
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        if (playerController != null)
            playerController.enabled = true;

        lifeBar.SetActive(true);
        citizenCounter.SetActive(true);
        moneyCounter.SetActive(true);
        isEnemy = false;
    }

    public static void ResetDialogues()
    {
        shownDialogues.Clear();
    }

    // 🔹 Busca el sprite del interlocutor según el nombre
    private Sprite GetPortraitByName(string speakerName)
    {
        foreach (var p in portraits)
        {
            if (p.speakerName == speakerName)
                return p.portraitSprite;
        }
        Debug.LogWarning($"Retrato no encontrado para '{speakerName}'.");
        return null;
    }
}

[System.Serializable]
public class SpeakerPortrait
{
    public string speakerName;
    public Sprite portraitSprite;
}
