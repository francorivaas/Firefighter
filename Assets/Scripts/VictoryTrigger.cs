using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.rescuedCitizens >= GameManager.Instance.minCitizens)
                SceneManager.LoadScene(1);
            else print("no rescataste a nadie logi");
        }
    }
}
