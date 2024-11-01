using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the entrance to an area in the game.
/// </summary>
public class AreaEntrance : MonoBehaviour {
	/// <value>
    /// The name of the transition to use when entering this area.
    /// </value>
    public string transitionName;

	// Use this for initialization
	// test

	/// <summary>
    /// Initializes the AreaEntrance component.
    /// </summary>
	void Start () {
		// Check if the current area transition matches the one set for this entrance
		if(transitionName == PlayerController.instance.areaTransitionName)
        {
			// Position the player at the entrance position
            PlayerController.instance.transform.position = transform.position;
        }

		// Fade out the black screen
        UIFade.instance.FadeFromBlack();

		// Reset the fading-between-areas flag
        GameManager.instance.fadingBetweenAreas = false;
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// </summary>
	void Update () {
		
	}
}
