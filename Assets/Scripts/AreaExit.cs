using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script manages the exit from an area in the game, including loading the next area and handling transitions.
/// </summary>
public class AreaExit : MonoBehaviour
{

    /// <value>
    /// The name of the area to load after exiting the current area.
    /// </value>
    public string areaToLoad;

    /// <value>
    /// The transition name to use when leaving the current area.
    /// </value>
    public string areaTransitionName;

    /// <value>
    /// Reference to the AreaEntrance component in the next area.
    /// </value>
    public AreaEntrance theEntrance;

    /// <value>
    /// Time delay before loading the next area after fading out.
    /// </value>
    public float waitToLoad = 1f;


    /// <value>
    /// Flag indicating whether the scene should be loaded after fading out.
    /// </value>
    private bool shouldLoadAfterFade;

    // Use this for initialization

    /// <summary>
    /// Initializes the AreaExit component.
    /// Sets the transition name for the entrance in the next area.
    /// </summary>
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;

    }

    // Update is called once per frame

    /// <summary>
    /// Called every frame after Start().
    /// Handles loading the next area after fading out.
    /// </summary>
    void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    /// <summary>
    /// Triggered when the player collides with this exit.
    /// Sets up the transition, fades out the screen, and prepares to load the next area.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "Town")
            {
                Vector3 entrancePosition = theEntrance.gameObject.transform.position;
                SaveEntrancePositionToPlayerPrefs(entrancePosition);
            }

            //SceneManager.LoadScene(areaToLoad);
            // Prepare to load the next area
            shouldLoadAfterFade = true;
            GameManager.instance.fadingBetweenAreas = true;

            // Fade out the screen
            UIFade.instance.FadeToBlack();

            // Set the player's transition name
            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }

    public static void SaveEntrancePositionToPlayerPrefs(Vector3 vector)
    {
        PlayerPrefs.SetFloat("entrancePosition_x", vector.x);
        PlayerPrefs.SetFloat("entrancePosition_y", vector.y);
        PlayerPrefs.SetFloat("entrancePosition_z", vector.z);
        PlayerPrefs.Save();
    }
}
