using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any object that has the ability to influence IAttractable inherits this
/// </summary>
public interface IMagnetic
{
    /// <summary>
    /// Triggered by the IAttractable to notify this IMagnetic object
    /// that another has been attached to it
    /// </summary>
    /// <param name="attractable"></param>
    void Attach(IAttractable attractable);

    /// <summary>
    /// Notifies the invoker the object is no longer attached to it
    /// </summary>
    /// <param name="attractable"></param>
    void Detach(IAttractable attractable);

    /// <summary>
    /// The associated game object
    /// </summary>
    /// <returns></returns>
    GameObject gameObject { get; }

    /// <summary>
    /// How many tiles can this invoker repel an object
    /// </summary>
    int RepelTileDistance { get; }

    /// <summary>
    /// Allows the attractable items to tell the invoker to play
    /// a repelling sound at will
    /// </summary>
    void PlayRepelSound();
}
