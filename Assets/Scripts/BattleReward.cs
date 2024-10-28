using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages the reward system for battles in the game.
/// </summary>
public class BattleReward : MonoBehaviour
{
	/// <value>
    /// Static reference to the BattleReward instance.
    /// </value>
    public static BattleReward instance;

	/// <value>
    /// Reference to the Text component displaying XP earned.
    /// </value>
    public Text xpText;

	 /// <value>
    /// Reference to the Text component displaying items earned.
    /// </value>
    public Text itemText;

	/// <value>
    /// GameObject representing the reward screen UI.
    /// </value>
    public GameObject rewardScreen;

	/// <value>
    /// Array of item names to be displayed as rewards.
    /// </value>
    public string[] rewardItems;
    //public int xpEarned;

	/// <value>
    /// Flag indicating whether a quest should be marked as completed.
    /// </value>
    public bool markQuestComplete;

	/// <value>
    /// Name of the quest to be marked as completed.
    /// </value>
    public string questToMark;

    // Use this for initialization

	/// <summary>
    /// Initializes the BattleReward component.
    /// Sets up the static instance.
    /// </summary>
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] { "Iron sword", "Iron Armor" });
        }
    }

	/// <summary>
    /// Opens the reward screen with the given XP and items earned.
    /// </summary>
    /// <param name="xp">Amount of XP earned.</param>
    /// <param name="rewards">Array of item names earned.</param>
    public void OpenRewardScreen(int xp, string[] rewards)
    {
        // xpEarned = xp;
        rewardItems = rewards;

        // xpText.text = "Everyone earned " + xpEarned + " xp!";
        itemText.text = "";

        for (int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }

	/// <summary>
    /// Closes the reward screen and applies the rewards to the game state.
    /// </summary>
    public void CloseRewardScreen()
    {
        /*
        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned);
            }
        }
        */
		// Apply item rewards
        for (int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }

		// Hide the reward screen
        rewardScreen.SetActive(false);

		// End the current battle
        GameManager.instance.battleActive = false;

		// Mark quest as completed if necessary
        if (markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
