using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private Splitter splitter;

    private float ClampVelocity(float input)
    {
        return Mathf.Clamp(input, -terminalVelocity - boostVelocity * Mathf.Clamp(movementInput.y, 0, 1), terminalVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterActions = new CharacterActions();
    }

    private void OnEnable()
    {
        characterActions.Player_Map.Enable();
    }

    private void OnDisable()
    {
        characterActions.Player_Map.Disable();
    }

    private void FixedUpdate()
    {
        movementInput = characterActions.Player_Map.Movement.ReadValue<Vector2>();
        rb.AddForce(transform.forward * (passiveVelocity + boostVelocity * Mathf.Clamp(movementInput.y, 0, 1)));
        transform.Rotate(Vector3.up * rotationSpeed * movementInput.x);
        rb.velocity = new Vector3(ClampVelocity(rb.velocity.x), 0f, ClampVelocity(rb.velocity.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitter") && other.isTrigger == false)
        {
            // Destroy the Hitter GameObject
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Splitter") && other.isTrigger == false)
        {
            // Get the Splitter script from the Splitter GameObject
            splitter = other.gameObject.GetComponent<Splitter>();
            if (splitter != null)
            {
                if (splitter.splitCount > 0)
                {
                    // Call the Split method
                    splitter.Split();
                }
                else
                {
                    // Destroy the Splitter if it cannot split further
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.CompareTag("Sticker") && other.isTrigger == false)
        {
            // Destroy the Sticker GameObject
            Destroy(other.gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected!");

        // If the player's collider hits a Sticker
        if (collision.collider.CompareTag("Sticker"))
        {
            // Get the Sticker component from the collided object
            Sticker sticker = collision.collider.gameObject.GetComponent<Sticker>();
            if (sticker != null)
            {
                // Call the Stick function on the Sticker script
                sticker.Stick(transform);
            }
        }
      
        else if (collision.collider.CompareTag("LightWall"))
        {
            Debug.Log("Player entered detection range. Blob is chasing!");
        }
        else if (collision.collider.CompareTag("DarkWall"))
        {
            // Destroy the object when colliding with a DarkWall
            Destroy(gameObject);
        }
    }

}
