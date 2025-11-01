using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string text;
    public Sprite portrait;
}

[System.Serializable]
public class SpeakerColor
{
    public string speakerName;
    public Color nameColor = Color.white;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image leftPortrait;
    public Image rightPortrait;
    public Text speakerNameText;
    public Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Botón de inicio de misión")]
    public GameObject startMissionPanel;
    public Button startMissionButton;

    [Header("Configuración")]
    public float typingSpeed = 0.03f;
    public string gameSceneName = "EscenaJuego";

    [Header("Cámara (opcional)")]
    public Camera mainCamera;
    public Transform leftFocus;
    public Transform rightFocus;
    public float cameraMoveSpeed = 2f;

    [Header("Colores por personaje")]
    public List<SpeakerColor> speakerColors = new List<SpeakerColor>();

    private Dictionary<string, Color> colorLookup = new Dictionary<string, Color>();
    private Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();
    private DialogueLine currentLine;
    private bool isTyping = false;
    private bool dialogueEnded = false;

    public Animator fireFighterAnimator;
    public Animator bossAnimator;

    void Start()
    {
        dialoguePanel.SetActive(true);
        startMissionPanel.SetActive(false);

        // Convertir lista a diccionario para acceso rápido
        foreach (var entry in speakerColors)
        {
            if (!colorLookup.ContainsKey(entry.speakerName))
                colorLookup.Add(entry.speakerName, entry.nameColor);
        }

        // Busca automáticamente un DialogueTrigger en escena y lo inicia
        DialogueTrigger trigger = FindObjectOfType<DialogueTrigger>();
        if (trigger != null)
        {
            IniciarDialogo(trigger.lineas);
        }

        // Asigna función al botón
        startMissionButton.onClick.AddListener(IniciarMision);
    }

    void Update()
    {
        if (dialogueEnded)
        {
            fireFighterAnimator.SetBool("Out", true);
            bossAnimator.SetBool("Out", true);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentLine.text;
                isTyping = false;
            }
            else
            {
                MostrarSiguienteLinea();
            }
        }
    }

    public void IniciarDialogo(DialogueLine[] lineas)
    {
        dialogueQueue.Clear();
        foreach (var l in lineas)
            dialogueQueue.Enqueue(l);

        MostrarSiguienteLinea();
    }

    void MostrarSiguienteLinea()
    {
        if (dialogueQueue.Count == 0)
        {
            TerminarDialogo();
            return;
        }

        currentLine = dialogueQueue.Dequeue();
        StartCoroutine(EscribirTexto(currentLine));
        CambiarPersonajeActivo(currentLine);
    }

    IEnumerator EscribirTexto(DialogueLine linea)
    {
        isTyping = true;

        // Cambiar color del nombre según el personaje
        if (colorLookup.TryGetValue(linea.speakerName, out Color nameColor))
            speakerNameText.color = nameColor;
        else
            speakerNameText.color = Color.white;

        speakerNameText.text = linea.speakerName;
        dialogueText.text = "";

        foreach (char c in linea.text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void CambiarPersonajeActivo(DialogueLine linea)
    {
        bool isLeft = linea.speakerName == "Bombero"; // Ajustá según tus nombres

        leftPortrait.color = isLeft ? Color.white : new Color(1, 1, 1, 0.35f);
        rightPortrait.color = !isLeft ? Color.white : new Color(1, 1, 1, 0.35f);

        if (mainCamera && leftFocus && rightFocus)
        {
            Transform target = isLeft ? leftFocus : rightFocus;
            StopAllCoroutines();
            StartCoroutine(MoverCamara(target.position));
        }
    }

    IEnumerator MoverCamara(Vector3 target)
    {
        while (Vector3.Distance(mainCamera.transform.position, target) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                new Vector3(target.x, target.y, mainCamera.transform.position.z),
                Time.deltaTime * cameraMoveSpeed
            );
            yield return null;
        }
    }

    void TerminarDialogo()
    {
        dialogueEnded = true;
        dialoguePanel.SetActive(false);
        StartCoroutine(MostrarBotonInicio());
    }

    IEnumerator MostrarBotonInicio()
    {
        yield return new WaitForSeconds(0.5f);
        startMissionPanel.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
    }

    void IniciarMision()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
