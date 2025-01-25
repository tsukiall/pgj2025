using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class DarkFluidWallMorph : MonoBehaviour
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



}