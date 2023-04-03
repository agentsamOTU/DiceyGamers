using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public DiceGame game;
    public Sockets Sockets;

    public TextMeshProUGUI pBet;
    public TextMeshProUGUI pWincon;
    public TextMeshProUGUI pDice;
    public TextMeshProUGUI pCash;

    private void Awake()
    {
        Instance = this;
    }

    public void ExitGame()
    {
        Application.Quit(); //exits the application
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game"); //loads the game
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reloads current scene
    }

}
