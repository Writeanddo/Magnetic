using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : Tile
{
    /// <summary>
    /// Where the tile starts
    /// This is where it will move back to when disabled
    /// </summary>
    Vector3 origin;

    /// <summary>
    /// Where the tile will move to when enabled
    /// </summary>
    [SerializeField]
    Vector3 destination;

    /// <summary>
    /// How quickly the tile will move to the destination
    /// </summary>
    [SerializeField]
    float speed;

    /// <summary>
    /// How close to the destination before snapping into place
    /// </summary>
    [SerializeField]
    float distancePadding = 0.3f;

    /// <summary>
    /// Initialize class
    /// </summary>
    void Awake()
    {
        // Set the origin to the current position
        this.origin = this.transform.position;
    }

    /// <summary>
    /// Stops the coroutine from running if the tile is enabled
    /// </summary>
    void Update()
    {
        if(this.isEnabled) {
            StopCoroutine("ReturnToOrigin");
        }
    }

    /// <summary>
    /// Slowly moves to the destination until it gets there
    /// </summary>
    public override void Enable()
    {
        this.isEnabled = true;
        float distanceToDestination = Vector3.Distance(this.destination, this.transform.position);

        // Arrived
        if(distanceToDestination <= this.distancePadding) {
            this.transform.position = this.destination;
            return;
        }

        // Still moving
        Vector3 targetDestination = Vector3.Lerp(this.transform.position, this.destination, this.speed * Time.deltaTime);
        this.transform.position = targetDestination;
    }

    /// <summary>
    /// Forces the tile to move back to its origin
    /// </summary>
    public override void Disable()
    {
        if(this.isEnabled) {
            this.isEnabled = false;
            StartCoroutine("ReturnToOrigin");
        }
        
    }

    /// <summary>
    /// Returns the tile back to the origin
    /// </summary>
    /// <returns></returns>
    IEnumerator ReturnToOrigin()
    {
        float distanceToDestination = Vector3.Distance(this.destination, this.transform.position);
        do {
            yield return new WaitForEndOfFrame();

            Vector3 targetDestination = Vector3.Lerp(this.transform.position, this.origin, this.speed * Time.deltaTime);
            this.transform.position = targetDestination;

            distanceToDestination = Vector3.Distance(this.destination, this.transform.position);
        } while(distanceToDestination > this.distancePadding);

        // Arrived
        this.transform.position = this.origin;
    }

    
}
