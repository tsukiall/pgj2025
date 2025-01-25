using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; // Speed of the blob
    [SerializeField]
    private float turnSpeed = 0.5f; // Speed at which the blob can turn

    public int splitCount = 4; // Number of times the splitter can split

    private Transform player; // Reference to the player's transform
    private bool isChasing = false; // Whether the blob is following the player

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

    // goes to the player 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start following the player
            player = other.transform;
        }
    }

    public void Split()
    {
        if (splitCount <= 0) return; // Prevent infinite splitting

        // Create two smaller splitters
        for (int i = 0; i < 2; i++)
        {
            // Create a new GameObject
            GameObject smallerSplitter = new GameObject($"Splitter_{splitCount}_{i}");
            smallerSplitter.transform.position = transform.position; // Start at the same position
            smallerSplitter.transform.localScale = transform.localScale * 0.5f; // Scale down
            smallerSplitter.transform.rotation = Quaternion.Euler(0, i * 180, 0); // Offset rotation

            // Add visual components (MeshFilter and MeshRenderer)
            MeshFilter meshFilter = smallerSplitter.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = smallerSplitter.AddComponent<MeshRenderer>();

            // Copy mesh and material from the current splitter
            meshFilter.mesh = GetComponent<MeshFilter>().mesh;
            meshRenderer.material = GetComponent<MeshRenderer>().material;

            // Add behavior components
            Splitter splitterScript = smallerSplitter.AddComponent<Splitter>();
            splitterScript.moveSpeed = moveSpeed * 2f; // Double the speed
            splitterScript.splitCount = splitCount - 1; // Reduce split count

            // Add Rigidbody and Collider
            Rigidbody rb = smallerSplitter.AddComponent<Rigidbody>();
            rb.useGravity = false;

            SphereCollider collider = smallerSplitter.AddComponent<SphereCollider>();
            collider.isTrigger = false; // Adjust based on your gameplay needs
        }
    }
}
