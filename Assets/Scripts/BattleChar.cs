using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour {

    // Indicates if this character is controlled by the player
    public bool isPlayer;

    // Array of moves available to this character
    public string[] movesAvailable;

    // Character's attributes
    public string charName;
    public int currentHp;   // Current health points
    public int maxHP;       // Maximum health points
    public int currentMP;   // Current magic points
    public int maxMP;       // Maximum magic points
    public int strength;    // Strength attribute
    public int defence;     // Defence attribute
    public int wpnPower;    // Weapon power
    public int armrPower;   // Armor power

    // Indicates if the character has died
    public bool hasDied;

    // SpriteRenderer component to handle the character's sprite
    public SpriteRenderer theSprite;
    public Sprite deadSprite;  // Sprite to show when character is dead
    public Sprite aliveSprite; // Sprite to show when character is alive

    private bool shouldFade;   // Flag to determine if the character should fade out
    public float fadeSpeed = 1f; // Speed at which the character fades out

    // Called when the script is first initialized
    void Start () {
        // Initialization code can go here (currently unused)
    }
    
    // Called once per frame
    void Update () {
        // Check if the character should fade out
        if(shouldFade)
        {
            // Gradually change the sprite color to fade out
            theSprite.color = new Color(
                Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), // Red component
                Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), // Green component
                Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), // Blue component
                Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime)  // Alpha component (transparency)
            );

            // Check if the sprite is fully transparent
            if(theSprite.color.a == 0)
            {
                // Deactivate the game object once fully faded out
                gameObject.SetActive(false);
            }
        }
    }

    // Method to trigger the fading effect
    public void EnemyFade()
    {
        shouldFade = true;
    }
}
