using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to allow buttons to trigger the singleton instance of the gamemanager
/// </summary>
[RequireComponent(typeof(AudioSource))]
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
    /// A reference to the audio source component
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// Initialize
    /// </summary>
    void Start()
    {
        Cursor.visible = true;
        this.audioSource = GetComponent<AudioSource>();
        GameManager.instance.PlayMainMenuMusic();
    }

    /// <summary>
    /// Hides the main select 
    /// Shows the level select
    /// </summary>
	public void PlayGame()
    {
        this.audioSource.Play();
        if(this.MainSelectGO != null) {
            this.MainSelectGO.SetActive(false);
        }
        if(this.LevelSelectGO != null) {
            this.LevelSelectGO.SetActive(true);
        }
    }

    /// <summary>
    /// Goes to the main menu
    /// </summary>
    public void GoToMainMenu()
    {
         this.audioSource.Play();
        GameManager.instance.LoadLevel("MainMenu");
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Exit()
    {
        this.audioSource.Play();
        GameManager.instance.Exit();
    }

    /// <summary>
    /// Load the given scene
    /// </summary>
    /// <param name="levelButton"></param>
    public void GoToScene(LevelSelectButton levelButton)
    {
        this.audioSource.Play();
        GameManager.instance.LoadLevel(levelButton.SceneName);
    }
}
