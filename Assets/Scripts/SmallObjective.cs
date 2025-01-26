using UnityEngine;

public class SmallObjective : ObjectiveController {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.Instance.IncrementCounter(this);
            Destroy(gameObject);
        }
    }
}
