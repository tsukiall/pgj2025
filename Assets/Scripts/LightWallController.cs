using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public class LightWallController : MonoBehaviour {
    [SerializeField]
    private float delay;

    private InputAction burst;
    private CharacterActions characterActions;

    private IEnumerator Burst() {
        GetComponent<Collider>().enabled = false;
        
        yield return new WaitForSeconds(delay);

        GetComponent<Collider>().enabled = true;
    }

    private void Awake() {
        characterActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().characterActions;
        burst = characterActions.Player_Map.Burst;
        burst.performed += (_) => {
            StartCoroutine("Burst");
        };
    }
}
