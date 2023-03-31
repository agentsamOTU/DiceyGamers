using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
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
