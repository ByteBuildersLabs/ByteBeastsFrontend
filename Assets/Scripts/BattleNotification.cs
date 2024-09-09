using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour {

    // Duration for which the notification will be visible
    public float awakeTime;
    // Counter to keep track of the remaining time before hiding the notification
    private float awakeCounter;
    // Reference to the Text component that displays the notification message
    public Text theText;

    // Use this for initialization
    void Start () {
        // Initialization code can be added here if needed
    }
    
    // Update is called once per frame
    void Update () {
        // Check if the notification is active
        if(awakeCounter > 0)
        {
            // Decrease the counter by the time elapsed since the last frame
            awakeCounter -= Time.deltaTime;

            // If the counter has reached zero or below, hide the notification
            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Method to activate the notification
    public void Activate()
    {
        // Set the notification game object to active
        gameObject.SetActive(true);
        // Reset the counter to the specified awakeTime
        awakeCounter = awakeTime;
    }
}
