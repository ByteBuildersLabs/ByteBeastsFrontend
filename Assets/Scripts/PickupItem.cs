using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the pickup of items by players in the game.
/// </summary>
public class PickupItem : MonoBehaviour {

	/// <summary>
    /// Flag indicating whether the item can be picked up by the player.
    /// </summary>
    private bool canPickup;

	// Use this for initialization
	void Start () {
		
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// Checks for input to pick up the item if it's within range of the player.
    /// </summary>
	void Update () {
		if(canPickup && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
        {
            GameManager.instance.AddItem(GetComponent<Item>().itemName);
            Destroy(gameObject);
        }
	}

	/// <summary>
    /// Trigger event called when another collider enters the trigger bounds.
    /// Enables the pickup flag if the entering object is tagged as "Player".
    /// </summary>
    /// <param name="other">The Collider2D that entered the trigger bounds.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canPickup = true;
        }
    }

	/// <summary>
    /// Trigger event called when another collider exits the trigger bounds.
    /// Disables the pickup flag if the exiting object is tagged as "Player".
    /// </summary>
    /// <param name="other">The Collider2D that exited the trigger bounds.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPickup = false;
        }
    }
}
