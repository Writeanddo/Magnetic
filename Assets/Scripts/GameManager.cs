using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles high level transactions such as:
///     - Keep track of player's progress
///     - Play music based on loaded scene
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager instance = null;

    /// <summary>
    /// A reference to the audio source component
    /// </summary>
    protected AudioSource audioSource;

    /// <summary>
    /// Audio to play on main menu
    /// </summary>
    [SerializeField]
    AudioClip menuMusicClip;

    /// <summary>
    /// Audio to play on each level
    /// </summary>
    [SerializeField]
    AudioClip levelMusicClip;

    /// <summary>
    /// The name of the first level to load
    /// </summary>
    [SerializeField]
    string firstLevelName;

    /// <summary>
    /// The name of the next level/scene to load
    /// </summary>
    [SerializeField]
    public string nextLevelName;

    /// <summary>
    /// Name of the main menu for when the player wants to return
    /// </summary>
    [SerializeField]
    string mainMenuSceneName;

    /// <summary>
    /// Prevents more than once instance of the GameManager
    /// </summary>
    void Awake()
    {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Initialize
    /// </summary>
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        this.PlayMainMenuMusic();
    }

    /// <summary>
    /// Closes the app
    /// </summary>
    public void Exit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    /// <summary>
    /// Tranistions to the first level
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(this.firstLevelName);
        this.PlayLevelMusic();
    }

    /// <summary>
    /// Tranistions to the next level
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(this.nextLevelName);
        this.PlayLevelMusic();
    }

    /// <summary>
    /// Takes the player back to the main menu
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(this.mainMenuSceneName);
        this.PlayMainMenuMusic();
    }

    /// <summary>
    /// Plays the music for a level
    /// </summary>
    public void PlayLevelMusic()
    {
        this.PlayLoopSound(this.levelMusicClip);
    }

    /// <summary>
    /// Plays the music for a level
    /// </summary>
    public void PlayMainMenuMusic()
    {
        this.PlayLoopSound(this.menuMusicClip);
    }

    /// <summary>
    /// Loops the given sound clip
    /// Triggers the play only once
    /// Ensures it is the only clip playing
    /// </summary>
    /// <param name="clip"></param>
    void PlayLoopSound(AudioClip clip)
    {
        this.audioSource.loop = true;

        // Not the current sound
        if( this.audioSource.clip != clip ) {
            this.audioSource.Stop();
            this.audioSource.clip = clip;
        }

        // Play it only if it is not currently playing
        if(!this.audioSource.isPlaying) {
            this.audioSource.Play();
        }
    }   
}
