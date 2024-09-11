using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

  // Singleton instance of the BattleManager
  public static BattleManager instance;

  // Boolean to track if a battle is currently active
  private bool battleActive;

  // Reference to the battle scene GameObject
  public GameObject battleScene;

  // Positions on the battlefield for players and enemies
  public Transform[] playerPositions;
  public Transform[] enemyPositions;

  // Prefabs for player and enemy battle characters
  public BattleChar[] playerPrefabs;
  public BattleChar[] enemyPrefabs;

  // List of all active characters in the battle
  public List<BattleChar> activeBattlers = new List<BattleChar>();

  // Tracks which character's turn it currently is
  public int currentTurn;
  public bool turnWaiting;

  // UI elements for player actions (e.g., attack, magic)
  public GameObject uiButtonsHolder;

  // List of possible moves characters can perform
  public BattleMove[] movesList;

  // Effect prefab for enemy attacks
  public GameObject enemyAttackEffect;

  // Reference to the damage number display
  public DamageNumber theDamageNumber;

  // Text elements to display player's name, HP, and MP
  public Text[] playerName, playerHP, playerMP;

  // Menus for selecting target and magic
  public GameObject targetMenu;
  public BattleTargetButton[] targetButtons;
  public GameObject magicMenu;
  public BattleMagicSelect[] magicButtons;

  // Notification system for battle messages
  public BattleNotification battleNotice;

  // Chance to flee from the battle
  public int chanceToFlee = 35;
  private bool fleeing;

  // Scene to load when the player is defeated
  public string gameOverScene;

  // Experience points and rewards gained after battle
  public int rewardXP;
  public string[] rewardItems;

  // Prevents fleeing if set to true
  public bool cannotFlee;

  // Initialization method
  void Start()
  {
    instance = this;  // Set the instance for the singleton pattern
    DontDestroyOnLoad(gameObject);  // Persist this object across scenes
  }

  // Called once per frame
  void Update()
  {
    // For testing, pressing 'T' starts a battle with an "Eyeball" enemy
    if (Input.GetKeyDown(KeyCode.T))
    {
      BattleStart(new string[] { "Eyeball" }, false);
    }

    // If a battle is active
    if (battleActive)
    {
      // If the battle is waiting for the current character's turn
      if (turnWaiting)
      {
        // If it's a player's turn, show the action buttons
        if (activeBattlers[currentTurn].isPlayer)
        {
          uiButtonsHolder.SetActive(true);
        }
        // Otherwise, let the enemy take its turn automatically
        else
        {
          uiButtonsHolder.SetActive(false);
          StartCoroutine(EnemyMoveCo());  // Coroutine for enemy action
        }
      }

      // For debugging, pressing 'N' moves to the next turn
      if (Input.GetKeyDown(KeyCode.N))
      {
        NextTurn();
      }
    }
  }
  public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
  {
    // Only start the battle if one is not already active
    if (!battleActive)
    {
      // Set whether fleeing is allowed for this battle
      cannotFlee = setCannotFlee;

      // Mark the battle as active
      battleActive = true;

      // Notify the GameManager that a battle has started
      GameManager.instance.battleActive = true;

      // Position the BattleManager at the camera's position
      transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

      // Activate the battle scene GameObject to show the battle on screen
      battleScene.SetActive(true);

      // Play the battle background music (BGM)
      AudioManager.instance.PlayBGM(0);

      // Loop through the player positions and instantiate player characters
      for (int i = 0; i < playerPositions.Length; i++)
      {
        if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
        {
          // Find the corresponding player prefab and instantiate it in the player's position
          for (int j = 0; j < playerPrefabs.Length; j++)
          {
            if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
            {
              BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
              newPlayer.transform.parent = playerPositions[i]; // Parent the new player to the corresponding position
              activeBattlers.Add(newPlayer); // Add the player to the list of active battlers

              // Set the player's stats in the battle according to the player's CharStats in the GameManager
              CharStats thePlayer = GameManager.instance.playerStats[i];
              activeBattlers[i].currentHp = thePlayer.currentHP;
              activeBattlers[i].maxHP = thePlayer.maxHP;
              activeBattlers[i].currentMP = thePlayer.currentMP;
              activeBattlers[i].maxMP = thePlayer.maxMP;
              activeBattlers[i].strength = thePlayer.strength;
              activeBattlers[i].defence = thePlayer.defence;
              activeBattlers[i].wpnPower = thePlayer.wpnPwr;
              activeBattlers[i].armrPower = thePlayer.armrPwr;
            }
          }
        }
      }

      // Loop through the enemy list and instantiate enemy characters
      for (int i = 0; i < enemiesToSpawn.Length; i++)
      {
        if (enemiesToSpawn[i] != "")
        {
          // Find the corresponding enemy prefab and instantiate it in the enemy's position
          for (int j = 0; j < enemyPrefabs.Length; j++)
          {
            if (enemyPrefabs[j].charName == enemiesToSpawn[i])
            {
              BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
              newEnemy.transform.parent = enemyPositions[i]; // Parent the new enemy to the corresponding position
              activeBattlers.Add(newEnemy); // Add the enemy to the list of active battlers
            }
          }
        }
      }

      // Set up the first turn of the battle
      turnWaiting = true;
      currentTurn = Random.Range(0, activeBattlers.Count); // Randomly choose who goes first

      // Update the UI to reflect the current stats of the characters
      UpdateUIStats();
    }
  }

  public void NextTurn()
  {
    // Move to the next character's turn
    currentTurn++;
    if (currentTurn >= activeBattlers.Count)
    {
      currentTurn = 0; // Loop back to the first character if the end of the list is reached
    }

    // Set turnWaiting to true to wait for the next character to take action
    turnWaiting = true;

    // Update the battle status and UI
    UpdateBattle();
    UpdateUIStats();
  }

  public void UpdateBattle()
  {
    // Check if all enemies or all players are dead
    bool allEnemiesDead = true;
    bool allPlayersDead = true;

    for (int i = 0; i < activeBattlers.Count; i++)
    {
      // Ensure no battler has negative HP
      if (activeBattlers[i].currentHp < 0)
      {
        activeBattlers[i].currentHp = 0;
      }

      // Handle characters with 0 HP
      if (activeBattlers[i].currentHp == 0)
      {
        if (activeBattlers[i].isPlayer)
        {
          // Set the player's sprite to the dead sprite
          activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
        }
        else
        {
          // Fade out the enemy when they die
          activeBattlers[i].EnemyFade();
        }
      }
      else
      {
        if (activeBattlers[i].isPlayer)
        {
          allPlayersDead = false; // If a player is still alive, mark that not all players are dead
          activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
        }
        else
        {
          allEnemiesDead = false; // If an enemy is still alive, mark that not all enemies are dead
        }
      }
    }

    // If all enemies or players are dead, end the battle
    if (allEnemiesDead || allPlayersDead)
    {
      if (allEnemiesDead)
      {
        // End the battle with a victory for the player
        StartCoroutine(EndBattleCo());
      }
      else
      {
        // End the battle with a game over for the player
        StartCoroutine(GameOverCo());
      }
    }
    else
    {
      // If the current character is dead, skip their turn
      while (activeBattlers[currentTurn].currentHp == 0)
      {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
          currentTurn = 0; // Loop back to the first character if the end of the list is reached
        }
      }
    }
  }

  public IEnumerator EnemyMoveCo()
  {
    // Disable player input while the enemy is taking its turn
    turnWaiting = false;

    // Wait for 1 second before the enemy attacks
    yield return new WaitForSeconds(1f);

    // Perform the enemy attack
    EnemyAttack();

    // Wait for another 1 second before moving to the next turn
    yield return new WaitForSeconds(1f);

    // Proceed to the next turn
    NextTurn();
  }

  public void EnemyAttack()
  {
    // Find all players that are still alive (HP > 0)
    List<int> players = new List<int>();
    for (int i = 0; i < activeBattlers.Count; i++)
    {
      if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0)
      {
        players.Add(i); // Add the index of the alive player to the list
      }
    }

    // Randomly select one of the alive players as the target
    int selectedTarget = players[Random.Range(0, players.Count)];

    // Randomly choose one of the enemy's available moves
    int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
    int movePower = 0;

    // Find the move in the moves list to retrieve its power and effect
    for (int i = 0; i < movesList.Length; i++)
    {
      if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
      {
        // Instantiate the move's effect at the selected target's position
        Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
        movePower = movesList[i].movePower; // Get the power of the selected move
      }
    }

    // Play the enemy's attack effect
    Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

    // Deal damage to the selected target based on the move's power
    DealDamage(selectedTarget, movePower);
  }

  public void DealDamage(int target, int movePower)
  {
    // Calculate the attacking battler's attack power (strength + weapon power)
    float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;

    // Calculate the defending battler's defense power (defense + armor power)
    float defPwr = activeBattlers[target].defence + activeBattlers[target].armrPower;

    // Calculate the damage with a small random variation (between 90% and 110%)
    float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
    int damageToGive = Mathf.RoundToInt(damageCalc); // Round the damage to an integer

    // Log the damage dealt for debugging
    Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

    // Reduce the target's HP by the calculated damage
    activeBattlers[target].currentHp -= damageToGive;

    // Instantiate the damage number effect at the target's position to display the damage
    Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

    // Update the UI to reflect the new HP values
    UpdateUIStats();
  }

  public void UpdateUIStats()
  {
    // Loop through all the player UI elements to update their stats display
    for (int i = 0; i < playerName.Length; i++)
    {
      if (activeBattlers.Count > i)
      {
        if (activeBattlers[i].isPlayer)
        {
          // Get the data for the current player and display their stats
          BattleChar playerData = activeBattlers[i];

          playerName[i].gameObject.SetActive(true);
          playerName[i].text = playerData.charName;
          playerHP[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHP;
          playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
        }
        else
        {
          // If it's not a player, hide the UI element
          playerName[i].gameObject.SetActive(false);
        }
      }
      else
      {
        // If there are fewer active battlers than UI elements, hide the extra UI
        playerName[i].gameObject.SetActive(false);
      }
    }
  }

  public void PlayerAttack(string moveName, int selectedTarget)
  {
    // Find the move in the moves list and apply its effect
    int movePower = 0;
    for (int i = 0; i < movesList.Length; i++)
    {
      if (movesList[i].moveName == moveName)
      {
        // Instantiate the move's effect at the selected target's position
        Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
        movePower = movesList[i].movePower; // Get the power of the selected move
      }
    }

    // Play the player's attack effect
    Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

    // Deal damage to the selected target based on the move's power
    DealDamage(selectedTarget, movePower);

    // Disable the attack buttons and the target selection menu after the attack
    uiButtonsHolder.SetActive(false);
    targetMenu.SetActive(false);

    // Proceed to the next turn
    NextTurn();
  }

  // Opens the target menu when a move is selected
  public void OpenTargetMenu(string moveName)
  {
    targetMenu.SetActive(true); // Activate the target menu

    // Find all enemies (non-player battlers)
    List<int> Enemies = new List<int>();
    for (int i = 0; i < activeBattlers.Count; i++)
    {
      if (!activeBattlers[i].isPlayer)
      {
        Enemies.Add(i); // Add enemy index to the list
      }
    }

    // Display target buttons for enemies that are still alive
    for (int i = 0; i < targetButtons.Length; i++)
    {
      if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHp > 0)
      {
        targetButtons[i].gameObject.SetActive(true); // Show the button

        targetButtons[i].moveName = moveName; // Assign move name to the button
        targetButtons[i].activeBattlerTarget = Enemies[i]; // Assign target index
        targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName; // Display enemy name
      }
      else
      {
        targetButtons[i].gameObject.SetActive(false); // Hide button if there are no enemies or enemy is dead
      }
    }
  }

  // Opens the magic menu to display available spells for the current turn
  public void OpenMagicMenu()
  {
    magicMenu.SetActive(true); // Activate the magic menu

    // Display magic buttons based on available moves
    for (int i = 0; i < magicButtons.Length; i++)
    {
      if (activeBattlers[currentTurn].movesAvailable.Length > i)
      {
        magicButtons[i].gameObject.SetActive(true); // Show button

        magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i]; // Assign spell name
        magicButtons[i].nameText.text = magicButtons[i].spellName; // Display spell name

        // Find the spell cost from the move list and display it
        for (int j = 0; j < movesList.Length; j++)
        {
          if (movesList[j].moveName == magicButtons[i].spellName)
          {
            magicButtons[i].spellCost = movesList[j].moveCost; // Set spell cost
            magicButtons[i].costText.text = magicButtons[i].spellCost.ToString(); // Display spell cost
          }
        }
      }
      else
      {
        magicButtons[i].gameObject.SetActive(false); // Hide unused buttons
      }
    }
  }

  // Handles the flee action during battle
  public void Flee()
  {
    if (cannotFlee)
    {
      // Display message if fleeing is not allowed
      battleNotice.theText.text = "Can not flee this battle!";
      battleNotice.Activate();
    }
    else
    {
      // Random chance to flee
      int fleeSuccess = Random.Range(0, 100);
      if (fleeSuccess < chanceToFlee)
      {
        // Successfully flee the battle and end it
        fleeing = true;
        StartCoroutine(EndBattleCo());
      }
      else
      {
        // Failed to flee, continue the battle
        NextTurn();
        battleNotice.theText.text = "Couldn't escape!";
        battleNotice.Activate();
      }
    }
  }

  // Ends the battle and handles cleanup
  public IEnumerator EndBattleCo()
  {
    battleActive = false; // Mark the battle as inactive
    uiButtonsHolder.SetActive(false); // Hide UI buttons
    targetMenu.SetActive(false); // Hide target menu
    magicMenu.SetActive(false); // Hide magic menu

    yield return new WaitForSeconds(.5f); // Short delay

    UIFade.instance.FadeToBlack(); // Fade out screen

    yield return new WaitForSeconds(1.5f); // Wait for fade

    // Restore player stats after battle
    for (int i = 0; i < activeBattlers.Count; i++)
    {
      if (activeBattlers[i].isPlayer)
      {
        for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
        {
          if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
          {
            // Update player stats in the game manager
            GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHp;
            GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
          }
        }
      }

      // Destroy the battler objects
      Destroy(activeBattlers[i].gameObject);
    }

    UIFade.instance.FadeFromBlack(); // Fade in screen
    battleScene.SetActive(false); // Hide battle scene
    activeBattlers.Clear(); // Clear the list of battlers
    currentTurn = 0; // Reset the turn counter

    // Check if the player fled or won the battle
    if (fleeing)
    {
      GameManager.instance.battleActive = false; // Mark the battle as over
      fleeing = false; // Reset fleeing status
    }
    else
    {
      // Show battle rewards if the battle was won
      BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
    }

    // Resume background music
    AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
  }

  // Handles game over sequence
  public IEnumerator GameOverCo()
  {
    battleActive = false; // Mark battle as inactive
    UIFade.instance.FadeToBlack(); // Fade out the screen

    yield return new WaitForSeconds(1.5f); // Wait for fade

    battleScene.SetActive(false); // Hide the battle scene
    SceneManager.LoadScene(gameOverScene); // Load the game over scene
  }
}
