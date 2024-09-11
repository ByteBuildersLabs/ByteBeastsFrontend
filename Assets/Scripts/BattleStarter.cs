using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour {

    public BAttleType[] potentialBattles; // List of potential battles that can start

    public bool activateOnEnter, activateOnStay, activateOnExit; // Flags to determine when to start the battle (on entering, staying, or exiting the zone)

    private bool inArea; // Indicates if the player is inside the activation area
    public float timeBetweenBattles = 10f; // Time between battles
    private float betweenBattleCounter; // Counter for the time between battles

    public bool deactivateAfterStarting; // If true, deactivates the trigger after starting the battle

    public bool cannotFlee; // Determines if fleeing from the battle is possible

    public bool shouldCompleteQuest; // Indicates if a quest should be completed after the battle
    public string QuestToComplete; // Name of the quest to complete

    // Initialization
    void Start () {
        // Sets the counter between battles to a random range
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
    }
    
    // Called once per frame
    void Update () {
        // If the player is in the area and can move
        if(inArea && PlayerController.instance.canMove)
        {
            // If the player is moving (using keyboard or joystick)
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || PlayerController.instance.joystick.Horizontal != 0 || PlayerController.instance.joystick.Vertical != 0)
            {
                betweenBattleCounter -= Time.deltaTime; // Decreases the counter between battles
            }

            // If the counter reaches 0, start a battle
            if(betweenBattleCounter <= 0)
            {
                // Resets the counter between battles with a new random value
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);

                // Starts the battle
                StartCoroutine(StartBattleCo());
            }
        }
    }

    // Called when the player enters the activation area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") // Checks if the object entering is the player
        {
            if (activateOnEnter) // If the battle should activate on entering
            {
                StartCoroutine(StartBattleCo()); // Start the battle
            }
            else
            {
                inArea = true; // Marks that the player is in the area
            }
        }
    }

    // Called when the player exits the activation area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") // Checks if the object exiting is the player
        {
            if (activateOnExit) // If the battle should activate on exiting
            {
                StartCoroutine(StartBattleCo()); // Start the battle
            }
            else
            {
                inArea = false; // Marks that the player has left the area
            }
        }
    }

    // Coroutine that handles starting the battle
    public IEnumerator StartBattleCo()
    {
        UIFade.instance.FadeToBlack(); // Starts the transition to a black screen
        GameManager.instance.battleActive = true; // Marks that the battle is active

        int selectedBattle = Random.Range(0, potentialBattles.Length); // Selects a random battle from the potential ones

        // Sets the rewards for the selected battle
        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        yield return new WaitForSeconds(1.5f); // Waits briefly before starting the battle

        // Starts the battle with the selected enemies and the option to flee
        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);
        UIFade.instance.FadeFromBlack(); // Ends the transition from the black screen

        if(deactivateAfterStarting) // If the object should deactivate after starting the battle
        {
            gameObject.SetActive(false); // Deactivates this object
        }

        // Marks the quest as complete if necessary
        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = QuestToComplete;
    }
}
