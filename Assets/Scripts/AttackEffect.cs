using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    // Duration for which the attack effect will be active
    public float effectLength;
    // Index of the sound effect to play
    public int soundEffect;

    // Called when the script is first run or when the object is instantiated
    void Start () {
        // Play the sound effect associated with this attack effect
        AudioManager.instance.PlaySFX(soundEffect);
    }
    
    // Update is called once per frame
    void Update () {
        // Destroy the attack effect game object after the effect length has elapsed
        Destroy(gameObject, effectLength);
    }
}
