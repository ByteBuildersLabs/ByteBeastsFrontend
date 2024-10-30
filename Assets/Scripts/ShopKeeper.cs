using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the interaction between the player and the shopkeeper NPC.
/// </summary>
public class ShopKeeper : MonoBehaviour {

	/// <summary>
    /// Flag indicating whether the shopkeeper can interact with the player.
    /// </summary>
    private bool canOpen;

	/// <summary>
    /// Array of item names available for sale in the shop.
    /// </summary>
    public string[] ItemsForSale = new string[40];

	// Use this for initialization

	/// <summary>
    /// Called when the script is instantiated.
    /// Currently empty, but can be used for initial setup.
    /// </summary>
	void Start () {
		
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Opens the shop menu when certain conditions are met.
    /// </summary>
	void Update () {
		if(canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            Shop.instance.itemsForSale = ItemsForSale;

            Shop.instance.OpenShop();
        }
	}

	/// <summary>
    /// Called when the Collider2D overlaps with another object.
    /// Enables interaction with the player when they approach the shopkeeper.
    /// </summary>
    /// <param name="other">The Collider2D component that triggered the event.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }

	/// <summary>
    /// Called when the Collider2D stops overlapping with another object.
    /// Disables interaction with the player when they move away from the shopkeeper.
    /// </summary>
    /// <param name="other">The Collider2D component that stopped triggering the event.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
