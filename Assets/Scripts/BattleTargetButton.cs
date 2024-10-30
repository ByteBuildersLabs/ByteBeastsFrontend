using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages the functionality of a button used to select a target for attacks in battle.
/// </summary>
public class BattleTargetButton : MonoBehaviour {

	/// <value>
    /// Name of the move associated with this button.
    /// </value>
    public string moveName;

	/// <value>
    /// ID of the active battler whose target this button represents.
    /// </value>
    public int activeBattlerTarget;

	/// <value>
    /// Reference to the Text component displaying the target's name.
    /// </value>
    public Text targetName;

	// Use this for initialization

	/// <summary>
    /// Initializes the BattleTargetButton component.
    /// </summary>
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
    /// Triggers the attack action against the selected target.
    /// </summary>
    public void Press()
    {
		// Call the PlayerAttack method of BattleManager with the move name and target ID
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
    }
}
