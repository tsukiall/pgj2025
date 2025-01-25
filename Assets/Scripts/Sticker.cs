using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; // Speed of the blob
    [SerializeField]
    private float turnSpeed = 0.5f; // Speed at which the blob can turn

    private Transform player; // Reference to the player's transform
    private bool isChasing = false; // Whether the blob is following the player
    private bool isStuck = false; // Whether the sticker is stuck to the player

    void Update()
    {
        if (isChasing && player != null)
        {
            // Follow the player
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (isStuck) return; // Stop moving if already stuck

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && player != null)
        {
            // Get the player's position in 2D (x, z plane)
            Vector2 blobPosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 playerPosition = new Vector2(player.position.x, player.position.z);

            // Calculate the direction toward the player
            Vector2 direction = (playerPosition - blobPosition).normalized;

            // Add force to move toward the player
            rb.AddForce(new Vector3(direction.x, 0, direction.y) * moveSpeed);

            // Smooth rotation
            Vector3 targetDirection = new Vector3(direction.x, 0, direction.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Clamp velocity to ensure the Sticker doesn't exceed terminal speed
            float terminalVelocity = 10f; // Define terminal velocity
            rb.linearVelocity = new Vector3(
                Mathf.Clamp(rb.linearVelocity.x, -terminalVelocity, terminalVelocity),
                rb.linearVelocity.y, // Preserve Y velocity for gravity
                Mathf.Clamp(rb.linearVelocity.z, -terminalVelocity, terminalVelocity)
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start following the player
            player = other.transform;
            isChasing = true; // Set isChasing to true
        }
    }

    public void Stick(Transform playerMan)
    {
        if (!isStuck)
        {
            // Stick to the player
            isStuck = true;
            isChasing = false; // Stop chasing
            player = playerMan;

            // Slow down the player
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Reduce velocity by 5 units
                Vector3 reducedVelocity = playerRb.linearVelocity.normalized * Mathf.Max(0, playerRb.linearVelocity.magnitude - 5);
                playerRb.linearVelocity = reducedVelocity;
            }

            // Parent the sticker to the player
            transform.parent = player;
        }
    }
}
