using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player movement 
/// Handles attracting and repelling objects
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// A reference to the levelController
    /// </summary>
    LevelController levelController;

    /// <summary>
    /// Stores the directions the player wants to move
    /// </summary>
    Vector3 inputVector = Vector3.zero;

    /// <summary>
    /// Where the player wants to move towards
    /// </summary>
    Vector3 destination;

    /// <summary>
    /// How close to the destination before snapping into place
    /// </summary>
    [SerializeField]
    float distancePadding;

    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField]
    float speed;
    public float Speed { get { return this.speed; } }

	/// <summary>
    /// Initialize
    /// </summary>
	void Start ()
    {
        this.levelController = FindObjectOfType<LevelController>();
        this.destination = this.transform.position;
	}

    /// <summary>
    /// Updates the player's input
    /// </summary>
    void Update()
    {
        this.SavePlayerInput();
        this.Move();
    }

    /// <summary>
    /// Stores the player's movement input as well as their action input
    /// </summary>
    void SavePlayerInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

         // Horizontal takes prescedence
        if(h != 0 && v != 0) {
            v = 0f;
        }

        this.inputVector = new Vector3(h, 0f, v);
    }

    /// <summary>
    /// Handles moving the player to the desired destination
    /// as well as any and all objects the player is currently attached to
    /// unless the player is attached to a wall, the wall does not move
    /// </summary>
    void Move()
    {
        float distance = Vector3.Distance(this.destination, this.transform.position);

        // Made it, snap to destination and update the destination based on player input
        if(distance <= this.distancePadding) {
            this.transform.position = this.destination;
            
            // Where the player could potentially move
            Vector3 targetDestination = this.transform.position + this.inputVector;

            // Tile is available
            if(this.levelController.IsPositionAvailable(targetDestination)) {
                this.destination = targetDestination;
            }

        // Still Moving
        } else {
            Vector3 targetDestination = Vector3.Lerp(this.transform.position, destination, this.speed * Time.deltaTime);
            this.transform.position = targetDestination;
        }
    }
	
}
