using FMODUnity;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SmallObjective : ObjectiveController {
    private Animator animator;
    private bool broken = false;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator SelfDistruct() {
        yield return new WaitForSeconds(12);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !broken) {
            broken = true;
            GameManager.Instance.IncrementCounter(this);
            animator.SetTrigger("PickUp");
            GetComponent<StudioEventEmitter>().Play();
            StartCoroutine("SelfDistruct");
        }
    }
}
