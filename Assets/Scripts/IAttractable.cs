using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All objects that can be "magnetized" unto another will inherit from this
/// </summary>
public interface IAttractable
{
    /// <summary>
    /// Begins the processing of attaching to the invoker
    /// </summary>
    void Attract(IMagnetic invoker);

    /// <summary>
    /// Invoke already has other objects attached to it
    /// This causes this object to begin moving towards the invoker
    /// ignoring any warm-up
    /// </summary>
    void InstantAttract(IMagnetic invoker);

    /// <summary>
    /// If canceable, stops the attract process
    /// </summary>
    void CancelAttract();

    /// <summary>
    /// Begins the processes of being repeled from the invoker
    /// </summary>
    void Repel(IMagnetic invoker);

    /// <summary>
    /// Returns the associated transform component
    /// </summary>
    Transform ObjectTransform { get; }
    
    /// <summary>
    /// Sets the destination for this object to "follow" the invoker
    /// </summary>
    Vector3 FollowDestination { set; get; }

    /// <summary>
    /// True when the object is attached to the invoker
    /// </summary>
    bool IsAttached { get; }
    bool IsFalling { get;}
}
