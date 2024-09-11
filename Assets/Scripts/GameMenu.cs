using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    // UI elements
    public GameObject theMenu; // Main game menu UI object
    public GameObject[] windows; // Array of different windows in the menu (inventory, stats, etc.)

    // Reference to player stats
    private CharStats[] playerStats;

    // Arrays for displaying player information (name, HP, MP, etc.)
    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider; // Sliders to show experience progress
    public Image[] charImage; // Character images
    public GameObject[] charStatHolder; // Holds the UI elements for character stats

    // Status window elements
    public GameObject[] statusButtons; // Buttons for selecting a character in the status menu

    // Text elements to display character details in the status window
    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
    public Image statusImage; // Displays the character's image

    // Inventory-related variables
    public ItemButton[] itemButtons; // Buttons for selecting items in the inventory
    public string selectedItem; // The name of the currently selected item
    public Item activeItem; // The actual item object that is selected
    public Text itemName, itemDescription, useButtonText; // Text for displaying item details

    // Menu for selecting a character when using an item
    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames; // Text elements to display character names for item usage

    public static GameMenu instance; // Singleton instance of the GameMenu class

    public string mainMenuName; // Name of the main menu scene to load on quit

    // Initialize the game menu
    void Start () {
        instance = this; // Set this instance as the singleton instance
    }
    
    // Update is called once per frame
    void Update () {
        // Toggle the game menu when the 'Fire2' button is pressed (usually mapped to right-click or a specific key)
        if(Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy) // If the menu is currently open
            {
                CloseMenu(); // Close the menu
            } else // If the menu is currently closed
            {
                theMenu.SetActive(true); // Open the menu
                UpdateMainStats(); // Update character stats in the menu
                GameManager.instance.gameMenuOpen = true; // Notify GameManager that the menu is open
            }

            AudioManager.instance.PlaySFX(5); // Play a sound effect when the menu is toggled
        }
    }

    // Update the main stats in the menu (for all active characters)
    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats; // Get the current player stats
        int nonActiveChar = 0; // Track how many characters are not active

        // Loop through each character and update their stats
        for(int i = 0; i < playerStats.Length; i++)
        {
            // If the character is active and not "Tim", update their stats
            if(playerStats[i].gameObject.activeInHierarchy && playerStats[i].charName != "Tim")
            {
                // Show the character's stats in the menu
                charStatHolder[i-nonActiveChar].SetActive(true);
                nameText[i-nonActiveChar].text = playerStats[i].charName;
                hpText[i - nonActiveChar].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i - nonActiveChar].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i - nonActiveChar].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i - nonActiveChar].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i - nonActiveChar].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i - nonActiveChar].value = playerStats[i].currentEXP;
                charImage[i - nonActiveChar].sprite = playerStats[i].charIamge; // Update character image
            } else
            {
                charStatHolder[i].SetActive(false); // Hide the character's stats if not active
                nonActiveChar++; // Increment the inactive character count
            }
        }
    }

    // Toggle different menu windows
    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats(); // Update stats before toggling windows

        // Loop through all windows and toggle the selected one
        for(int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy); // Toggle the selected window
            } else
            {
                windows[i].SetActive(false); // Close other windows
            }
        }

        itemCharChoiceMenu.SetActive(false); // Close the item character choice menu if open
    }

    // Close the entire game menu
    public void CloseMenu()
    {
        // Close all open windows
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false); // Close the main menu
        GameManager.instance.gameMenuOpen = false; // Notify GameManager that the menu is closed
        itemCharChoiceMenu.SetActive(false); // Close the item character choice menu
    }

    // Open the status window and display character information
    public void OpenStatus()
    {
        UpdateMainStats(); // Update stats

        // List of indices for active buttons
        List<int> activeButtons = new List<int>();

        // Loop through status buttons and display info for active characters
        for(int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy && playerStats[i].charName != "Tim"); // Show active characters
            if (statusButtons[i].activeInHierarchy)
            {
                activeButtons.Add(i); // Add active character index to the list
            }
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName; // Set character name on the button
        }
        StatusChar(activeButtons[0]); // Display the first active character's stats by default
    }

    // Display the selected character's stats in the status window
    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defence.ToString();
        if(playerStats[selected].equippedWpn != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWpn;
        }
        statusWpnPwr.text = playerStats[selected].wpnPwr.ToString();
        if (playerStats[selected].equippedArmr != "")
        {
            statusArmrEqpd.text = playerStats[selected].equippedArmr;
        }
        statusArmrPwr.text = playerStats[selected].armrPwr.ToString();
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statusImage.sprite = playerStats[selected].charIamge; // Update the character's image
    }

    // Show the items in the inventory
    public void ShowItems()
    {
        GameManager.instance.SortItems(); // Sort items before displaying

        // Loop through each item button and display the corresponding item
        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true); // Show item image
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite; // Set item sprite
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString(); // Display item count
            } else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false); // Hide button if no item
                itemButtons[i].amountText.text = "";
            }
        }
    }

    // Select an item from the inventory
    public void SelectItem(Item newItem)
    {
        activeItem = newItem; // Set the active item

        // Update the button text based on the item type
        if(activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        if(activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName; // Display item name
        itemDescription.text = activeItem.description; // Display item description
    }

    // Discard the selected item
    public void DiscardItem()
    {
        // If an item is selected, discard it
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
            ShowItems(); // Update inventory display
        }
    }

    // Quit the game and return to the main menu
    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}
