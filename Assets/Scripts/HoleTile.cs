using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tiles that have a Hole where the player cannot walk over unless there's a crate inside of it
/// </summary>
public class HoleTile : Tile
{
    /// <summary>
    /// Initialize
    /// </summary>
    void Start()
    {
        this.type = Type.Hole;
    }

    /// <summary>
    /// True when the there's a crate in it
    /// </summary>
    internal bool isFilled = false;

    /// <summary>
    /// This tile is always available regardless of what's inside or not
    /// </summary>
    /// <returns></returns>
    public override bool IsAvailable(){ return !this.hasObject; }


    /// <summary>
    /// Walkable when it has been filled and there are no other objects on it
    /// </summary>
    /// <returns></returns>
    public override bool IsWalkable()
    {
        return this.isFilled && !this.hasObject;
    }

    /// <summary>
    /// Hole has been filled with an object
    /// This is called also when the sound trigger collider is entered
    /// If that is the case then play the sound of an object entering
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        this.hasObject = false;

        // It's the player
        if(other.tag == "Player") {
            this.hasObject = true;
            this.objectOnTile = other.gameObject;
            return;
        }

        // If this is an attractable object then will ignore it if its being held
        IAttractable attractable = other.GetComponent<IAttractable>();
        if(attractable != null && !attractable.IsAttached) {
            this.hasObject = true;
            this.objectOnTile = other.gameObject;
        }
    }

    /// <summary>
    /// Object has left
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == this.objectOnTile) {
            this.hasObject = false;
            this.objectOnTile = null;
        }
    }
}
