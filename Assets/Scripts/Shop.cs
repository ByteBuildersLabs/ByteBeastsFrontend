using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the shop functionality in the game, including buying and selling items.
/// </summary>
public class Shop : MonoBehaviour {

	/// <summary>
    /// Singleton instance of the Shop manager.
    /// </summary>
    public static Shop instance;

	/// <summary>
    /// UI elements for the shop interface.
    /// </summary>
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

	/// <summary>
    /// Reference to the text displaying the player's gold amount.
    /// </summary>
    public Text goldText;

	/// <summary>
    /// Array of item names available for sale in the shop.
    /// </summary>
    public string[] itemsForSale;

	/// <summary>
    /// Arrays of buttons for buying and selling items.
    /// </summary>
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

	/// <summary>
    /// Currently selected item for purchase or sale.
    /// </summary>
    public Item selectedItem;

	/// <summary>
    /// UI elements for displaying item details during selection.
    /// </summary>
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

	/// <summary>
    /// Called when the script is instantiated.
    /// Initializes the singleton instance.
    /// </summary>
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Checks for opening the shop with the K key.
    /// </summary>
	void Update () {
		if(Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        {
            OpenShop();
        }
	}

	/// <summary>
    /// Opens the shop interface.
    /// Activates the shop menu and buy menu, sets the game state to active shop mode.
    /// </summary>
    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

	/// <summary>
    /// Closes the shop interface.
    /// Deactivates the shop menu and sets the game state to inactive shop mode.
    /// </summary>
    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
    }

	/// <summary>
    /// Opens the buy menu, displaying available items for purchase.
    /// </summary>
    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale[i] != "")
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            }
            else
            {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

	/// <summary>
    /// Selects an item for purchase, updating the UI display.
    /// </summary>
    /// <param name="buyItem">The item to select for purchase.</param>
    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();

        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        ShowSellItems();
        
    }

	/// <summary>
    /// Displays the items available for sale in the sell menu.
    /// Updates the UI buttons with item details.
    /// </summary>
    private void ShowSellItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

	/// <summary>
    /// Selects an item for purchase, updating the UI display.
    /// </summary>
    /// <param name="buyItem">The item to select for purchase.</param>
    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + "g";
    }

	/// <summary>
    /// Selects an item for sale, updating the UI display.
    /// </summary>
    /// <param name="sellItem">The item to select for sale.</param>
    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + "g";
    }

	/// <summary>
    /// Purchases the selected item if the player has enough gold.
    /// </summary>
    public void BuyItem()
    {
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;

                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

	/// <summary>
    /// Sells the selected item, adding half its value to the player's gold.
    /// </summary>
    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);

            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

        ShowSellItems();
    }
}
