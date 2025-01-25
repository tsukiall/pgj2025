using System.Collections;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; // Speed of the blob
    [SerializeField]
    private float turnSpeed = 2f; // Speed at which the blob can turn

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
        // Get the current position of the blob (only X and Z for 2D movement)
        Vector2 blobPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerPosition = new Vector2(player.position.x, player.position.z);

        // Calculate the direction toward the player
        Vector2 direction = (playerPosition - blobPosition).normalized;

        // Smoothly rotate the blob toward the player
        Vector3 targetDirection = new Vector3(direction.x, 0, direction.y);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Move the blob forward in the direction it is facing
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
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
