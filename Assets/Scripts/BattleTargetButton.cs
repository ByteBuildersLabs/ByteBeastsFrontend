using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour {

    // Name of the move associated with this button
    public string moveName;
    // Index of the target battler this button will affect
    public int activeBattlerTarget;
    // UI Text component to display the target's name
    public Text targetName;

    // Use this for initialization
    void Start () {
        // Initialization code here, if needed
    }
    
    // Update is called once per frame
    void Update () {
        // Update logic here, if needed
    }

    // Method called when the button is pressed
    public void Press()
    {
        // Calls the PlayerAttack method in BattleManager to perform the attack
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
    }
}
