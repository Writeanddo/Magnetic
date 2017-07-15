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
    public override void Disable(){}
    public override void Enable(){}
}
