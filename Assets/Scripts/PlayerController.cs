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
    private bool hasAttacked = false;

    public CharacterActions characterActions;

    private float ClampVelocity(float input) {
        return Mathf.Clamp(input, -terminalVelocity - boostVelocity * Mathf.Clamp(movementInput.y, 0, 1), terminalVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1));
    }

    private IEnumerator Burst() {
        rb.linearVelocity = Vector3.zero;
        isBursting = true;
        animator.SetBool("isBursting", true);

        yield return new WaitForSeconds(burstDelay);

        rb.AddForce(transform.forward * burstForce, ForceMode.Impulse);
        isBursting = false;
        animator.SetBool("isBursting", false);
    }

    private IEnumerator Cooldown() {
        hasAttacked = true;

        yield return new WaitForSeconds(1f);

        hasAttacked = false;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        characterActions = new CharacterActions();
        burst = characterActions.Player_Map.Burst;
        burst.performed += (_) => {
            GameManager.Instance.burstEvent.Invoke();
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

        if (movementInput.x < 0) {
            animator.SetFloat("isTurning", -1f);
        } else {
            animator.SetFloat("isTurning", 1f);
        }

        if (!isBursting) {
            rb.AddForce(transform.forward * (passiveVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1)));
            rb.linearVelocity = new Vector3(ClampVelocity(rb.linearVelocity.x), 0f, ClampVelocity(rb.linearVelocity.z));
        }
    }

    

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy") && other.isTrigger == false && !hasAttacked) {
            rb.AddForce(-transform.forward * 5f);
            StartCoroutine("Cooldown");
            other.GetComponent<Enemy>().Hit();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        animator.SetTrigger("isHit");
    }
}
