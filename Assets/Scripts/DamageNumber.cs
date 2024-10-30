using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Displays damage numbers above characters in the game.
/// </summary>
public class DamageNumber : MonoBehaviour {

	/// <value>
    /// Reference to the Text component that displays the damage number.
    /// </value>
    public Text damageText;

	/// <value>
    /// Lifetime of the damage number before it disappears.
    /// </value>
    public float lifetime = 1f;

	/// <value>
    /// Speed at which the damage number moves across the screen.
    /// </value>
    public float moveSpeed = 1f;

	/// <value>
    /// Jitter factor applied to the initial position of the damage number.
    /// </value>
    public float placementJitter = .5f;

	/// <summary>
    /// Initializes the DamageNumber component.
    /// </summary>
	void Start () {
		
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// Destroys the game object after the specified lifetime and moves it across the screen.
    /// </summary>
	void Update () {
		// Destroy the game object after the specified lifetime
        Destroy(gameObject, lifetime);

		// Move the damage number across the screen
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
	}

	/// <summary>
    /// Sets the damage amount to be displayed and positions it randomly.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to display.</param>
    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
