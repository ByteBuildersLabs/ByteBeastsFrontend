using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class defines the structure for different types of battles in the game
[System.Serializable]
public class BAttleType {

    // Array of strings representing the types of enemies encountered in this battle
    public string[] enemies;

    // Amount of experience points rewarded for winning this battle
    public int rewardXP;

    // Array of strings representing the items rewarded for winning this battle
    public string[] rewardItems;
}
