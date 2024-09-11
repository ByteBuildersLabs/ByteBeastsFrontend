using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {

    // Name of the transition area this entrance corresponds to
    public string transitionName;

    // Called when the script is first run or when the object is instantiated
    void Start () {
        // Check if the current transitionName matches the player's transition name
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
            // If it matches, set the player's position to the position of this entrance
            PlayerController.instance.transform.position = transform.position;
        }

        // Fade in from black when entering the area
        UIFade.instance.FadeFromBlack();

        // Set the game manager's flag to indicate that the area transition is complete
        GameManager.instance.fadingBetweenAreas = false;
    }
    
    // Update is called once per frame
    void Update () {
        // Currently, no logic is executed during the update phase
    }
}
