using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the actions of the player
/// </summary>
public class PlayerController : MonoBehaviour
{   
	void Update ()
    {
        if(Input.GetButton("Fire1")) {
            IEnableable[] targets = FindObjectsOfType<Tile>();

            foreach(IEnableable target in targets) {
                target.Enable();
            }
        } else {
            IEnableable[] targets = FindObjectsOfType<Tile>();

            foreach(IEnableable target in targets) {
                target.Disable();
            }
        }
	}
}
