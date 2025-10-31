using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.rescuedCitizens >= GameManager.Instance.minCitizens)
                SceneManager.LoadScene(1);
            else print("no rescataste a nadie logi");
        }
    }
}
