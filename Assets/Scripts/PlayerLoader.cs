using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour {

    // Reference to the player GameObject prefab
    public GameObject player;

    // Use this for initialization
    void Start () {
        // Check if an instance of PlayerController does not already exist
        if(PlayerController.instance == null)
        {
            // Instantiate the player GameObject if PlayerController does not exist
            Instantiate(player);
        }
    }
    
    // Update is called once per frame
    void Update () {
        // No updates needed for this class
    }
}
