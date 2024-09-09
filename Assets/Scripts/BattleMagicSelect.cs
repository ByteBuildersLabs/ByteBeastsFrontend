using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour {

    // Name of the spell associated with this selection
    public string spellName;
    
    // MP cost required to cast the spell
    public int spellCost;

    // UI Text components for displaying the spell name and cost
    public Text nameText;
    public Text costText;

    // Called when the script is first initialized
    void Start () {
        // Initialization code can go here (currently unused)
    }
    
    // Called once per frame
    void Update () {
        // Update logic can go here (currently unused)
    }

    // Called when this magic selection is pressed
    public void Press()
    {
        // Check if the current battler has enough MP to cast the spell
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            // Hide the magic menu
            BattleManager.instance.magicMenu.SetActive(false);
            
            // Open the target selection menu for the spell
            BattleManager.instance.OpenTargetMenu(spellName);
            
            // Deduct the MP cost from the current battler's MP
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        } 
        else
        {
            // Notify the player that they do not have enough MP
            BattleManager.instance.battleNotice.theText.text = "Not Enough MP!";
            BattleManager.instance.battleNotice.Activate();
            
            // Hide the magic menu
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
