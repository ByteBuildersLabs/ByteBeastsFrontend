using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages the notification of battle events in the game.
/// </summary>
public class BattleNotification : MonoBehaviour {

	/// <value>
    /// Time duration for which the notification should be active.
    /// </value>
    public float awakeTime;

	/// <value>
    /// Counter to track the remaining time for the notification.
    /// </value>
    private float awakeCounter;

	/// <value>
    /// Reference to the Text component displaying the notification message.
    /// </value>
    public Text theText;

	// Use this for initialization

	/// <summary>
    /// Initializes the BattleNotification component.
    /// </summary>

	void Start () {
		
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Handles the countdown and deactivation of the notification.
    /// </summary>
	void Update () {
		if(awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;
            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

	/// <summary>
    /// Activates the notification and starts the countdown.
    /// </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
        awakeCounter = awakeTime;
    }
}
