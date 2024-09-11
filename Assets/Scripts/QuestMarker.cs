using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour {

    // The name of the quest to be marked as complete or incomplete
    public string questToMark;

    // If true, the quest will be marked as complete. If false, it will be marked as incomplete.
    public bool markComplete;

    // If true, the quest will be marked when the player enters the trigger collider. Otherwise, it will be marked when the player presses a button.
    public bool markOnEnter;
    
    // A flag to determine if the quest can be marked when the player presses the button
    private bool canMark;

    // If true, the GameObject will be deactivated after marking the quest
    public bool deactivateOnMarking;

    // Use this for initialization
    void Start () {
        // Initialization code here if needed
    }
    
    // Update is called once per frame
    void Update () {
        // Check if the quest can be marked and the button is pressed
        if(canMark && Input.GetButtonDown("Fire1"))
        {
            canMark = false; // Prevent further marking until the player exits and re-enters the trigger
            MarkQuest(); // Call the method to mark the quest
        }
    }

    // Method to mark the quest as complete or incomplete
    public void MarkQuest()
    {
        // Mark the quest as complete or incomplete based on the markComplete flag
        if(markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        } else
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }

        // Deactivate the GameObject if deactivateOnMarking is true
        gameObject.SetActive(!deactivateOnMarking);
    }

    // Called when another collider enters the trigger collider of this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.tag == "Player")
        {
            // Mark the quest if markOnEnter is true
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                // Set canMark to true to allow marking when the player presses the button
                canMark = true;
            }
        }
    }

    // Called when another collider exits the trigger collider of this GameObject
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.tag == "Player")
        {
            // Prevent marking the quest when the player is not in the trigger collider
            canMark = false;
        }
    }
}
