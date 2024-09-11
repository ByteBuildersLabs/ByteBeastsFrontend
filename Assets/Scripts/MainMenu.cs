using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // The name of the scene to load for a new game
    public string newGameScene;

    // Method to load the new game scene
    public void NewGame()
    {
        // Load the scene specified by the newGameScene string
        SceneManager.LoadScene(newGameScene);
    }
}
