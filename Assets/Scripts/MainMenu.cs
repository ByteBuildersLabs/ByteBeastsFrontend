using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string newGameScene;

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
}
