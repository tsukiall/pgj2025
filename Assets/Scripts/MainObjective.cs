using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjective : MonoBehaviour
{
    private bool isUnlocked = false;
    private Renderer objectRenderer;
    private Collider collider;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();

        SetLockedState();
    }

    public void Unlock()
    {
        isUnlocked = true;
        SetUnlockedState();
    }

    private void SetLockedState()
    {
        // za sega shte si smenq cveta - red kato e zaklucheno
        objectRenderer.material.color = Color.red; 
    }

    private void SetUnlockedState()
    {
        //green kato e otkliucheno
        objectRenderer.material.color = Color.green;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked && other.CompareTag("Player")) 
        {
            Debug.Log("Main objective collected!");
            Destroy(gameObject); 
        }
    }
}
