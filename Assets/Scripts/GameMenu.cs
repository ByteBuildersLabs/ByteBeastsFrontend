using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game menu functionality, including character stats display, item management, and menu navigation.
/// </summary>
public class GameMenu : MonoBehaviour {

	/// <summary>
    /// Reference to the main menu UI object.
    /// </summary>
    public GameObject theMenu;

	/// <summary>
    /// Array of UI windows in the menu.
    /// </summary>
    public GameObject[] windows;

	/// <summary>
    /// Array of player character stats.
    /// </summary>
    private CharStats[] playerStats;

	 /// <summary>
    /// References to various UI elements for displaying character information.
    /// </summary>
    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;
    public GameObject[] charStatHolder;

	/// <summary>
    /// References to status buttons for each character.
    /// </summary>
    public GameObject[] statusButtons;

	 /// <summary>
    /// References to individual status display elements.
    /// </summary>
    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqpd, statusArmrPwr, statusExp;
    public Image statusImage;

	/// <summary>
    /// References to item buttons.
    /// </summary>
    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDescription, useButtonText;

	/// <summary>
    /// Reference to the item character choice menu.
    /// </summary>
    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

	/// <summary>
    /// Singleton instance of the GameMenu class.
    /// </summary>
    public static GameMenu instance;

	/// <summary>
    /// Name of the main menu scene.
    /// </summary>
    public string mainMenuName;

    /// <summary>
    /// Called when the script is instantiated.
    /// Initializes the singleton instance of GameMenu.
    /// </summary>
    void Awake () {
        if (instance == null) instance = this;
        else Destroy(gameObject);
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Handles toggling the menu open/close state and updating character stats.
    /// </summary>
	void Update () {
		if(Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;

                CloseMenu();
            } else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }

            AudioManager.instance.PlaySFX(5);
        }
	}

	/// <summary>
    /// Updates the main stats display for all active characters.
    /// </summary>
    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;
        int nonActiveChar = 0;
        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy && playerStats[i].charName != "Tim")
            {
                charStatHolder[i-nonActiveChar].SetActive(true);
                Debug.Log("Showing stats for: " + playerStats[i].charName);
                nameText[i-nonActiveChar].text = playerStats[i].charName;
                hpText[i - nonActiveChar].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i - nonActiveChar].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i - nonActiveChar].text = "Lvl: " + playerStats[i].playerLevel;
                expText[i - nonActiveChar].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i - nonActiveChar].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i - nonActiveChar].value = playerStats[i].currentEXP;
                charImage[i - nonActiveChar].sprite = playerStats[i].charIamge;
            } else
            {
                Debug.Log("Disabled Stats :" + nameText[i].text);
                charStatHolder[i].SetActive(false);
                nonActiveChar++;
            }
        }
    }

	/// <summary>
    /// Toggles visibility of a specific window in the menu.
    /// </summary>
    /// <param name="windowNumber">Index of the window to toggle.</param>
    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for(int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            } else
            {
                windows[i].SetActive(false);
            }
        }

        itemCharChoiceMenu.SetActive(false);
    }

	/// <summary>
    /// Closes the entire menu.
    /// </summary>
    public void CloseMenu()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        theMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;

        itemCharChoiceMenu.SetActive(false);
    }

	/// <summary>
    /// Opens the status display for active characters.
    /// </summary>
    public void OpenStatus()
    {
        UpdateMainStats();

        //update the information that is shown
        List<int> activeButtons = new List<int>();

        for(int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy && playerStats[i].charName != "Tim");
            if (statusButtons[i].activeInHierarchy)
            {
                activeButtons.Add(i);
            }
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
        StatusChar(activeButtons[0]);
    }

	/// <summary>
    /// Displays detailed status information for a selected character.
    /// </summary>
    /// <param name="selected">Index of the selected character.</param>
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
        statusImage.sprite = playerStats[selected].charIamge;

    }

	/// <summary>
    /// Shows items held by the player.
    /// </summary>
    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if(GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            } else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

	/// <summary>
    /// Selects an item for use or equipping.
    /// </summary>
    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if(activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if(activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

	/// <summary>
    /// Discards the currently selected item.
    /// </summary>
    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

	/// <summary>
    /// Opens the character selection menu for item usage.
    /// </summary>
    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for(int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
        }
    }

	/// <summary>
    /// Closes the character selection menu for item usage.
    /// </summary>
    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

	 /// <summary>
    /// Uses the selected item on a specific character.
    /// </summary>
    /// <param name="selectChar">Index of the character to apply the item to.</param>
    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }

	/// <summary>
    /// Saves the current game state.
    /// </summary>
    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

	/// <summary>
    /// Plays a button sound effect.
    /// </summary>
    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

	/// <summary>
    /// Quits the game to the main menu scene.
    /// Destroys various game objects and loads the main menu scene.
    /// </summary>
    public void QuitGame()
    {
        SceneManager.LoadSceneAsync(mainMenuName);

        Destroy(BattleManager.instance.gameObject);
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
