using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu functionality, allowing players to start a new game.
/// </summary>
public class MainMenu : MonoBehaviour {

	/// <value>
    /// Name of the scene to load when starting a new game.
    /// </value>
    public string newGameScene;

	/// <summary>
    /// Starts a new game by loading the specified scene.
    /// </summary>
    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
}
