using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script initiates battles in the game world.
/// </summary>
public class BattleStarter : MonoBehaviour {

	/// <value>
    /// Array of potential battles that can occur.
    /// Each element represents a different type of battle scenario.
    /// </value>
    public BAttleType[] potentialBattles;

	/// <value>
    /// Flags indicating whether the battle starter should activate based on player entry, stay, or exit.
    /// </value>
    public bool activateOnEnter, activateOnStay, activateOnExit;

	/// <value>
    /// Flag indicating whether the character is currently in the battle area.
    /// </value>
    private bool inArea;

	 /// <value>
    /// Time interval between battles.
    /// </value>
    public float timeBetweenBattles = 10f;

	/// <value>
    /// Counter tracking the remaining time before the next battle starts.
    /// </value>
    private float betweenBattleCounter;

	/// <value>
    /// Flag indicating whether the battle starter should deactivate after starting a battle.
    /// </value>
    public bool deactivateAfterStarting;

	/// <value>
    /// Flag preventing the player from fleeing during the battle.
    /// </value>
    public bool cannotFlee;

	/// <value>
    /// Flag indicating whether a quest should be completed upon winning the battle.
    /// </value>
    public bool shouldCompleteQuest;

	/// <value>
    /// Name of the quest to be completed upon winning the battle.
    /// </value>
    public string QuestToComplete;

	// Use this for initialization

	/// <summary>
    /// Initializes the BattleStarter component.
    /// Sets up the initial time counter for the next battle.
    /// </summary>
	void Start () {
		// Set the initial time counter randomly between half and double the base time
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
	}
	
	// Update is called once per frame

	/// <summary>
    /// Called every frame after Start().
    /// Handles the countdown and initiation of battles.
    /// </summary>
	void Update () {
		if(inArea && PlayerController.instance.canMove)
        {
			// Check for player movement input
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || PlayerController.instance.joystick.Horizontal != 0 || PlayerController.instance.joystick.Vertical!=0)
            {
				// Decrease the time counter when movement occurs
                betweenBattleCounter -= Time.deltaTime;
            }

			// Start a battle when the time counter reaches zero
            if(betweenBattleCounter <= 0)
            {
				// Reset the time counter with random variation
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);

				// Start the battle coroutine
                StartCoroutine(StartBattleCo());
            }
        }
	}

	/// <summary>
    /// Triggered when the player enters the battle area.
    /// Starts a battle immediately if activateOnEnter is true.
    /// Otherwise, sets the inArea flag to true.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (activateOnEnter)
            {
				// Start the battle immediately
                StartCoroutine(StartBattleCo());
            }
            else
            {
				// Mark the player as no longer being in the battle area
                inArea = true;
            }
        }
    }

	/// <summary>
    /// Triggered when the player exits the battle area.
    /// Starts a battle immediately if activateOnExit is true.
    /// Otherwise, resets the inArea flag to false.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activateOnExit)
            {
				// Start the battle immediately
                StartCoroutine(StartBattleCo());
            }
            else
            {
				// Mark the player as no longer being in the battle area
                inArea = false;
            }
        }
    }

	/// <summary>
    /// Coroutine to initiate a battle sequence.
    /// </summary>
    /// <returns>A coroutine representing the battle initiation process.</returns>
    public IEnumerator StartBattleCo()
    {
		// Fades to black screen
        UIFade.instance.FadeToBlack();

		// Set the battle state to active
        GameManager.instance.battleActive = true;

		// Select a random battle type from the potential battles array
        int selectedBattle = Random.Range(0, potentialBattles.Length);

		// Configure the reward items and XP for the selected battle
        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

		// Wait for a brief moment before initiating the battle
        yield return new WaitForSeconds(1.5f);

		// Start the actual battle
        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);

		// Fade from black screen
        UIFade.instance.FadeFromBlack();

		// Deactivate this game object if requested
        if(deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

		// Prepare for quest completion if necessary
        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = QuestToComplete;
    }
}
