using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player movement 
/// Handles attracting and repelling objects
/// </summary>
public class PlayerController : MonoBehaviour, IMagnetic
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
    /// Total tiles the player can push/repel and object away
    /// </summary>
    [SerializeField]
    int repelTileDistance = 2;
    public int RepelTileDistance
    {
        get
        {
            return this.repelTileDistance;
        }
    }

    /// <summary>
    /// How many "tiles" away from the player can an object be attracted from
    /// </summary>
    [SerializeField]
    int attractionDistance = 2;

    /// <summary>
    /// True for as long as the player is triggering this action
    /// </summary>
    bool isAttracting = false;

    /// <summary>
    /// True when the player is waiting for object to be "attached"
    /// </summary>
    [SerializeField]
    bool waitForAttachables = false;

    /// <summary>
    /// True when the player was waiting for attacheables and they
    /// are all attached
    /// </summary>
    [SerializeField]
    bool doneWaitingForAttachables = false;

    /// <summary>
    /// The last position the player was when they invoked the "attraction" action
    /// This helps prevent the attraction logic from happening more than onces per
    /// tile the player stands on and allows them to move
    /// </summary>
    [SerializeField]
    Vector3 attractedFromPosition = new Vector3(-1, -1f, -1f);

    /// <summary>
    /// A list of objects attracted to the player
    /// </summary>
    List<IAttractable> attractables = new List<IAttractable>();

    /// <summary>
    /// A list of objects the player is pending to have attached to them
    /// </summary>
    List<IAttractable> attractablesPending = new List<IAttractable>();

    /// <summary>
    /// True when the player has not reached their destination
    /// </summary>
    [SerializeField]
    bool isMoving = false;
    
    /// <summary>
    /// True when the player is allowed 
    /// </summary>
    bool canMove = true;

    /// <summary>
    /// The layer to look for when casting the ray during the attract
    /// </summary>
    [SerializeField]
    LayerMask attractableLayer;

    /// <summary>
    /// How high from the pivot point to calculate the raycast from
    /// </summary>
    private float raycastHeight = 0.25f;

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
        // Can perform actions while moving
        if(!this.isMoving) {
            this.SetPlayerAction();
        }
        
        this.SetInputVector();

        // Disable movement while attracting action is happenig
        if(this.canMove) {
            this.Move();
        }        
    }

    /// <summary>
    /// Determines whether the player wants to:
    ///     - Start attracting 
    ///     - Cancel attract
    ///     - Repel objects
    /// </summary>
    void SetPlayerAction()
    {
        // Default to false
        this.canMove = false;
        bool objectsPending = this.attractablesPending.Count > 0;
        this.isAttracting = Input.GetButton("Fire1");

        // Check if we can attrack
        if(this.isAttracting) {
            this.Attract();

        // There are objects still being attached to the player
        // But player has opted to stop this
        } else if(!this.isAttracting && objectsPending) {
            this.CancelAttraction();
        
        // Push any objects away
        } else if(!this.isAttracting) {
            this.Repel();
            // Re-evaluate because we may have some now
            objectsPending = this.attractablesPending.Count > 0;
        }

        // No longer waiting on anything
        // Can move even if the is Attracting is enabled
        if(!objectsPending) {
            this.canMove = true;
        }
    }

    /// <summary>
    /// Casts a ray in all direction to find new objects within range to 
    /// attract them to the player 
    /// </summary>
    void Attract()
    {

        // We've calculated from this position so prevent doing so again
        this.attractedFromPosition = this.transform.position;

        // The directions we will fire the raycast in attempts to 
        // catch something that is attractable
        List<Vector3> directions = new List<Vector3>() {
            Vector3.forward,
            Vector3.left,
            Vector3.back,
            Vector3.right,
        };

        // where the ray will start from
        Vector3 origin = new Vector3(this.transform.position.x, this.raycastHeight, this.transform.position.z);

        foreach(Vector3 direction in directions) {
            // Show a line in the editor to see when we are calculating for attraction
            Debug.DrawLine(origin, this.transform.position + direction * this.attractionDistance);

            Ray ray = new Ray(origin, direction);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, this.attractionDistance, this.attractableLayer)) {

                IAttractable attractable = hitInfo.collider.GetComponent<IAttractable>();
                if(attractable != null) {
                    // Found a new attractable item
                    bool isNew = !this.attractables.Contains(attractable) && !this.attractablesPending.Contains(attractable);
                    if(isNew) {
                        this.attractablesPending.Add(attractable);
                        attractable.Attract(this);
                    }
                }
            }
        } // foreach
    }

    /// <summary>
    /// Called when the had invoke objects to be attracted but cancelled
    /// before they point of "no return" 
    /// </summary>
    void CancelAttraction()
    {
        foreach(IAttractable attractable in this.attractablesPending) {
            attractable.CancelAttract();
        }
    }

    /// <summary>
    /// Pushes all attached objects away
    /// </summary>
    void Repel()
    {        
        foreach(IAttractable attractable in this.attractables) {
            this.attractablesPending.Add(attractable);
            attractable.Repel(this);
        }
    }

    /// <summary>
    /// Stores the player's movement input as well as their action input
    /// </summary>
    void SetInputVector()
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
            this.isMoving = false;
            this.transform.position = this.destination;

            // Snap the childs too
            foreach(IAttractable attractable in this.attractables) {
                Transform goTransform = attractable.ObjectTransform;
                goTransform.position = attractable.FollowDestination;
            }
            
            // Where the player could potentially move
            Vector3 targetDestination = this.transform.position + this.inputVector;

            // Tile is available
            if(this.levelController.IsPositionAvailable(targetDestination)) {
                this.destination = targetDestination;

                // Update the children's destination
                foreach(IAttractable attractable in this.attractables) {
                    Transform goTransform = attractable.ObjectTransform;
                    attractable.FollowDestination = goTransform.position + this.inputVector;
                }
            }

        // Still Moving
        } else {
            this.isMoving = true;
            Vector3 targetDestination = Vector3.Lerp(this.transform.position, this.destination, this.speed * Time.deltaTime);
            this.transform.position = targetDestination;

            // Move all attached objects
            foreach(IAttractable attractable in this.attractables) {
                Transform goTransform = attractable.ObjectTransform;
                targetDestination = Vector3.Lerp(goTransform.position, attractable.FollowDestination, this.speed * Time.deltaTime);
                goTransform.position = targetDestination;
            }
        }
    }

    /// <summary>
    /// Invoked by an attractable that has finished attaching itself to the player
    /// </summary>
    /// <param name="attractable"></param>
    public void Attach(IAttractable attractable)
    {
        this.attractablesPending.Remove(attractable);
        this.attractables.Add(attractable);
    }

    /// <summary>
    /// Detaches the object 
    /// Typically invoked after the player has cancelled the attracting action
    /// or perhaps the object is no longer available
    /// </summary>
    /// <param name="attractable"></param>
    public void Detach(IAttractable attractable)
    {
        this.attractablesPending.Remove(attractable);
        this.attractables.Remove(attractable);
    }
}
