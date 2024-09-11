using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using bottlenoselabs.C2CS.Runtime;
using Dojo;
using Dojo.Starknet;
using dojo_bindings;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

// Fix to use Records in Unity ref. https://stackoverflow.com/a/73100830
using System.ComponentModel;
using dojo_examples;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}

public class GameManager : MonoBehaviour
{

    [SerializeField] WorldManager worldManager;

    [SerializeField] ChatManager chatManager;

    [SerializeField] WorldManagerData dojoConfig;

    [SerializeField] GameManagerData gameManagerData;

    public BurnerManager burnerManager;
    private Dictionary<FieldElement, string> spawnedAccounts = new();
    public Actions actions;

    public JsonRpcClient provider;
    public Account masterAccount;

    // Static instance for singleton pattern to ensure there's only one GameManager
    public static GameManager instance;

    // Array of character stats for players
    public CharStats[] playerStats;

    // Booleans to track different game states (menu, dialogue, fading between areas, shop, battle)
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;

    // Arrays for tracking items held, their quantities, and reference to item data
    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    // Player's gold amount
    public int currentGold;

    // Called when the game starts
    void Start()
    {
        // Set the singleton instance to this object
        instance = this;

        provider = new JsonRpcClient(dojoConfig.rpcUrl);
        masterAccount = new Account(provider, new SigningKey(gameManagerData.masterPrivateKey), new FieldElement(gameManagerData.masterAddress));
        burnerManager = new BurnerManager(provider, masterAccount);

        worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
        foreach (var entity in worldManager.Entities<Position>())
        {
            InitEntity(entity);
        }

        // Prevent the object from being destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Sort items in the inventory at the start of the game
        SortItems();
    }

    // Called every frame
    void Update()
    {
        // Disable player movement if any of these activities are active
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }

        // Debug: Add and remove items with keyboard inputs
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Iron Armor");
            AddItem("Blabla");

            RemoveItem("Health Potion");
            RemoveItem("Bleep");
        }

        // Debug: Save the game with the 'O' key
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        // Debug: Load the game with the 'P' key
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    // Returns details about a specific item by name
    public Item GetItemDetails(string itemToGrab)
    {
        // Loop through reference items to find a match
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i]; // Return item details if found
            }
        }
        return null; // Return null if item is not found
    }

    // Sorts the items in the player's inventory by moving empty slots to the end
    public void SortItems()
    {
        bool itemAfterSpace = true;

        // Continue sorting while there are items after empty slots
        while (itemAfterSpace)
        {
            itemAfterSpace = false;

            // Loop through the inventory and move items up if there are empty spaces
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    // Shift the next item into the current slot
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    // Shift the number of items as well
                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    // If a shift was made, continue the loop
                    if (itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    // Adds an item to the player's inventory
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        // Look for an empty space or a stack of the same item
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                foundSpace = true;
                break;
            }
        }

        // If space was found, check if the item exists in the reference items
        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    break;
                }
            }

            // If item exists, add it to the inventory
            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }

        // Update the game menu to show the new item
        GameMenu.instance.ShowItems();
    }

    // Removes an item from the player's inventory
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        // Find the item in the inventory
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;
                break;
            }
        }

        // If item was found, reduce its quantity or remove it completely
        if (foundItem)
        {
            numberOfItems[itemPosition]--;

            // Remove the item if its quantity reaches zero
            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }

            // Update the game menu to reflect the changes
            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    // Saves the game data to PlayerPrefs
    public void SaveData()
    {
        // Save the current scene and player position
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        // Save character stats for each player
        for (int i = 0; i < playerStats.Length; i++)
        {
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", playerStats[i].gameObject.activeInHierarchy ? 1 : 0);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
        }

        // Save inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }

    // Loads the game data from PlayerPrefs
    public void LoadData()
    {
        // Load player position
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        // Load character stats
        for (int i = 0; i < playerStats.Length; i++)
        {
            playerStats[i].gameObject.SetActive(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 1);
            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        // Load inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }

    private void InitEntity(GameObject entity)
    {
        // check if entity has position component
        if (!entity.TryGetComponent(out Position position)) return;
        
        var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        // change color of capsule to a random color
        capsule.GetComponent<Renderer>().material.color = Random.ColorHSV();
        capsule.transform.parent = entity.transform;

        foreach (var account in spawnedAccounts)
        {
            if (account.Value == null)
            {
                spawnedAccounts[account.Key] = entity.name;
                break;
            }
        }
    }
}

