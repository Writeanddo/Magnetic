using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    /// <summary>
    /// A reference to the particle system,
    /// </summary>
    ParticleSystem particalSystem;

	/// <summary>
    /// Initialize
    /// </summary>
	void Start () {
        this.particalSystem = GetComponent<ParticleSystem>();
	}
	
	/// <summary>
    /// Wait until the partical effect is done and remove it
    /// </summary>
	void Update ()
    {
		if(this.particalSystem != null && !this.particalSystem.IsAlive()) {
            Destroy(this.gameObject);
        }
	}
}
