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
    public delegate void SwitchActivatedDelegate(SwitchTile tile);
    public delegate void SwitchDeactivatedDelegate(SwitchTile tile);

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

    /// <summary>
    /// Color to display when the switch is active
    /// </summary>
    [SerializeField]
    Material activedColor;

    /// <summary>
    /// Color to display when the switch is not active
    /// </summary>
    [SerializeField]
    Material deacivatedColor;

    /// <summary>
    /// A reference to the child renderer so that we can change the material color
    /// </summary>
    Renderer childRenderer;

    // Use this for initialization
    void Start ()
    {
		this.type = Type.Switch;
        this.childRenderer = this.transform.GetChild(0).GetComponent<Renderer>();
        this.childRenderer.material = this.deacivatedColor;
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
                this.childRenderer.material = this.activedColor;

                if(this.SwitchActivatedEvent != null) {
                    this.SwitchActivatedEvent(this);
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
            this.childRenderer.material = this.deacivatedColor;

            if(this.SwitchDeactivatedEvent != null) {
                this.SwitchDeactivatedEvent(this);
            }
        }
    }
}
