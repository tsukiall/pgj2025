using System.Collections;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; // Speed of the blob
    [SerializeField]
    private float turnSpeed = 0.5f; // Speed at which the blob can turn

    private Transform player; // Reference to the player's transform
    private bool isChasing = false; // Whether the blob is following the player

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player has the tag 'Player'.");
        }
    }

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

            // Clamp velocity to ensure the Hitter doesn't exceed terminal speed
            float boostVelocity = 0f; // Boost velocity if you want to add extra speed dynamically
            float terminalVelocity = 10f; // Define terminal velocity
            rb.linearVelocity = new Vector3(
                Mathf.Clamp(rb.linearVelocity.x, -terminalVelocity - boostVelocity, terminalVelocity + boostVelocity),
                rb.linearVelocity.y, // Preserve Y velocity for gravity
                Mathf.Clamp(rb.linearVelocity.z, -terminalVelocity - boostVelocity, terminalVelocity + boostVelocity)
            );
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger, start chasing
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("Player entered detection range. Blob is chasing!");
        }
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Blob collider hit!");

        // If the blob's collider hits the player's collider, deal damage and destroy the blob
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Blob hit the player!");

            // Call GameManager to deal damage to the player
            GameManager.Instance.ReducePlayerHealth();

            // Destroy the blob
            //Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("LightWall"))
        {
            Debug.Log("Player entered detection range. Blob is chasing!");

        }
        else if (collision.collider.CompareTag("DarkWall"))
        {
            Destroy(gameObject);

        }
    }
}
