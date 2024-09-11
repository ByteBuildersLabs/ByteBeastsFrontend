using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {

    public GameObject UIScreen; // Prefab for the UI Fade component
    public GameObject player; // Prefab for the PlayerController
    public GameObject gameMan; // Prefab for the GameManager
    public GameObject audioMan; // Prefab for the AudioManager
    public GameObject battleMan; // Prefab for the BattleManager

    // Called when the script instance is being loaded
    void Awake () {
        // Check if UIFade instance is already created
        if(UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }

        // Check if PlayerController instance is already created
        if(PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        // Check if GameManager instance is already created
        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

        // Check if AudioManager instance is already created
        if(AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

        // Check if BattleManager instance is already created
        if(BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        }
    }
    
    // Called once per frame
    void Update () {
        // Update logic (if any) goes here
    }
}
