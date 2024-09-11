using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    public Image buttonImage; // Image component of the button
    public Text amountText;   // Text component displaying the amount
    public int buttonValue;   // Index or identifier for the button

    // Called when the script instance is being loaded
    void Start () {
        // Initialization code (if any) goes here
    }

    // Called once per frame
    void Update () {
        // Update logic (if any) goes here
    }

    // Called when the button is pressed
    public void Press()
    {
        // Check if the GameMenu is active
        if (GameMenu.instance.theMenu.activeInHierarchy)
        {
            // Check if there is an item held for the current button value
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                // Select the item and show its details in the GameMenu
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }

        // Check if the Shop is active
        if(Shop.instance.shopMenu.activeInHierarchy)
        {
            // Check if the buy menu is active
            if(Shop.instance.buyMenu.activeInHierarchy)
            {
                // Select the item to buy and show its details
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }

            // Check if the sell menu is active
            if(Shop.instance.sellMenu.activeInHierarchy)
            {
                // Select the item to sell and show its details
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }
    }
}
