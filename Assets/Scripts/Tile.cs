using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all tile types
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class Tile : MonoBehaviour
{
    /// <summary>
    /// The different types of tiles
    /// </summary>
    public enum Type
    {
        Normal,
        Hole,
        Switch,
        Exit,
    }
    
    /// <summary>
    /// This tile's type
    /// </summary>
    public Type type;

    /// <summary>
    /// True when the tile has been trigger to be enabled
    /// By default all tiles are off since until the player triggers them
    /// </summary>
    [SerializeField]
    protected bool hasObject = false;

    /// <summary>
    /// Holds the game object that is currently on the tile 
    /// </summary>
    protected GameObject objectOnTile;

    /// <summary>
    /// A reference to the audio source component
    /// </summary>
    protected AudioSource audioSource;

    /// <summary>
    /// Initializes
    /// </summary>
    void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Triggers the behavior when tile is enabled
    /// </summary>
    public abstract bool IsWalkable();

    /// <summary>
    /// Triggers the behavior when tile is disabled
    /// </summary>
    public virtual bool IsAvailable(){ return !this.hasObject; }

    /// <summary>
    /// Plays the soundclip given
    /// </summary>
    /// <param name="clip"></param>
    protected void PlaySound(AudioClip clip)
    {
        this.audioSource.clip = clip;
        this.audioSource.Play();
    }
}