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

    [Header("Control del jugador")]
    public MonoBehaviour playerController;

    // 🔹 Registro de diálogos ya mostrados
    private static HashSet<string> shownDialogues = new HashSet<string>();

    private bool isDialogueActive = false;
    private bool isTyping = false;

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
    /// Llama este método para mostrar un diálogo único.
    /// </summary>
    public void TriggerDialogue(string dialogueID, string text, Sprite portrait)
    {
        if (shownDialogues.Contains(dialogueID))
            return; // 🔹 Ya se mostró, no lo repite

        shownDialogues.Add(dialogueID);

        if (isDialogueActive) return; // Evita superposición

        if (playerController != null)
            playerController.enabled = false;

        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        speakerPortrait.sprite = portrait;

        StartCoroutine(TypeText(text));
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
    }

    // 🔹 Si querés reiniciar todos los diálogos (por ejemplo, al reiniciar nivel)
    public static void ResetDialogues()
    {
        shownDialogues.Clear();
    }
}
