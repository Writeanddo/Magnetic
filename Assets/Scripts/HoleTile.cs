using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tiles that have a Hole where the player cannot walk over unless there's a crate inside of it
/// </summary>
public class HoleTile : Tile
{
    public override void IsAvailable()
    {
        throw new NotImplementedException();
    }

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
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        this.hasObject = true;
    }
}
