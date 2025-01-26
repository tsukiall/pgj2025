using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    private int objectiveCounter = 0; 
    public MainObjective mainObjective; 
    private int playerHealth = 100; 

    private List<SmallObjective> objectives;

    public UnityEvent burstEvent;

    private void Awake() {
        if (Instance == null) {
            Instance = this;

            DontDestroyOnLoad(gameObject); // Optional: Persist the manager between scenes
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        objectives = new List<SmallObjective>(FindObjectsOfType<SmallObjective>());
    }

    public void IncrementCounter(SmallObjective objective) {
        objectiveCounter++;
        objectives.Remove(objective);

        if (objectiveCounter >= 3) {
            mainObjective.Unlock();
        }
    }

    public List<SmallObjective> GetSmallObjectives() {
        return objectives;
    }

    public void ReducePlayerHealth() {
        playerHealth -= 10;
        Debug.Log("Player Health: " + playerHealth);

        if (playerHealth <= 0) {
            Debug.Log("Player has died!");
        }
    }

    public void StartLevel() {
        GetComponent<StudioEventEmitter>().SetParameter("inGame", 1);
        SceneManager.LoadScene("Level 1");
    }
}
