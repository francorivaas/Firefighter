using UnityEngine;

public class InGameDialogueTrigger : MonoBehaviour
{
    public InGameDialogue inGameDialogue;
    public string dialogueID;
    public string speakerName;

    [TextArea(2, 5)]
    public string textoDelJefe;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            inGameDialogue.TriggerDialogue(dialogueID, textoDelJefe, speakerName);
        }
    }
}