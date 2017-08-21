using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects when an object has "filled" then hole and marks
/// the tile as "walkable" so long as the object in it is a crate
/// </summary>
public class HoleTileFilledCollider : MonoBehaviour
{
    /// <summary>
    /// A reference to the parent hole tile
    /// </summary>
    HoleTile tile;

	/// <summary>
    /// Stores the parent tile
    /// </summary>
	void Start ()
    {
        this.tile = GetComponentInParent<HoleTile>();
	}

    /// <summary>
    /// Check if the hole was filled with a crate
    /// Respawn the object if it's the ball
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        MetalBall ball = other.GetComponent<MetalBall>();
        if(ball != null) {
            ball.Respawn();
        } else {
            this.tile.isFilled = true;
        }
    }
}
