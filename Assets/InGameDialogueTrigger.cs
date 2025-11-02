using UnityEngine;
using UnityEngine.UI;

public class InGameDialogueTrigger : MonoBehaviour
{
    public InGameDialogue inGameDialogue;
    public Image sprite;
    public string dialogueID = "D_1";
    [TextArea(2, 5)] public string textoDelJefe;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject.CompareTag("Player")) 
        {
            triggered = false;
            inGameDialogue.TriggerDialogue("D_1","No podrás rescatar a este ciudadano sin un cuchillo para cortar las cuerdas... seguro que hay uno por aquí. ¡Ve a buscarlo!", sprite);
        }
    }
}
