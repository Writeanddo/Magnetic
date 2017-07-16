using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic simple tile that is not affected by the player's ability
/// </summary>
public class NormalTile : Tile
{
    /// <summary>
    /// Nothing happens for Normal tiles
    /// </summary>
    public override void IsAvailable(){}
    public override bool IsWalkable(){ return !this.hasObject; }

    /// <summary>
    /// Triggers the has object true to prevent another from entering this tile
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        this.hasObject = false;

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
