using UnityEngine;
using UnityEngine.UI;

public class InGameDialogueTrigger : MonoBehaviour
{
    public InGameDialogue inGameDialogue;
    public Image sprite;
    public string dialogueID;

    [TextArea(2, 5)] public string textoDelJefe;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject.CompareTag("Player")) 
        {
            triggered = false;
            inGameDialogue.TriggerDialogue(dialogueID, textoDelJefe, sprite);
        }
    }
}
