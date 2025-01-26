using System.Collections;
using UnityEngine;

public class LightWallController : MonoBehaviour {
    [SerializeField]
    private float delay;

    private IEnumerator Burst() {
        GetComponent<Collider>().enabled = false;
        
        yield return new WaitForSeconds(delay);

        GetComponent<Collider>().enabled = true;
    }

    private void Awake() {
        GameManager.Instance.burstEvent.AddListener(() => {
            StartCoroutine("Burst");
        });
    }
}
