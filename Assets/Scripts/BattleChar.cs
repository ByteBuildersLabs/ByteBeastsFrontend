using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script manages the behavior and appearance of battle characters in the game.
/// </summary>
public class BattleChar : MonoBehaviour {

	/// <value>
    /// Indicates whether this character is controlled by the player.
    /// </value>
    public bool isPlayer;

	/// <value>
    /// Array of move names available to this character.
    /// </value>
    public string[] movesAvailable;

	/// <value>
    /// Name of the character.
    /// </value>
    public string charName;

    public int currentHp, maxHP, currentMP, maxMP, strength, defence, wpnPower, armrPower;
    public bool hasDied;

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

	/// <value>
    /// Flag indicating whether the character should fade out.
    /// </value>
    private bool shouldFade;

	/// <value>
    /// Speed of the fade effect.
    /// </value>
    public float fadeSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	/// <summary>
    /// Initializes the BattleChar component.
    /// </summary>

	/// <summary>
    /// Called every frame after Start().
    /// Handles fading out the character when it dies.
    /// </summary>
	void Update () {
		if(shouldFade)
        {
			// Gradually decrease alpha of the sprite
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
			
			// Hide the character when fully faded out
            if(theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

	/// <summary>
    /// Initiates the fading-out process for enemy characters.
    /// </summary>
    public void EnemyFade()
    {
        shouldFade = true;
    }
}
