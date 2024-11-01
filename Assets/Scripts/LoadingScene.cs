using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the loading of scenes based on the current scene and saved data.
/// </summary>
public class LoadingScene : MonoBehaviour {
	/// <summary>
    /// Delay time before loading the next scene.
    /// </summary>
    public float waitToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	/// <summary>
    /// Called every frame after Start().
    /// Handles loading the next scene after a delay.
    /// </summary>
	void Update () {
		if(waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

                GameManager.instance.LoadData();
                QuestManager.instance.LoadQuestData();
            }
        }
	}
}
