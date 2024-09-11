using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour {

    // The name of the scene to load when exiting the area
    public string areaToLoad;

    // The name of the transition area to set in the PlayerController
    public string areaTransitionName;

    // Reference to the AreaEntrance component associated with this exit
    public AreaEntrance theEntrance;

    // Time to wait before loading the new scene (after fading)
    public float waitToLoad = 1f;
    // Flag to indicate whether the scene should be loaded after fading
    private bool shouldLoadAfterFade;

    // Called when the script is first run or when the object is instantiated
    void Start () {
        // Set the transition name in the AreaEntrance to match this exit's transition name
        theEntrance.transitionName = areaTransitionName;
    }
    
    // Update is called once per frame
    void Update () {
        // If a fade is happening and the timer has elapsed
        if(shouldLoadAfterFade)
        {
            // Decrease the wait time
            waitToLoad -= Time.deltaTime;
            // If the wait time is less than or equal to zero
            if(waitToLoad <= 0)
            {
                // Reset the flag and load the new scene
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    // Called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if(other.tag == "Player")
        {
            // Start the fade effect and set the flag to load the new scene after fading
            shouldLoadAfterFade = true;
            GameManager.instance.fadingBetweenAreas = true;
            UIFade.instance.FadeToBlack();

            // Set the player's transition name to match this exit's transition name
            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
