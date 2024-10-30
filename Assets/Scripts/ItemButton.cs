using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the behavior of item buttons in the game interface.
/// </summary>
public class ItemButton : MonoBehaviour {

	/// <summary>
    /// Reference to the UI Image component representing the button.
    /// </summary>
    public Image buttonImage;

	/// <summary>
    /// Reference to the UI Text component displaying the item amount.
    /// </summary>
    public Text amountText;

	/// <summary>
    /// Value associated with the button (e.g., index of the item).
    /// </summary>
    public int buttonValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
    /// Handles the button press action.
    /// </summary>
    public void Press()
    {
        if (GameMenu.instance.theMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }

        if(Shop.instance.shopMenu.activeInHierarchy)
        {
            if(Shop.instance.buyMenu.activeInHierarchy)
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }

            if(Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }
    }
}
