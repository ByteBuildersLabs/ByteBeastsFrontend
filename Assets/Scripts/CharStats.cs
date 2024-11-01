using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages character statistics and progression in the game.
/// </summary>
public class CharStats : MonoBehaviour {

    /// <value>
    /// Name of the character.
    /// </value>
    public string charName;

    /// <value>
    /// Current player level.
    /// </value>
    public int playerLevel = 1;

    /// <value>
    /// Current experience points.
    /// </value>
    public int currentEXP;

    /// <value>
    /// Array of experience points required for each level.
    /// </value>
    public int[] expToNextLevel;

    /// <value>
    /// Maximum allowed level.
    /// </value>
    public int maxLevel = 100;

    /// <value>
    /// Base experience points required to reach the next level.
    /// </value>
    public int baseEXP = 1000;

    /// <value>
    /// Current hit points.
    /// </value>
    public int currentHP;

    /// <value>
    /// Maximum hit points.
    /// </value>
    public int maxHP = 100;

    /// <value>
    /// Current mana points.
    /// </value>
    public int currentMP;

    /// <value>
    /// Maximum mana points.
    /// </value>
    public int maxMP = 30;

    /// <value>
    /// Array of mana point bonuses per level.
    /// </value>
    public int[] mpLvlBonus;

    /// <value>
    /// Character strength stat.
    /// </value>
    public int strength;

    /// <value>
    /// Character defense stat.
    /// </value>
    public int defence;

    /// <value>
    /// Weapon power stat.
    /// </value>
    public int wpnPwr;

    /// <value>
    /// Armor power stat.
    /// </value>
    public int armrPwr;

    /// <value>
    /// Name of the currently equipped weapon.
    /// </value>
    public string equippedWpn;

    /// <value>
    /// Name of the currently equipped armor.
    /// </value>
    public string equippedArmr;

    /// <value>
    /// Sprite representing the character's image.
    /// </value>
    public Sprite charImage;

    /// <value>
    /// Flag indicating whether to recover HP and MP when leveling up.
    /// </value>
    public bool recoverOnLevelUp = false;

	/// <summary>
    /// Initializes the CharStats component.
    /// Sets up the experience progression curve.
    /// </summary>
	void Start () {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
	}

 	/// <summary>
    /// Adds experience points to the character.
    /// Handles level ups and stat improvements.
    /// </summary>
    /// <param name="expToAdd">Amount of experience points to add.</param>
    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //determine whether to add to str or def based on odd or even
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

				// Increase maximum hit points and mana points
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                maxMP += mpLvlBonus[playerLevel];

				// Recover HP and MP if enabled
                if (recoverOnLevelUp)
                {
                    currentHP = maxHP;
                    currentMP = maxMP;
                }
            }
        }

		// Reset experience if at maximum level
        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
