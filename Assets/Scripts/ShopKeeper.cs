using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour {

    // Flag indicating if the shop menu can be opened
    private bool canOpen;

    // Array of items available for sale, with a maximum of 40 items
    public string[] ItemsForSale = new string[40];

    // Use this for initialization
    void Start () {
        // No initialization needed for this class
    }
    
    // Update is called once per frame
    void Update () {
        // Check if the shop menu can be opened
        if(canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            // Set the items for sale in the shop
            Shop.instance.itemsForSale = ItemsForSale;

            // Open the shop menu
            Shop.instance.OpenShop();
        }
    }

    // Triggered when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // Set canOpen to true when the player is within range
            canOpen = true;
        }
    }

    // Triggered when another collider exits the trigger collider attached to this GameObject
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Set canOpen to false when the player leaves the range
            canOpen = false;
        }
    }
}
