using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, IEnableable
{
    /// <summary>
    /// True when the tile has been trigger to be enabled
    /// By default all tiles are off since until the player triggers them
    /// </summary>
    [SerializeField]
    protected bool isEnabled = false;

    /// <summary>
    /// Triggers the behavior when tile is enabled
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// Triggers the behavior when tile is disabled
    /// </summary>
    public abstract void Disable();
}