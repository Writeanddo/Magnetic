using UnityEngine;
using System.Collections;

/// <summary>
/// Interface for all objects that can move from tile to tile
/// Unlike an attractable which simply attaches or repels from the player
/// a moveable is designed to either move with the player or move
/// along a given direction until told to stop
/// </summary>
public interface IMoveable
{
    bool CanFall();
}
