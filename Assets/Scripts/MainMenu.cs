using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to allow buttons to trigger the singleton instance of the gamemanager
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// A reference to the Level Selection GameObject
    /// </summary>
    [SerializeField]
    GameObject MainSelectGO;

    /// <summary>
    /// A reference to the Level Selection GameObject
    /// </summary>
    [SerializeField]
    GameObject LevelSelectGO;

    /// <summary>
    /// Hides the main select 
    /// Shows the level select
    /// </summary>
	public void PlayGame()
    {
        this.MainSelectGO.SetActive(false);
        this.LevelSelectGO.SetActive(true);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Exit()
    {
        GameManager.instance.Exit();
    }

    /// <summary>
    /// Load the given scene
    /// </summary>
    /// <param name="levelButton"></param>
    public void GoToScene(LevelSelectButton levelButton)
    {
        GameManager.instance.LoadLevel(levelButton.SceneName);
    }
}
