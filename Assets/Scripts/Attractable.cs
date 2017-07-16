﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that an IMagnetic can interact with by attracting or 
/// repelling it
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Attractable : MonoBehaviour, IAttractable
{
    /// <summary>
    /// A reference to the levelController
    /// </summary>
    LevelController levelController;

    /// <summary>
    /// The object that triggered an attract or repel on this object
    /// </summary>
    IMagnetic invoker;

    /// <summary>
    /// The last position this item was before the invoker cancelled the attract action
    /// </summary>
    Vector3 lastPosition;

    /// <summary>
    /// Where to move to when being attracted
    /// </summary>
    [SerializeField]
    Vector3 destination;

    /// <summary>
    /// True while the object is being attracted
    /// </summary>
    bool isBeingAttracted = false;

    /// <summary>
    /// True when the animation to simulate attraction is completed
    /// </summary>
    bool isAnimationDone = true;

    /// <summary>
    /// A reference to the animator component
    /// </summary>
    Animator animator;

    /// <summary>
    /// How fast the object moves when attaching to the invoker
    /// </summary>
    [SerializeField]
    float speed = 5f;

    /// <summary>
    /// A reference to the rigid body to which we will be
    /// enabling and disabling gravity as needed
    /// </summary>
    Rigidbody rigidBody;

    /// <summary>
    /// Returns the transform for this object
    /// </summary>
    public Transform ObjectTransform
    {
        get
        {
            return this.transform;
        }
    }

    /// <summary>
    /// Where to move to when attached to an object
    /// This is used by the invoker to tell this object how to move
    /// </summary>
    Vector3 followDestination;
    public Vector3 FollowDestination
    {
        get { return this.positionedAt; }
        set { this.positionedAt = value; }
    }

    /// <summary>
    /// True when the object is being attracted
    /// </summary>
    public bool IsAttached
    {
        get
        {
            return this.isBeingAttracted;
        }
    }

    /// <summary>
    /// Stores the direction in relationship to the invoker
    /// that this object is positioned at
    /// </summary>
    Vector3 positionedAt;

    /// <summary>
    /// Initialize
    /// </summary>
    void Start ()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        this.levelController = FindObjectOfType<LevelController>();
        this.lastPosition = this.destination = this.transform.position;

        // Make sure gravity is disabled
        this.rigidBody.useGravity = false;
	}

    /// <summary>
    /// Saves the ivoker
    /// Sets the destination to be one tile adjacent to the invoker
    /// Initated the animation that shows this will be attracted
    /// Saves the position in relation to the invoker at this moment too
    /// </summary>
    /// <param name="invoker"></param>
    public void Attract(IMagnetic invoker)
    {
        // Make sure gravity is disabled
        this.rigidBody.useGravity = false;

        this.isBeingAttracted = true;
        this.isAnimationDone = false;
        this.invoker = invoker;

        // The position is based on whether the invoker is on the same column or row
        Vector3 invokerPosition = invoker.gameObject.transform.position;
        bool sameRow = invokerPosition.z == this.transform.position.z;

        // What the new position will be
        float x = invokerPosition.x;
        float z = invokerPosition.z;
        
        if(sameRow) {
            // Invoker is on the left
            if(this.transform.position.x < x) {
                x += -1;
                this.positionedAt = Vector3.left;
            } else {
                x += 1;
                this.positionedAt = Vector3.right;
            }
            
        } else {
            // Invoker is behind
            if(this.transform.position.z < z) {
                z += -1;
                this.positionedAt = Vector3.back;
            } else {
                z += 1;
                this.positionedAt = Vector3.forward;
            }
        }

        this.destination = new Vector3(x, 0f, z);
        this.animator.SetTrigger("Spin");
    }

    /// <summary>
    /// Object has not fully attached itself 
    /// Invoker cancelled the request
    /// </summary>
    public void CancelAttract()
    {
        this.isBeingAttracted = false;
    }

    /// <summary>
    /// Invoker is pushing this object away
    /// calculate where it should go and begin movement
    /// </summary>
    /// <param name="invoker"></param>
    public void Repel(IMagnetic invoker)
    {
        this.isBeingAttracted = false;
        Vector3 destination = this.transform.position;

        // Tile can be null or walkable
        // If it is not available then we have a destination
        for(int i = 1; i <= invoker.RepelTileDistance; i++) {
            Vector3 targetDestination = this.transform.position + this.positionedAt * i;

            if(this.levelController.IsTileAtPositionAvailable(targetDestination)) {
                destination = targetDestination;
            } else {
                break;
            }
        }

        this.destination = destination;
        StartCoroutine("MoveToDestination");
    }
   
    /// <summary>
    /// Invoked by the animator once the animation is done
    /// </summary>
    public void AnimationEnd()
    {
        this.isAnimationDone = true;

        if(this.isBeingAttracted) {
            StartCoroutine("MoveToDestination");
        } else {
            this.invoker.Detach(this);
            this.invoker = null;
            this.transform.position = this.destination = this.lastPosition;
        }
        
    }

    /// <summary>
    /// Moves towards the destination until reached
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToDestination()
    {
        // target not reached - move
        while( Vector3.Distance(this.destination, this.transform.position) > 0.1f ) {
            this.transform.position = Vector3.Lerp(this.transform.position, this.destination, this.speed * Time.deltaTime);
            yield return null;
        }

        // Snap to location
        this.lastPosition = this.transform.position = this.destination;

        // Object has been either attached or detached
        if(this.isBeingAttracted) {
            this.invoker.Attach(this);
        } else {
            this.invoker.Detach(this);
            this.rigidBody.useGravity = true;
        }
        
    }
}
