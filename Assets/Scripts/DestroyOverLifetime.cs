using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverLifetime : MonoBehaviour {

    // Duration (in seconds) after which the game object will be destroyed
    public float lifetime;

    // Use this for initialization
    void Start () {
        // No initialization needed for this class
    }
    
    // Update is called once per frame
    void Update () {
        // Schedule the destruction of this game object after the specified lifetime
        Destroy(gameObject, lifetime);
    }
}
