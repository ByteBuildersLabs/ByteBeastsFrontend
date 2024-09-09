using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    // Flag to indicate whether the item can be picked up
    private bool canPickup;

    // Use this for initialization
    void Start () {
        // Initialization logic here (currently empty)
    }
    
    // Update is called once per frame
    void Update () {
        // Check if the item can be picked up and the pickup button is pressed
        if(canPickup && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
        {
            // Add the item to the GameManager's inventory
            GameManager.instance.AddItem(GetComponent<Item>().itemName);

            // Destroy the item GameObject
            Destroy(gameObject);
        }
    }

    // Trigger detection when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger area
        if(other.tag == "Player")
        {
            // Set the flag to allow pickup
            canPickup = true;
        }
    }

    // Trigger detection when another collider exits the trigger collider attached to this GameObject
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the trigger area
        if (other.tag == "Player")
        {
            // Set the flag to disallow pickup
            canPickup = false;
        }
    }
}
