using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public GameObject cameraCutscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cameraCutscene.GetComponent<Animator>().SetBool("Trigger", true);
        }
    }
}
