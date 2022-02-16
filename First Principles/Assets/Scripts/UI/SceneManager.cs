using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public enum CurrentState
    {
        MENU, GAME
    };
  
    public void LoadScene(CurrentState cs) => cs switch
    {
        CurrentState.MENU => SceneManager.LoadScene("Menu"),
        CurrentState.GAME => SceneManager.LoadScene("Game"),
        _   => SceneManager.LoadScene("Menu")
    };

    public void ChangeToMenu()
    {
        SwitchScenes(CurrentState.MENU);
    }

    public void ChangeToGame()
    {
        SwitchScenes(CurrentState.GAME);
    }
}