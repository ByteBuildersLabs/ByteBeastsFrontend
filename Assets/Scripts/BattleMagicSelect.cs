using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages the selection of magic spells during battles in the game.
/// </summary>
public class BattleMagicSelect : MonoBehaviour {

	/// <value>
    /// Name of the spell being selected.
    /// </value>
    public string spellName;

	/// <value>
    /// Cost of the spell in MP.
    /// </value>
    public int spellCost;

	/// <value>
    /// Reference to the Text component displaying the spell name.
    /// </value>
    public Text nameText;

	/// <value>
    /// Reference to the Text component displaying the spell cost.
    /// </value>
    public Text costText;

	// Use this for initialization

	/// <summary>
    /// Initializes the BattleMagicSelect component.
    /// </summary>
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
    /// Handles the selection of a magic spell.
    /// </summary>
    public void Press()
    {
		// Check if the player has enough MP to cast the spell
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
			// Close the magic menu and open the target menu
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            
			// Deduct the spell cost from the character's MP
			BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        } else
        {
            //let player know there is not enough MP
			// Inform the player of insufficient MP
            BattleManager.instance.battleNotice.theText.text = "Not Enough MP!";
            BattleManager.instance.battleNotice.Activate();

			// Close the magic menu
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
