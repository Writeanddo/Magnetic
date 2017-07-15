using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for trigger enabled/disabled states
/// </summary>
public interface IEnableable
{
    /// <summary>
    /// Triggers the enabled state 
    /// </summary>
    void IsWalkable();

    /// <summary>
    /// Triggers the disabled state
    /// </summary>
    void IsAvailable();
}
