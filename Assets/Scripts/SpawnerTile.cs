using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A spawner tile behaves like a normal tile with the added feature
/// that it told which type of spawner tile it is to changed the material 
/// to display what objects "spawns" on this tile
/// </summary>
public class SpawnerTile : NormalTile
{
    /// <summary>
    /// Types of spawner
    /// Mainly used to determine which material to us
    /// </summary>
    enum Type
    {
        CrateSpawner,
        BallSpawner,
    }

    /// <summary>
    /// Which type of spawner tile this is
    /// </summary>
    [SerializeField]
    Type spawnerType;

    /// <summary>
    /// A reference to the model's mesh renderer
    /// </summary>
    [SerializeField]
    new MeshRenderer renderer;

    /// <summary>
    /// Material to use when the object to spawn is a create
    /// </summary>
    [SerializeField]
    Material crateSpawnerMaterial;

    /// <summary>
    /// Material to use when the object to spawn is a metal bal
    /// </summary>
    [SerializeField]
    Material ballpawnerMaterial;

    /// <summary>
    /// True once the material for this spawner has been set
    /// </summary>
    bool materialSet = false;
    
    /// <summary>
    /// Because the Tile and NormalTile parents already use Awake and Start
    /// we use Update to set the material especially since we cannot override
    /// the OnTriggerStay()
    /// </summary>
    void Update()
    {
        if(!this.materialSet) {
            this.materialSet = true;

            switch(this.spawnerType) {
                case Type.CrateSpawner:
                    this.renderer.material = this.crateSpawnerMaterial;
                    break;
                case Type.BallSpawner:
                    this.renderer.material = this.ballpawnerMaterial;
                    break;
            }
        }
    }
}
