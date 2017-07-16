using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functioins as "key" to open the exit tiles
/// Requires that an object be standing on it to be "active"
/// </summary>
public class SwitchTile : Tile
{
    /// <summary>
    /// The different states the switch can be in
    /// </summary>
    enum State
    {
        Off,
        On
    }

    /// <summary>
    /// Which state this switch is in
    /// Defaults to off
    /// </summary>
    [SerializeField]
    State state;

    /// <summary>
    /// Delegates for enabling/disabling the switch
    /// </summary>
    /// <param name="value">Value of the object clicked</param>
    public delegate void SwitchActivatedDelegate();
    public delegate void SwitchDeactivatedDelegate();

    /// <summary>
    /// Events triggered when switch is enabled/disabled
    /// </summary>
    public event SwitchActivatedDelegate SwitchActivatedEvent;
    public event SwitchDeactivatedDelegate SwitchDeactivatedEvent;

    /// <summary>
    /// Sound to play when the switch is activated
    /// </summary>
    [SerializeField]
    AudioClip switchOnClip;

    /// <summary>
    /// Sound to play when the switch is deactivated 
    /// </summary>
    [SerializeField]
    AudioClip switchOffClip;

    // Use this for initialization
    void Start ()
    {
		this.type = Type.Switch;
	}

    /// <summary>
    /// True when there's nothing on the tile
    /// </summary>
    /// <returns></returns>
    public override bool IsWalkable() { return !this.hasObject; }
	
	/// <summary>
    /// Something is on this tile
    /// Let's activate the switch if it is not already
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        bool saveObject = false;
        this.hasObject = false;

        // If this is an attractable object then will ignore it if its being held
        IAttractable attractable = other.GetComponent<IAttractable>();

        // Player is on switch 
        if(other.tag == "Player") {
            saveObject = true;

        // An attractable object is on top
        } else if(attractable != null && !attractable.IsAttached) {
            saveObject = true;
        }

        // Proceed with activating the switch
        if(saveObject) {                        
            if(this.objectOnTile != other.gameObject) {
                this.hasObject = true;
                this.objectOnTile = other.gameObject;
                this.PlaySound(this.switchOnClip);

                if(this.SwitchActivatedEvent != null) {
                    this.SwitchActivatedEvent();
                }
            }
        }
    }

    /// <summary>
    /// Something left this tile
    /// Turn it off
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == this.objectOnTile) {
            this.hasObject = false;
            this.objectOnTile = null;
            this.PlaySound(this.switchOffClip);

            if(this.SwitchDeactivatedEvent != null) {
                this.SwitchDeactivatedEvent();
            }
        }
    }
}
