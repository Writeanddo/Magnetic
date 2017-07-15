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

    public override bool IsWalkable()
    {
        return false;
    }
}
