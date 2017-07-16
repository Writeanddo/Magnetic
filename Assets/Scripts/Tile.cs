using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// Triggers the behavior when tile is enabled
    /// </summary>
    public abstract bool IsWalkable();

    /// <summary>
    /// Triggers the behavior when tile is disabled
    /// </summary>
    public virtual bool IsAvailable(){ return !this.hasObject; }
}