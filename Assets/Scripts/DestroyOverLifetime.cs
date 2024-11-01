using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the game object after a specified lifetime.
/// </summary>
public class DestroyOverLifetime : MonoBehaviour {

	/// <value>
    /// Lifetime of the game object before it is destroyed.
    /// </value>
    public float lifetime;

	/// <summary>
    /// Initializes the DestroyOverLifetime component.
    /// </summary>
	void Start () {
		
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// Destroys the game object after the specified lifetime.
    /// </summary>
	void Update () {
		// Destroy the game object after the specified lifetime
        Destroy(gameObject, lifetime);
	}
}
