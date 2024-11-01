using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for instantiating and managing the player object in the game.
/// </summary>
public class PlayerLoader : MonoBehaviour {

	/// <value>
    /// The prefab of the player object to instantiate.
    /// </value>
    public GameObject player;

	/// <summary>
    /// Called when the script is instantiated.
    /// Checks if the PlayerController instance exists and instantiates the player if not.
    /// </summary>
	void Start () {
		if(PlayerController.instance == null)
        {
            Instantiate(player);
        }
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// </summary>
	void Update () {
		
	}
}
