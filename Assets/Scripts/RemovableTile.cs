using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A removable tile is a tile that will either appear or dissapear
/// when triggered
/// </summary>
[RequireComponent(typeof(Animator))]
public class RemovableTile : Tile
{
    /// <summary>
    /// List of actions a removable tile can do
    /// </summary>
    enum Action
    {
        Hide,
        Show,
    }

    /// <summary>
    /// Which action to trigger when this tile is enabled
    /// </summary>
    [SerializeField]
    Action onEnable;

    /// <summary>
    /// Holds a reference to the animator component
    /// </summary>
    Animator animator;

    /// <summary>
    /// Initializes the class
    /// </summary>
    void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets the current state of the tile based what action to perform when enabled
    /// </summary>
    void Start()
    {
        // Hides the tile as the default has the tile showing
        if(this.onEnable == Action.Show) {
            this.animator.SetTrigger("Hide");
        }        
    }

    /// <summary>
    /// Hides or Shows the tile depending on the <see cref="this.onEnabled"/> action type
    /// </summary>
    public override void Enable()
    {
        if(this.onEnable == Action.Show) {
            this.ShowTile();
        } else {
            this.HideTile();
        }    
    }

	/// <summary>
    /// Hides or Shows the tile depending on the <see cref="this.onEnabled"/> action type
    /// </summary>
    public override void Disable()
    {
        if(this.onEnable == Action.Show) {
            this.HideTile();
        } else {
            this.ShowTile();
        }        
    }

    /// <summary>
    /// Triggers the "show tile" animation 
    /// </summary>
    void ShowTile()
    {
        if(!this.isEnabled) {
            this.isEnabled = true;
            this.animator.SetTrigger("Enabled");
        }
    }

    // <summary>
    /// Triggers the "hide tile" animation 
    /// </summary>
    void HideTile()
    {
        if(this.isEnabled) {
            this.isEnabled = false;
            this.animator.SetTrigger("Disabled");
        }
    }
}
