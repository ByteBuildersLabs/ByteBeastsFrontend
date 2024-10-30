using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an item in the game, such as weapons, armors, or consumable items.
/// </summary>
public class Item : MonoBehaviour {
	/// <summary>
    /// Determines whether the item is a regular item, weapon, or armor.
    /// </summary>
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmour;

	/// <summary>
    /// Contains details about the item's name, description, and value.
    /// </summary>
    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

	/// <summary>
    /// Specifies the effects of the item on character stats.
    /// </summary>
    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr;

	/// <summary>
    /// Contains strength values for weapons and armors.
    /// </summary>
    [Header("Weapon/Armor Details")]
    public int weaponStrength;

    public int armorStrength;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
    /// Applies the effects of the item to a specified character.
    /// </summary>
    /// <param name="charToUseOn">Index of the character to apply the item to.</param>
    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

        if(isItem)
        {
            if(affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }

            if(affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }

            if(affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }

        if(isWeapon)
        {
            if(selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
        }

        if(isArmour)
        {
            if (selectedChar.equippedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            selectedChar.equippedArmr = itemName;
            selectedChar.armrPwr = armorStrength;
        }

        GameManager.instance.RemoveItem(itemName);
    }
}
