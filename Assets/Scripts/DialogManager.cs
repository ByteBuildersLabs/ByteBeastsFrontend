using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages dialogue interactions in the game.
/// </summary>
public class DialogManager : MonoBehaviour {

	/// <value>
    /// Text component that displays the dialogue text.
    /// </value>
    public Text dialogText;

    /// <value>
    /// Text component that displays the character's name.
    /// </value>
    public Text nameText;

    /// <value>
    /// GameObject representing the dialogue box UI.
    /// </value>
    public GameObject dialogBox;

    /// <value>
    /// GameObject representing the character's name box UI.
    /// </value>
    public GameObject nameBox;

    /// <value>
    /// Array of dialogue lines to be displayed.
    /// </value>
    public string[] dialogLines;

    /// <value>
    /// Index of the current line being displayed.
    /// </value>
    public int currentLine;

    /// <value>
    /// Flag indicating whether the dialogue just started.
    /// </value>
    private bool justStarted;

    /// <value>
    /// Singleton instance of the DialogManager.
    /// </value>
    public static DialogManager instance;

    /// <value>
    /// Name of the quest to be activated when this dialogue is triggered.
    /// </value>
    private string questToMark;

    /// <value>
    /// Flag indicating whether the quest should be marked as complete upon activation.
    /// </value>
    private bool markQuestComplete;

    /// <value>
    /// Flag indicating whether the dialogue should activate a quest.
    /// </value>
    private bool shouldMarkQuest;

    /// <summary>
    /// Initializes the DialogManager component.
    /// Sets up the singleton instance.
    /// </summary>
    void Start () {
        instance = this;

		// Initialize the first line of dialogue
        //dialogText.text = dialogLines[currentLine];
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// Handles dialogue progression and quest activation.
    /// </summary>
	void Update () {
		
        if(dialogBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);

                        GameManager.instance.dialogActive = false;

                        if(shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            if(markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            } else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        CheckIfName();

                        dialogText.text = dialogLines[currentLine];
                    }
                } else
                {
                    justStarted = false;
                }

                
            }
        }

	}

	/// <summary>
    /// Shows a new dialogue sequence.
    /// </summary>
    /// <param name="newLines">Array of dialogue lines to display.</param>
    /// <param name="isPerson">Flag indicating whether the speaker is a person.</param>
    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;

        currentLine = 0;

        CheckIfName();

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;

        nameBox.SetActive(isPerson);

        GameManager.instance.dialogActive = true;
    }

	 /// <summary>
    /// Checks if the current line starts with "n-" and sets the name accordingly.
    /// </summary>
    public void CheckIfName()
    {
        if(dialogLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

	/// <summary>
    /// Sets up quest activation details for the end of the dialogue.
    /// </summary>
    /// <param name="questName">Name of the quest to activate.</param>
    /// <param name="markComplete">Flag indicating whether to mark the quest as complete.</param>
    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
