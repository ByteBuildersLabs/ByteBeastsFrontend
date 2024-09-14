using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Search;

// This class handles the interaction logic between an NPC and the player.
public class NPCInteraction : MonoBehaviour
{
    // A reference to the button that will appear when the player is near the NPC.
    [SerializeField] private GameObject npcInteractionButton;

    // A reference to the NPCDialogue script, which holds the dialogue data for this NPC.
    [SerializeField] private NPCDialogue npcDialogue;

    // Public property to access the NPC's dialogue from other scripts.
    public NPCDialogue Dialogue => npcDialogue;

    // This method is called when another Collider2D enters the trigger collider attached to this GameObject.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if the object entering the trigger has the tag "Player".
        if (collision.CompareTag("Player"))
        {   
            // Sets the current NPC in the DialogueManager to this NPC, allowing dialogue to be managed.
            DialogueManager.Instance.currentNPC = this;

            // Activates the interaction button, allowing the player to initiate dialogue.
            npcInteractionButton.SetActive(true);
        }
    }

    // This method is called when another Collider2D exits the trigger collider attached to this GameObject.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Checks if the object exiting the trigger has the tag "Player".
        if (collision.CompareTag("Player"))
        {   
            // Clears the current NPC in the DialogueManager, as the player has moved away.
            DialogueManager.Instance.currentNPC = null;

            // Deactivates the interaction button, hiding the option to interact.
            npcInteractionButton.SetActive(false);
        }
    } 
}
