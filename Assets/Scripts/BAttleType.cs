using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a type of battle scenario in the game.
/// </summary>
[System.Serializable]
public class BAttleType {

	 /// <value>
    /// Array of enemy names that will appear in this battle type.
    /// </value>
    public string[] enemies;

	/// <value>
    /// Amount of XP rewarded upon winning this battle type.
    /// </value>
    public int rewardXP;

	/// <value>
    /// Array of item names that will be rewarded upon winning this battle type.
    /// </value>
    public string[] rewardItems;
}
