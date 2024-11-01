using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the visual and audio effects of an attack in the game.
/// </summary>
public class AttackEffect : MonoBehaviour {

	/// <value>
    /// The length of time the effect should last before being destroyed.
    /// </value>
    public float effectLength;

	/// <value>
    /// The ID of the sound effect to play when the attack occurs.
    /// </value>
    public int soundEffect;

	// Use this for initialization

	/// <summary>
    /// Initializes the AttackEffect component.
    /// Plays the specified sound effect when the script starts.
    /// </summary>
	void Start () {
		// Play the sound effect associated with this attack
        AudioManager.instance.PlaySFX(soundEffect);
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Destroys the game object after the specified effect length has elapsed.
    /// </summary>
	void Update () {
		// Automatically destroy the game object after the effect duration
        Destroy(gameObject, effectLength);
	}
}
