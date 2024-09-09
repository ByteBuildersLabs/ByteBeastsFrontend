using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleReward : MonoBehaviour {

    // Singleton instance of the BattleReward class
    public static BattleReward instance;

    // References to the UI elements for displaying XP and items
    public Text xpText, itemText;
    // Reference to the reward screen UI
    public GameObject rewardScreen;

    // Array of reward items and the amount of XP earned
    public string[] rewardItems;
    public int xpEarned;

    // Flags for marking quest completion
    public bool markQuestComplete;
    public string questToMark;

    // Use this for initialization
    void Start () {
        // Initialize the singleton instance
        instance = this;
    }
    
    // Update is called once per frame
    void Update () {
        // For testing purposes: Press 'Y' key to open the reward screen with sample data
        if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] { "Iron sword", "Iron Armor" });
        }
    }

    // Method to open the reward screen with specified XP and rewards
    public void OpenRewardScreen(int xp, string[] rewards)
    {
        // Set XP earned and rewards
        xpEarned = xp;
        rewardItems = rewards;

        // Update the text elements to show the XP earned and the list of rewards
        xpText.text = "Everyone earned " + xpEarned + " xp!";
        itemText.text = "";

        // Append each reward item to the itemText UI element
        for(int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        // Display the reward screen
        rewardScreen.SetActive(true);
    }

    // Method to close the reward screen and handle the rewards
    public void CloseRewardScreen()
    {
        // Add XP to each active player
        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned);
            }
        }

        // Add each reward item to the player's inventory
        for(int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }

        // Hide the reward screen and set battle status to inactive
        rewardScreen.SetActive(false);
        GameManager.instance.battleActive = false;

        // If required, mark the quest as complete
        if(markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
