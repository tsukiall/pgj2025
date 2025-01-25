using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class LightFluidWallMorph : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;
    private MeshCollider meshCollider;

    public float morphSpeed = 0.01f; // Speed at which the wall morphs
    public float morphAmount = 0.1f; // Amount of random morphing

    void Start()
    {
        // Get the mesh and its vertices
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];

        // Copy the original vertices to the modified vertices
        originalVertices.CopyTo(modifiedVertices, 0);

        // Get the MeshCollider component
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        MorphWall();
    }

    void MorphWall()
    {
        // Loop through each vertex and apply a random offset
        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            Vector3 originalPosition = originalVertices[i];
            Vector3 randomOffset = new Vector3(
                Mathf.PerlinNoise(Time.time * morphSpeed + originalPosition.x, 0f) - 0.5f,
                Mathf.PerlinNoise(Time.time * morphSpeed + originalPosition.y, 1f) - 0.5f,
                Mathf.PerlinNoise(Time.time * morphSpeed + originalPosition.z, 2f) - 0.5f
            ) * morphAmount;

            modifiedVertices[i] = originalPosition + randomOffset;
        }

        // Update the mesh with the modified vertices
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Update the MeshCollider with the new mesh
        meshCollider.sharedMesh = null; // Clear the existing mesh to ensure the collider refreshes
        meshCollider.sharedMesh = mesh;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    // If the player enters the trigger, push them back
    //    if (other.CompareTag("Player"))
    //    {
    //        // Get the Rigidbody component from the player
    //        Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
    //        if (playerRigidbody != null)
    //        {
    //            // Calculate the direction to push the player using Vector2
    //            Vector2 wallPosition = new Vector2(transform.position.x, transform.position.z);
    //            Vector2 playerPosition = new Vector2(other.transform.position.x, other.transform.position.z);
    //            Vector2 pushDirection = (playerPosition - wallPosition).normalized; // Direction vector normalized

    //            // Apply force to the player's Rigidbody
    //            float pushForce = 10f; // Adjust the force as needed
    //            Vector3 forceToApply = new Vector3(pushDirection.x, 0, pushDirection.y); // Convert back to Vector3
    //            playerRigidbody.AddForce(forceToApply * pushForce, ForceMode.Impulse);
    //        }
    //    }
    //}




}