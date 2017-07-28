using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
public class ExitTile : Tile
{
    /// <summary>
    /// Total switches required to open the exist
    /// </summary>
    [SerializeField]
    int totalSwitches = 0;

    /// <summary>
    /// Total switches enabled
    /// </summary>
    [SerializeField]
    int activeSwitches = 0;

    /// <summary>
    /// True when not all expected switches have been enabled
    /// </summary>
    bool DoorIsLocked
    {
        get { return this.activatedSwitches.Count != this.totalSwitches; }
    }

    /// <summary>
    /// A list of switches that have notified the exit tile they are "enabled"
    /// This helps prevent the switch from activating itself more than once 
    /// before being deactivated
    /// </summary>
    List<SwitchTile> activatedSwitches = new List<SwitchTile>();

    /// <summary>
    /// Material to display when the exit is still locked
    /// </summary>
    [SerializeField]
    Material closedMaterial;

    /// <summary>
    /// Material to display when the exit is unlocked
    /// </summary>
    [SerializeField]
    Material openedMaterial;

    /// <summary>
    /// Holds a reference to renderer component
    /// </summary>
    [SerializeField]
    Renderer childRenderer;

    /// <summary>
    /// Holds a reference to particle system
    /// </summary>
    [SerializeField]
    ParticleSystem particle;

    /// <summary>
    /// The name of the scene to teleport the player to when this tile is reached
    /// </summary>
    [SerializeField]
    string nextLevelName;

    /// <summary>
    /// True when the player enters this tile after turning the switches on
    /// </summary>
    bool winIsTriggered = false;

    /// <summary>
    /// How long to wait before loading the next scene
    /// </summary>
    [SerializeField]
    float sceneLoadDelay = 1f;

    /// <summary>
    /// Sound to play when the exit is activated
    /// </summary>
    [SerializeField]
    AudioClip activedClip;

    /// <summary>
    /// Sound to play when the exit is deactivated 
    /// </summary>
    [SerializeField]
    AudioClip deactivatedClip;

    /// <summary>
    /// Sound to play when the player is on the exit
    /// </summary>
    [SerializeField]
    AudioClip exitSound;

    /// <summary>
    /// Initialize
    /// </summary>
    void Start()
    {
        this.type = Type.Exit;
        this.childRenderer = this.transform.GetChild(0).GetComponent<Renderer>();
        this.childRenderer.material = this.closedMaterial;

        // Fail safe, set it to self if next scene is unknown
        if(string.IsNullOrEmpty(this.nextLevelName)) {
            this.nextLevelName = SceneManager.GetActiveScene().name;
        }

        // Register the switches events to know when a switch is activated/deactivated
        foreach(SwitchTile switchTile in FindObjectsOfType<SwitchTile>()) {
            this.totalSwitches++;
            switchTile.SwitchActivatedEvent += this.OnSwitchOn;
            switchTile.SwitchDeactivatedEvent += this.OnSwitchOff;
        }
    }
    
    /// <summary>
    /// Listens for the switch to delegate becoming active
    /// Increases the counter of active switches
    /// </summary>
    public void OnSwitchOn(SwitchTile tile)
    {
        // Switch already active, ignore
        if(this.activatedSwitches.Contains(tile)) {
            return;
        }

        this.activeSwitches++;
        this.activatedSwitches.Add(tile);

        // All switches are active
        if( !this.DoorIsLocked) {
            this.PlaySound(this.activedClip);
            particle.Play();
            this.childRenderer.material = this.openedMaterial;
        }
    }

    /// <summary>
    /// Listens for the switch to delegate becoming unactive
    /// Decreases the counter of active switches
    /// </summary>
    public void OnSwitchOff(SwitchTile tile)
    {
        // Switch already off, ignore
        if( !this.activatedSwitches.Contains(tile) ) {
            return;
        }

        this.activeSwitches--;
        this.activatedSwitches.Remove(tile);

        // All switches are deactive
        if( this.DoorIsLocked) {
            this.PlaySound(this.deactivatedClip);
            particle.Stop();
            this.childRenderer.material = this.closedMaterial;
        }
    }

    /// <summary>
    /// Only when all switches have been turned is when this becomes enabled
    /// </summary>
    /// <returns></returns>
    public override bool IsWalkable()
    {
        return true;
    }
    
    /// <summary>
    /// Player Entered this tile
    /// Trigger win condition
    /// </summary>
    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && !this.winIsTriggered && !this.DoorIsLocked) {
            this.winIsTriggered = true;
            this.PlaySound(this.exitSound);
            StartCoroutine(this.LoadSceneAfterSeconds(this.nextLevelName, this.sceneLoadDelay));
        }
    }

    /// <summary>
    /// Loads the given scene after the given time has passed
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAfterSeconds(string sceneName, float time)
    {
        yield return new WaitForSeconds(time);

        if(sceneName == "MainMenu") {
            FindObjectOfType<LevelController>().MainMenu();
        } else {
            SceneManager.LoadScene(sceneName);
        }        
    }
}

