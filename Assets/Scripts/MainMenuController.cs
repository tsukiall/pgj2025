using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button creditsButton;
    [SerializeField]
    private Button creditsBack;

    private GameManager gameManager;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        creditsButton.onClick.AddListener(() => {
            mainMenu.SetActive(false);
            credits.SetActive(true);
        });
        creditsBack.onClick.AddListener(() => {
            credits.SetActive(false);
            mainMenu.SetActive(true);
        });

        startButton.onClick.AddListener(() => {
            gameManager.StartLevel();
        });
    }
}