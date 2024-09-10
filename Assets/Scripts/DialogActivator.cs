using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour {

    public string[] lines; // Lines of dialogue to be displayed

    private bool canActivate; // Determines if the dialogue can be activated

    public bool isPerson = true; // Checks if the entity is a person (affects dialogue behavior)

    public bool shouldActivateQuest; // Determines if a quest should be activated after the dialogue
    public string questToMark; // The quest to be marked (started or completed)
    public bool markComplete; // Marks whether the quest should be completed


    // Use this for initialization
    void Start () {
        // Initialization logic can go here if needed
    }
	
	// Update is called once per frame
	void Update () {
        // Check if the player is in the activation area and presses the activation button ("Fire1"),
        // while also ensuring no dialogue is currently active
		if(canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            // Show the dialogue with the corresponding lines and the "isPerson" condition
            DialogManager.instance.ShowDialog(lines, isPerson);

            // Check if a quest should be activated or marked complete after the dialogue ends
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
	}

    // Trigger when the player enters the collider area (indicating they are near the entity)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canActivate = true; // Allow dialogue activation
        }
    }

    // Trigger when the player exits the collider area (indicating they have moved away)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false; // Disable dialogue activation
        }
    }
}
