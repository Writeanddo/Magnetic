using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticPlayer : MonoBehaviour
{
    /// <summary>
    /// A reference to the arrows that indicate 
    /// whether the player is attacking or repelling
    /// </summary>
    [SerializeField]
    GameObject directionArrowsGO;

    /// <summary>
    /// How fast the lpayer moves
    /// </summary>
    [SerializeField]
    float speed = 5f;

    /// <summary>
    /// How fast the player rotates
    /// </summary>
    [SerializeField]
    float rotateSpeed = 15f;

    /// <summary>
    /// How close to the destination before snapping into place
    /// </summary>
    [SerializeField]
    float distancePadding = 0.1f;

    /// <summary>
    /// Holds the player movement input
    /// </summary>
    Vector3 movementInput = Vector3.zero;

    /// <summary>
    /// True: the player wants to attrack itmes
    /// </summary>
    bool isAttracking = false;
	
	
    /// <summary>
    /// Saves the player's input
    /// Process the move request
    /// Switches between attraction or repel
    /// </summary>
	void Update ()
    {
        this.SavePlayerInput();
        this.Move();
        //this.Rotate();

        if(this.directionArrowsGO == null) {
            return;
        }

        if(this.isAttracking) {
            this.Attrack();
        } else {
            this.Repel();
        }
	}

    /// <summary>
    /// Stores the players intented actions 
    /// </summary>
    void SavePlayerInput()
    {
        this.movementInput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        );

        // Moving to fast - normalize it
        if(this.movementInput.magnitude > 1) {
            this.movementInput.Normalize();
        }

        this.isAttracking = Input.GetButton("Fire1");
    }

    /// <summary>
    /// Smoothly transitions the player to the desired destination based on their input
    /// </summary>
    void Move()
    {

        if(this.movementInput == Vector3.zero) {
            return;
        }

        Vector3 destination = this.transform.position + this.movementInput;
        float distance = Vector3.Distance(destination, this.transform.position);

        if(distance <= 0.3f) {
            this.transform.position = destination;
            return;
        }

        Vector3 targetDestination = Vector3.Lerp(this.transform.position, destination, this.speed * Time.deltaTime);
        this.transform.position = targetDestination;
    }

    /// <summary>
    /// Rotate the player based on player input
    /// </summary>
    void Rotate()
    {
         // We get an error if this is zero
        if(this.movementInput == Vector3.zero) {
            return;
        }

        // Calculate and smooth rotate to target
        Quaternion targetRotation = Quaternion.LookRotation(this.movementInput, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(this.transform.rotation, 
                                                 targetRotation, 
                                                 this.rotateSpeed * Time.fixedDeltaTime);

        // Apply the rotation
        this.transform.Rotate(newRotation.eulerAngles);
    }

    /// <summary>
    /// Triggers the attraction logic
    /// </summary>
    void Attrack()
    {
        this.directionArrowsGO.transform.forward = new Vector3(0f, -180f, 0f);
    }

    /// <summary>
    /// Triggers the repel action
    /// </summary>
    void Repel()
    {
        this.directionArrowsGO.transform.forward = new Vector3(0f, 180f, 0f);
    }
}
