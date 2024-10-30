using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the game over scenario, providing options to quit to the main menu or load the last saved game.
/// </summary>
public class GameOver : MonoBehaviour {

	/// <value>
    /// Name of the main menu scene to load when quitting.
    /// </value>
    public string mainMenuScene;

	/// <value>
    /// Name of the scene to load when loading the last saved game.
    /// </value>
    public string loadGameScene;

	/// <summary>
    /// Called when the script is instantiated.
    /// Sets up the game over screen by playing background music.
    /// </summary>
	void Start () {
        AudioManager.instance.PlayBGM(4);

        /* PlayerController.instance.gameObject.SetActive(false);
        GameMenu.instance.gameObject.SetActive(false);
         BattleManager.instance.gameObject.SetActive(false); */
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
    /// Quits the game to the main menu.
    /// Destroys all instances of essential game managers and loads the main menu scene.
    /// </summary>
    public void QuitToMain()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

	/// <summary>
    /// Loads the last saved game.
    /// Destroys instances of essential game managers and loads the specified scene.
    /// </summary>
    public void LoadLastSave()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
       // Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(loadGameScene);


    }
}
