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
    /// This tile is always available regardless of what's inside or not
    /// </summary>
    /// <returns></returns>
    public override bool IsAvailable(){ return true; }


    /// <summary>
    /// Walkable only when it has an object in it
    /// </summary>
    /// <returns></returns>
    public override bool IsWalkable()
    {
        return this.hasObject;
    }

    /// <summary>
    /// Hole has been filled with an object
    /// This is called also when the sound trigger collider is entered
    /// If that is the case then play the sound of an object entering
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        MetalBall ball = other.GetComponent<MetalBall>();
        if(ball != null) {
            ball.Respawn();
        } else {
            this.hasObject = true;
        }
    }
}
