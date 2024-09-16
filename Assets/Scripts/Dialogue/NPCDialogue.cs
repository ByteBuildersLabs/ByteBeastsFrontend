using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Enum defining the different types of NPC interactions available.
public enum NPCInteractionType
{
    OfferItem, // NPC can offer an item to the player.
    Quest      // NPC can offer a quest to the player.
}

// This class defines an NPC's dialogue using ScriptableObject, which allows creating dialogue data assets.
[CreateAssetMenu] // Attribute to allow creating this ScriptableObject from the Unity menu.
public class NPCDialogue : ScriptableObject
{
    // Section header in the Inspector for organization.
    [Header("Information")]
    public string npcName; // Name of the NPC.
    public Sprite npcIcon; // Icon of the NPC to display in dialogues.
    public bool hasInteraction; // Indicates if the NPC has any interaction available.
    public NPCInteractionType npcInteractionType; // Type of interaction the NPC provides.

    // Section header for the NPC's greeting dialogue.
    [Header("Greeting")]
    [TextArea] public string Greeting; // The initial greeting text the NPC says.

    // Section header for the main conversation dialogues.
    [Header("Chat")]
    public TextDialogue[] Conversation; // Array of dialogue lines that form the main conversation.

    // Section header for the NPC's goodbye dialogue.
    [Header("GoodBye")]
    [TextArea] public string GoodBye; // The closing dialogue text when the interaction ends.
}

// Serializable class to define individual pieces of dialogue text.
[Serializable]
public class TextDialogue
{
    [TextArea] public string Dialogue; // A single piece of dialogue text from the NPC.
}
