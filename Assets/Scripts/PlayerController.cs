using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float passiveVelocity;

    [SerializeField]
    private float boostVelocity;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float terminalVelocity;

    private Rigidbody rb;
    private Vector2 movementInput;
    private CharacterActions characterActions;

    private float ClampVelocity(float input) {
        return Mathf.Clamp(input, -terminalVelocity - boostVelocity * Mathf.Clamp(movementInput.y, 0, 1), terminalVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1));
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        characterActions = new CharacterActions();
    }

    private void OnEnable() {
        characterActions.Player_Map.Enable();
    }

    private void OnDisable() {
        characterActions.Player_Map.Disable();
    }

    private void FixedUpdate() {
        movementInput = characterActions.Player_Map.Movement.ReadValue<Vector2>();
        rb.AddForce(transform.forward * passiveVelocity);
        transform.Rotate(Vector3.up * rotationSpeed * movementInput.x);
        rb.velocity = new Vector3(ClampVelocity(rb.velocity.x), 0f, ClampVelocity(rb.velocity.z));

        rb.AddForce(transform.forward * boostVelocity * Mathf.Clamp(movementInput.y, 0, 1));
    }
}
