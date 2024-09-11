using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {

    // Time to wait before loading the scene
    public float waitToLoad;

    // Called when the script instance is being loaded
    void Start () {
        // Initialization code (if any) goes here
    }

    // Called once per frame
    void Update () {
        // Check if there is a wait time remaining
        if (waitToLoad > 0)
        {
            // Decrease the wait time based on elapsed time
            waitToLoad -= Time.deltaTime;
            // If wait time has elapsed
            if (waitToLoad <= 0)
            {
                // Load the scene specified by "Current_Scene" in PlayerPrefs
                SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

                // Load game data and quest data from GameManager and QuestManager
                GameManager.instance.LoadData();
                QuestManager.instance.LoadQuestData();
            }
        }
    }
}
