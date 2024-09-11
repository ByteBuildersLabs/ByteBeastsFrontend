using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // Singleton instance of the Shop
    public static Shop instance;

    // References to the shop menu, buy menu, and sell menu GameObjects
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    // Text component to display the player's gold amount
    public Text goldText;

    // Array of item names available for sale
    public string[] itemsForSale;

    // Arrays of buttons for buying and selling items
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    // Reference to the currently selected item
    public Item selectedItem;

    // Text components for displaying item details in buy and sell menus
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    // Initialization
    void Start()
    {
        // Set the singleton instance
        instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Open the shop menu when the 'K' key is pressed and the shop menu is not currently active
        if (Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        {
            OpenShop();
        }
    }

    // Open the shop menu and set its state to active
    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        // Set shopActive to true in GameManager
        GameManager.instance.shopActive = true;

        // Update the gold text to show the current gold amount
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    // Close the shop menu and set its state to inactive
    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
    }

    // Open the buy menu and set its state to active
    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press(); // Highlight the first buy button

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        // Update the buy item buttons with available items for sale
        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale[i] != "")
            {
                // Set button image and hide amount text if item is available
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            }
            else
            {
                // Hide button image and amount text if item is not available
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    // Open the sell menu and set its state to active
    public void OpenSellMenu()
    {
        sellItemButtons[0].Press(); // Highlight the first sell button

        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        // Update the sell item buttons with the player's items
        ShowSellItems();
    }

    // Update the sell item buttons with items the player owns
    private void ShowSellItems()
    {
        // Sort items before displaying
        GameManager.instance.SortItems();
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                // Set button image and amount text for each item the player holds
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                // Hide button image and amount text if no item is held
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

    // Select an item to buy and display its details
    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + "g";
    }

    // Select an item to sell and display its details
    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + "g";
    }

    // Buy the selected item if the player has enough gold
    public void BuyItem()
    {
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;

                // Add the item to the player's inventory
                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }

        // Update the gold text
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    // Sell the selected item and update the player's gold
    public void SellItem()
    {
        if (selectedItem != null)
        {
            // Add half of the item's value to the player's gold
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);

            // Remove the item from the player's inventory
            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        // Update the gold text and refresh the sell items
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
        ShowSellItems();
    }
}
