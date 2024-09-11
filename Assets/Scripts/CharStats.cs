using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour {

    public string charName; // Character's name
    public int playerLevel = 1; // Initial player level
    public int currentEXP; // Current experience points
    public int[] expToNextLevel; // Experience needed to reach the next level
    public int maxLevel = 100; // Maximum level the player can reach
    public int baseEXP = 1000; // Base experience required for the first level up

    public int currentHP; // Current health points (HP)
    public int maxHP = 100; // Maximum health points (HP)
    public int currentMP; // Current magic points (MP)
    public int maxMP = 30; // Maximum magic points (MP)
    public int[] mpLvlBonus; // Magic points bonus gained per level
    public int strength; // Character's strength stat
    public int defence; // Character's defence stat
    public int wpnPwr; // Weapon power (damage)
    public int armrPwr; // Armor power (defense)
    public string equippedWpn; // Name of the equipped weapon
    public string equippedArmr; // Name of the equipped armor
    public Sprite charIamge; // Character's sprite (image)

	// Initialization
	void Start () {
        // Initialize experience needed to level up for all levels
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        // Populate the experience required for each level using a formula that increases it by 5% each level
        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f); // Experience increases progressively
        }
	}
	
	// Update is called once per frame
	void Update () {
		// Debugging feature: Press "K" to add 1000 EXP for testing purposes
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000); // Adds 1000 experience points to the character
        }
	}

    // Adds experience points to the character
    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd; // Add the passed experience to the current experience

        // Check if the player can level up (if below max level)
        if (playerLevel < maxLevel)
        {
            // If the current experience exceeds the required experience for the next level
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel]; // Deduct the experience needed to level up
                playerLevel++; // Increase the player's level

                // Strength increases on even levels, and defence on odd levels
                if (playerLevel % 2 == 0)
                {
                    strength++; // Increase strength on even levels
                }
                else
                {
                    defence++; // Increase defence on odd levels
                }

                // Increase max HP by 5% and restore HP to the new maximum
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                // Add bonus MP for the new level and restore MP to the new maximum
                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }
        }

        // If the player has reached or exceeded the max level, set experience to 0
        if(playerLevel >= maxLevel)
        {
            currentEXP = 0; // Stop accumulating experience once max level is reached
        }
    }
}
