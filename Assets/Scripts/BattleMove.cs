using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleMove {

    // The name of the battle move (e.g., "Fireball", "Slash")
    public string moveName;

    // The power or effectiveness of the battle move
    public int movePower;

    // The cost of using this move, typically in terms of MP (Magic Points) or another resource
    public int moveCost;

    // The effect associated with this move, such as visual or sound effects
    public AttackEffect theEffect;
}
