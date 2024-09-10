using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    [Header("Item Type")]
    public bool isItem; // Indicates if the item is a generic item
    public bool isWeapon; // Indicates if the item is a weapon
    public bool isArmour; // Indicates if the item is armor

    [Header("Item Details")]
    public string itemName; // The name of the item
    public string description; // Description of the item
    public int value; // The value of the item (possibly for selling or trading)
    public Sprite itemSprite; // Visual representation of the item

    [Header("Item Effects")]
    public int amountToChange; // Amount to change the character's stats (HP, MP, Strength)
    public bool affectHP; // Whether the item affects HP
    public bool affectMP; // Whether the item affects MP
    public bool affectStr; // Whether the item affects Strength

    [Header("Weapon/Armor Details")]
    public int weaponStrength; // Strength of the weapon
    public int armorStrength; // Strength of the armor

    // Use this for initialization
    void Start () {
        // Initialization code if needed
    }
    
    // Update is called once per frame
    void Update () {
        // Update logic if needed
    }

    // Method to use the item on a character
    public void Use(int charToUseOn) {
        // Get the character stats from GameManager
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        // Check if the item is a generic item
        if (isItem) {
            // If the item affects HP
            if (affectHP) {
                selectedChar.currentHP += amountToChange;

                // Ensure HP does not exceed the maximum HP
                if (selectedChar.currentHP > selectedChar.maxHP) {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            // If the item affects MP
            if (affectMP) {
                selectedChar.currentMP += amountToChange;

                // Ensure MP does not exceed the maximum MP
                if (selectedChar.currentMP > selectedChar.maxMP) {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            // If the item affects Strength
            if (affectStr) {
                selectedChar.strength += amountToChange;
            }
        }

        // Check if the item is a weapon
        if (isWeapon) {
            // If the character already has a weapon equipped, return it to the inventory
            if (selectedChar.equippedWpn != "") {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            // Equip the new weapon
            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength; // Set the weapon's strength
        }

        // Check if the item is armor
        if (isArmour) {
            // If the character already has armor equipped, return it to the inventory
            if (selectedChar.equippedArmr != "") {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            // Equip the new armor
            selectedChar.equippedArmr = itemName;
            selectedChar.armrPwr = armorStrength; // Set the armor's strength
        }

        // Remove the item from the inventory
        GameManager.instance.RemoveItem(itemName);
    }
}
