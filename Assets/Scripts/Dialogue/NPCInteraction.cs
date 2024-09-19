using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles the interaction logic between an NPC and the player.
public class NPCInteraction : MonoBehaviour
{
    // A reference to the button that will appear when the player is near the NPC.
    [SerializeField] private GameObject npcInteractionButton;

    // A reference to the NPCDialogue script, which holds the dialogue data for this NPC.
    [SerializeField] private NPCDialogue npcDialogue;

    [SerializeField] private NPCMovement npcMovement; // Reference to the NPC's movement script

    [SerializeField] private Animator npcAnimator;

    private bool wasMoving; // Flag to remember if the NPC was moving before interaction

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

            // Stop NPC movement when the player interacts
            if (npcMovement != null)
            {  
                npcMovement.enabled = false;
            }

            // Stop NPC animation when the player interacts
            if (npcAnimator != null)
            {
                npcAnimator.enabled = false; // Pauses the Animator
            }
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

            // Resume NPC movement when the interaction ends
            if (npcMovement != null)
            {
                npcMovement.enabled = true;
            }

            // Resume NPC animation when the interaction ends
            if (npcAnimator != null)
            {
                npcAnimator.enabled = true; // Resumes the Animator
            }
        }
    } 
}
