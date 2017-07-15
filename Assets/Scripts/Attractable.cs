using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that an IMagnetic can interact with by attracting or 
/// repelling it
/// </summary>
[RequireComponent(typeof(Animator))]
public class Attractable : MonoBehaviour, IAttractable
{
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
        get { return this.followDestination; }
        set { this.followDestination = value; }
    }


    /// <summary>
    /// Initialize
    /// </summary>
    void Start ()
    {
        this.animator = GetComponent<Animator>();
        this.followDestination = this.lastPosition = this.destination = this.transform.position;
	}

    /// <summary>
    /// Saves the ivoker
    /// Sets the destination to be one tile adjacent to the invoker
    /// Initated the animation that shows this will be attracted
    /// </summary>
    /// <param name="invoker"></param>
    public void Attract(IMagnetic invoker)
    {
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
            x += this.transform.position.x < x ? -1 : 1;
        } else {
            z += this.transform.position.z < z ? -1 : 1;
        }

        this.followDestination = this.destination = new Vector3(x, 0f, z);
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

    public void Repel(IMagnetic invoker)
    {
        throw new NotImplementedException();
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
            this.transform.position = this.followDestination = this.destination = this.lastPosition;
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

        // Done moving which mean we are "attached"
        this.transform.position = this.destination;
        this.invoker.Attach(this);
    }
}
