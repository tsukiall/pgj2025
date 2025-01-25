using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float passiveVelocity;

    [SerializeField]
    private float boostVelocity;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float terminalVelocity;

    [SerializeField]
    private float burstDelay;
    [SerializeField]
    private float burstForce;

    private Rigidbody rb;
    private InputAction burst;
    private Animator animator;
    private Vector2 movementInput;
    private bool isBursting = false;
    private CharacterActions characterActions;

    private Splitter splitter;

    private float ClampVelocity(float input) {
        return Mathf.Clamp(input, -terminalVelocity - boostVelocity * Mathf.Clamp(movementInput.y, 0, 1), terminalVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1));
    }

    private IEnumerator Burst() {
        rb.velocity = Vector3.zero;
        isBursting = true;
        animator.SetBool("isBursting", true);

        yield return new WaitForSeconds(burstDelay);

        rb.AddForce(transform.forward * burstForce, ForceMode.Impulse);
        isBursting = false;
        animator.SetBool("isBursting", false);
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        characterActions = new CharacterActions();
        burst = characterActions.Player_Map.Burst;
        burst.performed += (_) => {
            StartCoroutine("Burst");
        };
    }

    private void OnEnable() {
        characterActions.Player_Map.Enable();
    }

    private void OnDisable() {
        characterActions.Player_Map.Disable();
    }

    private void FixedUpdate() {
        movementInput = characterActions.Player_Map.Movement.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * rotationSpeed * movementInput.x);

        if (!isBursting) {
            rb.AddForce(transform.forward * (passiveVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1)));
            rb.velocity = new Vector3(ClampVelocity(rb.velocity.x), 0f, ClampVelocity(rb.velocity.z));
        }
    }

    

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hitter") && other.isTrigger == false) {
            // Destroy the Hitter GameObject
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Splitter") && other.isTrigger == false) {
            // Get the Splitter script from the Splitter GameObject
            splitter = other.gameObject.GetComponent<Splitter>();
            if (splitter != null) {
                if (splitter.splitCount > 0) {
                    // Call the Split method
                    splitter.Split();
                } else {
                    // Destroy the Splitter if it cannot split further
                    Destroy(other.gameObject);
                }
            }
        } else if (other.CompareTag("Sticker") && other.isTrigger == false) {
            // Destroy the Sticker GameObject
            Destroy(other.gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision detected!");

        // If the player's collider hits a Sticker
        if (collision.collider.CompareTag("Sticker")) {
            // Get the Sticker component from the collided object
            Sticker sticker = collision.collider.gameObject.GetComponent<Sticker>();
            if (sticker != null) {
                // Call the Stick function on the Sticker script
                sticker.Stick(transform);
            }
        } else if (collision.collider.CompareTag("LightWall")) {
            Debug.Log("Player entered detection range. Blob is chasing!");
        } else if (collision.collider.CompareTag("DarkWall")) {
            // Destroy the object when colliding with a DarkWall
            Destroy(gameObject);
        }
    }
}
