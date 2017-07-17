using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple camera follow that tracks a target keeping them in the center
/// </summary>
public class SimpleCameraFollow : MonoBehaviour
{
    /// <summary>
    /// The target to follow
    /// </summary>
	[SerializeField]
    Transform target;

    /// <summary>
    /// How fast to track
    /// </summary>
    [SerializeField]
    float followSpeed = 5f;

    /// <summary>
    /// LateUpdate is preferred to give the target time to move first
    /// and allowing the camera to "trail" behind
    /// </summary>
    void LateUpdate()
    {
        Vector3 targetPosition = this.target.position;
        this.transform.position = Vector3.Lerp(this.transform.position, 
                                               targetPosition, 
                                               this.followSpeed * Time.deltaTime);
    }
}
