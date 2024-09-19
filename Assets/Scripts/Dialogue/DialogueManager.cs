using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The DialogueManager class manages NPC dialogue interactions using a singleton pattern.
public class DialogueManager : Singleton<DialogueManager>
{
    // References to UI elements in the dialogue panel.
    [SerializeField] private GameObject dialoguePanel;         // Panel that displays the dialogue UI.
    [SerializeField] private Image npcIcon;                    // UI Image component to display the NPC's icon.
    [SerializeField] private TextMeshProUGUI npcNameTMP;       // TextMeshPro component to display the NPC's name.
    [SerializeField] private TextMeshProUGUI npcConversationTMP; // TextMeshPro component to display the NPC's dialogue text.

    // Current NPC being interacted with.
    public NPCInteraction currentNPC { get; set; }

    // Queue to manage the dialogue lines that are displayed sequentially.
    private Queue<string> dialogueQueue;

    // Flags to control the dialogue animation and goodbye state.
    private bool animatedDialogue;
    private bool showGoodBye;

    private bool isDialogueActive = false; // Flag to track if the dialogue panel is active.

    private float lastTapTime = 0f;  // Time when the last tap occurred.
    private float doubleTapTime = 0.3f;  // Max time allowed between two taps to consider it a double tap.


    // Called at the start of the game, initializes the dialogue queue.
    private void Start()
    {
        dialogueQueue = new Queue<string>();
    }

    // Called once per frame to handle input and dialogue interactions.
    private void Update()
    {
        // If there is no current NPC, do nothing.
        if (currentNPC == null) return;

        // Handle input for both PC (keyboard) and mobile (touch).
        HandleInput();
    }

    // Handles both keyboard and touch input for starting and progressing dialogues.
    private void HandleInput()
    {
        // Check for input to start the dialogue panel (initial touch or key press).
        if (!isDialogueActive && (Input.GetKeyDown(KeyCode.E) || DoubleTapDetected()))
        {
            SetPanel(currentNPC.Dialogue); // Only start the dialogue if it's not active.
            isDialogueActive = true; // Set the flag to true after starting the dialogue.
        }

        // Check for input to advance dialogue (space key or touch anywhere on the screen).
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || TouchDetected()))
        {
            // If it's time to say goodbye, close the dialogue panel.
            if (showGoodBye)
            {
                OpenDialoguePanel(false);
                showGoodBye = false;
                isDialogueActive = false; // Reset the flag when the dialogue is closed.
                return;
            }

            // If the current dialogue is done animating, display the next dialogue.
            if (animatedDialogue)
            {
                DisplayNextDialogue();
            }
        }
    }

    // Method to detect if the player has touched the screen.
    private bool TouchDetected()
    {
        // Check if there is at least one touch on the screen.
        if (Input.touchCount > 0)
        {
            // Check if the touch just began, meaning the player just tapped the screen.
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Began)
            {
                return true; // A touch was detected, returning true.
            }
        }
        return false; // No touch detected.
    }

    // Detects if the player has performed a double tap on the screen.
    private bool DoubleTapDetected()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Detect if the touch just began (first tap).
            if (touch.phase == TouchPhase.Began)
            {
                // Check if the time between the current tap and the last tap is within the allowed double-tap time window.
                if (Time.time - lastTapTime < doubleTapTime)
                {
                    // Reset the last tap time and confirm double tap detected.
                    lastTapTime = 0f;  
                    return true;  // Double tap detected.
                }
                else
                {
                    // If too much time has passed, consider this the first tap and update the lastTapTime.
                    lastTapTime = Time.time;
                }
            }
        }
        return false;  // No double tap detected.
    }

    // Opens or closes the dialogue panel.
    public void OpenDialoguePanel(bool state)
    {
        dialoguePanel.SetActive(state);
    }

    // Sets up the dialogue panel with the NPC's details and starts the conversation.
    private void SetPanel(NPCDialogue npcDialogue)
    {   
        OpenDialoguePanel(true); // Open the dialogue panel.
        LoadDialogueQueue(npcDialogue); // Load the dialogue lines into the queue.

        // Set the NPC's icon and name in the UI.
        npcIcon.sprite = npcDialogue.npcIcon;
        npcNameTMP.text = $"{npcDialogue.npcName}:";

        // Display the NPC's greeting message with animation.
        DisplayAnimatedText(npcDialogue.Greeting);
    }

    // Loads the NPC's conversation lines into the dialogue queue.
    private void LoadDialogueQueue(NPCDialogue npcDialogue)
    {
        // If there are no conversations defined, return early.
        if (npcDialogue.Conversation == null || npcDialogue.Conversation.Length <= 0) return;

        // Add each line of the conversation to the queue.
        for (int i = 0; i < npcDialogue.Conversation.Length; i++)
        {
            dialogueQueue.Enqueue(npcDialogue.Conversation[i].Dialogue);
        }
    }

    // Animates the dialogue text by displaying one character at a time.
    private IEnumerator TextAnimator(string dialogue)
    {
        animatedDialogue = false; // Set the flag to false to indicate that the animation is running.

        npcConversationTMP.text = string.Empty; // Clear the dialogue text.
        char[] letters = dialogue.ToCharArray(); // Convert the dialogue string to an array of characters.

        // Display each character one by one with a small delay.
        for (int i = 0; i < letters.Length; i++)
        {
            npcConversationTMP.text += letters[i];
            yield return new WaitForSeconds(0.03f);
        }

        animatedDialogue = true; // Set the flag to true to indicate that the animation is complete.
    }

    // Displays the next line of dialogue or ends the conversation if there are no more lines.
    private void DisplayNextDialogue()
    {
        // If there is no current NPC or if the goodbye message is already showing, return.
        if (currentNPC == null) return;
        if (showGoodBye) return;

        // If the queue is empty, display the NPC's goodbye message.
        if (dialogueQueue.Count == 0)
        {
            string goodBye = currentNPC.Dialogue.GoodBye;
            DisplayAnimatedText(goodBye);
            showGoodBye = true; // Set the flag to show that the goodbye message is being shown.
            return;
        }

        // Get the next dialogue line from the queue and display it with animation.
        string nextDialogue = dialogueQueue.Dequeue();
        DisplayAnimatedText(nextDialogue);
    }

    // Starts the animation of the given dialogue string.
    private void DisplayAnimatedText(string dialogue)
    {
        StartCoroutine(TextAnimator(dialogue));
    }
}
