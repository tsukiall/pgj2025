using UnityEngine;

public class SmallObjective : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected by SmallObjective!"); // Log a message to the console
            GameManager.Instance.IncrementCounter();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Non-player object entered: " + other.tag); // Log for non-player objects
        }
    }
}
