using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public string mainMenuScene; // Scene name for the main menu
    public string loadGameScene; // Scene name for loading the last save

    // Called when the script instance is being loaded
    void Start () {
        AudioManager.instance.PlayBGM(4); // Play background music for the game over screen

        /* Commented out code that could be used to deactivate game components
        PlayerController.instance.gameObject.SetActive(false);
        GameMenu.instance.gameObject.SetActive(false);
        BattleManager.instance.gameObject.SetActive(false); */
    }
    
    // Called once per frame
    void Update () {
        // Update logic (if any) goes here
    }

    // Method to quit to the main menu
    public void QuitToMain()
    {
        // Destroy game-related instances to reset the game state
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuScene);
    }

    // Method to load the last saved game
    public void LoadLastSave()
    {
        // Destroy game-related instances to reset the game state
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        // BattleManager instance is not destroyed here, presumably to preserve battle state

        // Load the last saved game scene
        SceneManager.LoadScene(loadGameScene);
    }
}
