using UnityEngine;

public class AlarmButton : MonoBehaviour
{
    public AudioSource alarm;
    public bool isOn;

    private void Start()
    {
        isOn = true;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isOn)
            {
                alarm.Stop();
                isOn = false;
            }
        }
    }
}
