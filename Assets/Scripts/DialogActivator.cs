using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates dialogues when triggered by the player.
/// </summary>
public class DialogActivator : MonoBehaviour {

	/// <value>
    /// Array of dialogue lines to be displayed.
    /// </value>
    public string[] lines;

	/// <value>
    /// Flag indicating whether the activator can trigger dialogues.
    /// </value>
    private bool canActivate;

	/// <value>
    /// Indicates whether this activator is for a person.
    /// </value>
    public bool isPerson = true;

	/// <value>
    /// Flag to determine if activating this dialogue should activate a quest.
    /// </value>
    public bool shouldActivateQuest;

	/// <value>
    /// Name of the quest to be activated when this dialogue is triggered.
    /// </value>
    public string questToMark;

	/// <value>
    /// Flag indicating whether the quest should be marked as complete upon activation.
    /// </value>
    public bool markComplete;


	/// <summary>
    /// Initializes the DialogActivator component.
    /// </summary>
	void Start () {
		
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// Checks for player input to activate dialogues.
    /// </summary>
	void Update () {
		if(canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
	}

	/// <summary>
    /// Triggered when another collider enters the trigger volume.
    /// Sets the canActivate flag to true.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = true;
        }
    }

	/// <summary>
    /// Triggered when another collider exits the trigger volume.
    /// Sets the canActivate flag to false.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }
    }
}
