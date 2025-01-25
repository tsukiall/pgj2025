using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    private int objectiveCounter = 0; 
    public MainObjective mainObjective; 
    private int playerHealth = 100; 

    private SmallObjective[] objectives;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
<<<<<<< Updated upstream
            DontDestroyOnLoad(gameObject); // Optional: Persist the manager between scenes
        } else {
=======
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
>>>>>>> Stashed changes
            Destroy(gameObject);
        }
    }

    private void Start() {
        objectives = GameObject.FindObjectsOfType<SmallObjective>();
    }

    public void IncrementCounter() {
        objectiveCounter++;
        Debug.Log("Small objectives collected: " + objectiveCounter);

        if (objectiveCounter >= 3) 
        {
            mainObjective.Unlock();
        }
    }

<<<<<<< Updated upstream
    public SmallObjective[] GetSmallObjectives() {
        return objectives; 
=======
    public void ReducePlayerHealth()
    {
        playerHealth -= 10; 
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0)
        {
            Debug.Log("Player has died!");
            
        }
>>>>>>> Stashed changes
    }
}
