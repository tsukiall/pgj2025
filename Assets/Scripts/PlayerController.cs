using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float movementSpeed = 1.0f;
    private CharacterActions characterActions;
    private Vector2 movementInput;

    private void Awake() {
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
        transform.position += new Vector3(movementInput.x, 0f, movementInput.y) * movementSpeed * Time.deltaTime;
    }
}
