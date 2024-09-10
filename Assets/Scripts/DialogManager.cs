using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    public Text dialogText; // The text component displaying the dialog
    public Text nameText; // The text component displaying the speaker's name
    public GameObject dialogBox; // The UI element representing the dialog box
    public GameObject nameBox; // The UI element representing the name box

    public string[] dialogLines; // Array of dialog lines to be shown

    public int currentLine; // Tracks the current dialog line being displayed
    private bool justStarted; // Flag to track if the dialog just started

    public static DialogManager instance; // Singleton instance of the DialogManager

    private string questToMark; // The name of the quest to mark as complete/incomplete
    private bool markQuestComplete; // Flag indicating if the quest should be marked complete
    private bool shouldMarkQuest; // Flag indicating if a quest should be marked at the end of the dialog

    // Use this for initialization
    void Start () {
        // Set the singleton instance for global access
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
        // Check if the dialog box is currently active
        if(dialogBox.activeInHierarchy)
        {
            // Check if the player releases the "Fire1" button (usually the action button)
            if(Input.GetButtonUp("Fire1"))
            {
                // If the dialog just started, skip this frame to prevent instant line progression
                if (!justStarted)
                {
                    // Move to the next line of dialog
                    currentLine++;

                    // Check if we've reached the end of the dialog
                    if (currentLine >= dialogLines.Length)
                    {
                        // Deactivate the dialog box and reset the dialog state
                        dialogBox.SetActive(false);
                        GameManager.instance.dialogActive = false;

                        // If there's a quest to mark, update the quest state
                        if(shouldMarkQuest)
                        {
                            shouldMarkQuest = false; // Reset the flag
                            if(markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark); // Mark the quest as complete
                            } 
                            else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark); // Mark the quest as incomplete
                            }
                        }
                    }
                    else
                    {
                        // Check if the next line should update the speaker's name
                        CheckIfName();

                        // Update the dialog text to the current line
                        dialogText.text = dialogLines[currentLine];
                    }
                } 
                else
                {
                    // If the dialog just started, set the flag to false after the first input
                    justStarted = false;
                }
            }
        }
	}

    // Method to display a new dialog sequence
    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines; // Set the new lines of dialog
        currentLine = 0; // Start at the first line

        // Check if the first line includes a name indicator
        CheckIfName();

        // Set the dialog text to the current line
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true); // Activate the dialog box

        justStarted = true; // Set the flag to indicate that the dialog just started

        nameBox.SetActive(isPerson); // Show or hide the name box based on whether the speaker is a person

        GameManager.instance.dialogActive = true; // Update the game state to indicate that a dialog is active
    }

    // Method to check if the current line includes a name for the speaker
    public void CheckIfName()
    {
        // If the line starts with "n-", treat it as a name indicator
        if(dialogLines[currentLine].StartsWith("n-"))
        {
            // Extract the name by removing the "n-" prefix and display it
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++; // Move to the next line after the name
        }
    }

    // Method to set a quest to be marked at the end of the dialog
    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName; // Set the quest name
        markQuestComplete = markComplete; // Set whether to mark it as complete or incomplete

        shouldMarkQuest = true; // Set the flag to mark the quest at the end of the dialog
    }
}
