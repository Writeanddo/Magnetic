﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a list of all tile locations to know which tiles are available to the player
/// Handles all behavior within the level including loading and level completion
/// </summary>
public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Holds a list of a tile positions on the map 
    /// As well as the tile script itself for quick reference
    /// </summary>
    Dictionary<Vector3, Tile> tilePositions = new Dictionary<Vector3, Tile>();

    /// <summary>
    /// A list of the angles the camera can cycle through
    /// </summary>
    List<float> rotationAngles = new List<float>() {0f, 90f, 180f, 270f};

    /// <summary>
    /// The current index that represents the rotation the camera has
    /// </summary>
    int currentAngleIndex;

    /// <summary>
    /// Holds the direction the player wants to rotate the camera to
    /// </summary>
    Vector3 rotationInput = Vector3.zero;

    /// <summary>
    /// Where to rotate the camera to
    /// </summary>
    Vector3 desiredRotation;

    /// <summary>
    /// How fast to rotate
    /// </summary>
    [SerializeField]
    float rotationSpeed;

    /// <summary>
    /// Minimum angle before snapping into desired rotation
    /// </summary>
    [SerializeField]
    float angleDamping = 1f;

    /// <summary>
    /// Creates the tilemap
    /// </summary>
    void Start ()
    {
        Tile[] tiles = FindObjectsOfType<Tile>() as Tile[];
        foreach(Tile tile in tiles) {
            // Ignore y axis as it needs to be 0
            Vector3 position = new Vector3(
                tile.transform.position.x,
                0f,
                tile.transform.position.z
            );

            // Avoid duplicates
            if( !this.tilePositions.ContainsKey(position) ) {
                this.tilePositions.Add(position, tile);
            }            
        }

        // Save the current angle
        int index = this.rotationAngles.IndexOf(this.transform.rotation.eulerAngles.y);

        // Not a recognized rotation
        // Default to the first rotation
        if(index == -1) {
            this.currentAngleIndex = 0;
            float angle = this.rotationAngles[this.currentAngleIndex];
            Vector3 targetRotation = angle * Vector3.up;
            this.transform.rotation = Quaternion.Euler(targetRotation);

        // Save the current rotation
        } else {
            this.desiredRotation = this.transform.eulerAngles;
        }

	}

    /// <summary>
    /// Stores the player's intended camera rotation
    /// </summary>
    void Update()
    {
        //this.SavePlayerInput();
    }

    /// <summary>
    /// Chose late update to allow any moving objects time to finish 
    /// their movement before rotating the camera
    /// </summary>
    void LateUpdate()
    {
        //this.Rotate();
    }

    /// <summary>
    /// Stores the direction the player wants to rotate the camera
    /// </summary>
    void SavePlayerInput()
    {
        // If we have not reached the desired rotation then ignore input
        //if(this.transform.eulerAngles != this.desiredRotation) {
        //    return;
        //}

        float left = Input.GetKeyDown(KeyCode.Q) ? -1:0;
        float right = Input.GetKeyDown(KeyCode.E) ? 1:0;
        int index = this.currentAngleIndex + (int)(left + right);

        // Back of the line
        if(index < 0) {
            index = this.rotationAngles.Count - 1;
        }

        // Back to the start
        if(index > this.rotationAngles.Count - 1) {
            index = 0;
        }

        this.currentAngleIndex = index;
        this.desiredRotation = this.rotationAngles[index] * Vector3.up;
    }

    /// <summary>
    /// Rotates the LevelController which houses all the level component
    /// thus giving the illusion of rotating the camera 
    /// </summary>
    public void Rotate()
    {
        // Calculate new rotation 
        Quaternion targetRotation = Quaternion.Euler(this.desiredRotation);
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, 
                                                 targetRotation, 
                                                 this.rotationSpeed * Time.fixedDeltaTime);

        // Snap into rotation
        if(Quaternion.Angle(this.transform.rotation, targetRotation) <= this.angleDamping) {
            this.transform.rotation = targetRotation;
        } else {
            this.transform.rotation = newRotation;
        }
    }

    /// <summary>
    /// Translates the given position from local coordinates to coordinates
    /// that match the direction the camera is facing
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 LocalToGlobalPosition(Vector3 position)
    {
        Vector3 globalPosition =  Camera.main.transform.TransformDirection(position);
        Vector3 newPosition = new Vector3(globalPosition.x, 0f, globalPosition.z);
        return newPosition;
    }
	
    /// <summary>
    /// Checks if the given position has a walkable/available tile
    /// Returns True if the position is available
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
	public bool IsPositionAvailable(Vector3 position)
    {
        if(!this.tilePositions.ContainsKey(position)) {
            return false;
        }

        Tile tile = this.tilePositions[position];
        return tile.IsWalkable();
    }

    /// <summary>
    /// True if there is no tile or the tile is walkable
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsTileAtPositionAvailable(Vector3 position)
    {
        if(!this.tilePositions.ContainsKey(position)) {
            return true;
        }

        Tile tile = this.tilePositions[position];
        return tile.IsAvailable();
    }
}
