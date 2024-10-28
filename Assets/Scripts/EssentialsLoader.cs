using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads essential game components and initializes them as singletons.
/// </summary>
public class EssentialsLoader : MonoBehaviour {

   /// <value>
    /// GameObject representing the main UI screen.
    /// </value>
    public GameObject UIScreen;

    /// <value>
    /// GameObject representing the player controller.
    /// </value>
    public GameObject player;

    /// <value>
    /// GameObject representing the game manager.
    /// </value>
    public GameObject gameMan;

    /// <value>
    /// GameObject representing the audio manager.
    /// </value>
    public GameObject audioMan;

    /// <value>
    /// GameObject representing the battle manager.
    /// </value>
    public GameObject battleMan;

	/// <summary>
    /// Called when the script is instantiated.
    /// Initializes essential game components as singletons.
    /// </summary>
	void Awake () {
		// Ensure UIFade is initialized
		if(UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

		// Ensure PlayerController is initialized
        if(PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

		// Ensure GameManager is initialized
        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

		// Ensure AudioManager is initialized
        if(AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

		// Ensure BattleManager is initialized
        if(BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
