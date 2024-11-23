using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script manages the entrance to an area in the game.
/// </summary>
public class AreaEntrance : MonoBehaviour
{
    /// <value>
    /// The name of the transition to use when entering this area.
    /// </value>
    public string transitionName;

    // Use this for initialization
    // test

    /// <summary>
    /// Initializes the AreaEntrance component.
    /// </summary>
    void Start()
    {
        // Check if the current area transition matches the one set for this entrance
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
            // Position the player at the entrance position
            PlayerController.instance.transform.position = transform.position;
        }

        // Overwrite player's position if it exists in PlayerPrefs
        if (SceneManager.GetActiveScene().name == "Town" && LoadEntrancePositionFromPlayerPrefs() != Vector3.zero)
        {
            PlayerController.instance.transform.position = LoadEntrancePositionFromPlayerPrefs();
            PlayerController.lastHouseEntered = Vector3.zero;
        }

        // Fade out the black screen
        UIFade.instance.FadeFromBlack();

        // Reset the fading-between-areas flag
        GameManager.instance.fadingBetweenAreas = false;
    }

    // Update is called once per frame

    /// <summary>
    /// Called every frame after Start().
    /// </summary>
    void Update()
    {

    }

    public static Vector3 LoadEntrancePositionFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey($"entrancePosition_x"))
        {
            float x = PlayerPrefs.GetFloat("entrancePosition_x");
            float y = PlayerPrefs.GetFloat("entrancePosition_y");
            float z = PlayerPrefs.GetFloat("entrancePosition_z");
            return new Vector3(x, y, z);
        }
        else
        {
            Debug.LogWarning($"No Vector3 data found in PlayerPrefs for entrancePosition");
            return Vector3.zero;
        }
    }

    private void ClearEntrancePositionFromPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("entrancePosition_x");
        PlayerPrefs.DeleteKey("entrancePosition_y");
        PlayerPrefs.DeleteKey("entrancePosition_z");
    }

    private void OnApplicationQuit()
    {
        ClearEntrancePositionFromPlayerPrefs();
    }
}
