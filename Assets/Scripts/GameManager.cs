using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int objectiveCounter = 0; // Counter for collected small objectives
    public MainObjective mainObjective; // Reference to the main objective

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist the manager between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementCounter()
    {
        objectiveCounter++;
        Debug.Log("Small objectives collected: " + objectiveCounter);

        if (objectiveCounter >= 3) // Unlock the main objective
        {
            mainObjective.Unlock();
        }
    }
}
