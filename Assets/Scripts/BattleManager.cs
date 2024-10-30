using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

/// <summary>
/// Manages the battle system in the game, handling combat mechanics, UI updates, and battle flow.
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the BattleManager.
    /// </summary>
    public static BattleManager instance;

    /// <summary>
    /// Flag indicating whether a battle is currently active.
    /// </summary>
    private bool battleActive;

    /// <summary>
    /// Reference to the battle scene GameObject.
    /// </summary>
    public GameObject battleScene;

    /// <summary>
    /// Array of positions for player characters.
    /// </summary>
    public Transform[] playerPositions;

    /// <summary>
    /// Array of positions for enemy characters.
    /// </summary>
    public Transform[] enemyPositions;

    /// <summary>
    /// Array of player character prefabs.
    /// </summary>
    public BattleChar[] playerPrefabs;

    /// <summary>
    /// Array of enemy character prefabs.
    /// </summary>
    public BattleChar[] enemyPrefabs;

    /// <summary>
    /// List of active battlers in the current battle.
    /// </summary>
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    /// <summary>
    /// Index of the current turn.
    /// </summary>
    public int currentTurn;

    /// <summary>
    /// Flag indicating whether the current turn is waiting for player input.
    /// </summary>
    public bool turnWaiting;

    /// <summary>
    /// Reference to the UI buttons holder GameObject.
    /// </summary>
    public GameObject uiButtonsHolder;

    /// <summary>
    /// Array of available moves.
    /// </summary>
    public BattleMove[] movesList;

    /// <summary>
    /// Reference to the enemy attack effect GameObject.
    /// </summary>
    public GameObject enemyAttackEffect;

    /// <summary>
    /// Reference to the DamageNumber prefab.
    /// </summary>
    public DamageNumber theDamageNumber;

    /// <summary>
    /// Array of Text components for displaying player names.
    /// </summary>
    public Text[] playerName;

    /// <summary>
    /// Array of Text components for displaying player HP.
    /// </summary>
    public Text[] playerHP;

    /// <summary>
    /// Array of Text components for displaying player MP.
    /// </summary>
    public Text[] playerMP;

    /// <summary>
    /// Reference to the target menu GameObject.
    /// </summary>
    public GameObject targetMenu;

    /// <summary>
    /// Array of BattleTargetButton components.
    /// </summary>
    public BattleTargetButton[] targetButtons;

    /// <summary>
    /// Reference to the magic menu GameObject.
    /// </summary>
    public GameObject magicMenu;

    /// <summary>
    /// Array of BattleMagicSelect components.
    /// </summary>
    public BattleMagicSelect[] magicButtons;

    /// <summary>
    /// Reference to the battle notification component.
    /// </summary>
    public BattleNotification battleNotice;

    /// <summary>
    /// Chance to flee from battle (in percent).
    /// </summary>
    public int chanceToFlee = 35;

    /// <summary>
    /// Flag indicating whether the character is currently fleeing.
    /// </summary>
    private bool fleeing;

    /// <summary>
    /// Name of the game over scene.
    /// </summary>
    public string gameOverScene;

    /// <summary>
    /// Experience reward for winning the battle.
    /// </summary>
    public int rewardXP;

    /// <summary>
    /// Array of item rewards for winning the battle.
    /// </summary>
    public string[] rewardItems;

    /// <summary>
    /// Flag preventing characters from fleeing.
    /// </summary>
    public bool cannotFlee;

    
    // Use this for initialization

    /// <summary>
    /// Initializes the BattleManager singleton instance.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    // Update is called once per frame

     /// <summary>
    /// Called every frame after Start().
    /// Handles various battle-related actions during gameplay.
    /// </summary>
    void Update()
    {
        // Handle test case for starting a battle
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] {"Drigan"}, false);
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
            // Handle next turn input
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    /// <summary>
    /// Starts a new battle with the given enemies.
    /// </summary>
    /// <param name="enemiesToSpawn">Array of enemy names to spawn.</param>
    /// <param name="setCannotFlee">Flag to set if players cannot flee.</param>
    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        PlayerController.instance.ActivateJoystick(false);
        if (!battleActive)
        {
            cannotFlee = setCannotFlee;

            battleActive = true;

            GameManager.instance.battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            // Spawn player characters
            for (int i = 0; i < playerPositions.Length; i++)//for no of players to spawn
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)//if the playerstats available/active
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)//for all characters available for player
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)//when the desired character prefab is found
                        {
                            Debug.Log("Spawning :" + playerPrefabs[j].charName);

                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            newPlayer.GetComponent<SpriteRenderer>().sortingOrder = playerPositions[i].GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;
                            if (playerPrefabs[j].charName != "Tim")
                            {
                                activeBattlers.Add(newPlayer);
                                int noOfBattlers = activeBattlers.Count - 1;

                                CharStats thePlayer = GameManager.instance.playerStats[i];
                                activeBattlers[noOfBattlers].currentHp = thePlayer.currentHP;
                                activeBattlers[noOfBattlers].maxHP = thePlayer.maxHP;
                                activeBattlers[noOfBattlers].currentMP = thePlayer.currentMP;
                                activeBattlers[noOfBattlers].maxMP = thePlayer.maxMP;
                                activeBattlers[noOfBattlers].strength = thePlayer.strength;
                                activeBattlers[noOfBattlers].defence = thePlayer.defence;
                                activeBattlers[noOfBattlers].wpnPower = thePlayer.wpnPwr;
                                activeBattlers[noOfBattlers].armrPower = thePlayer.armrPwr;
                                //activeBattlers[noOfBattlers].isPlayer = true;
                            }
                        }
                    }
                }
            }
            //spawning enemies
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            newEnemy.GetComponent<SpriteRenderer>().sortingOrder = enemyPositions[i].GetChild(0).GetComponent<SpriteRenderer>().sortingOrder;

                            activeBattlers.Add(newEnemy);
                            Debug.Log("Enemy Spawned");
                        }
                    }
                }
            }

            turnWaiting = true;
            currentTurn = 0;//Random.Range(0, activeBattlers.Count);

            UpdateUIStats();
        }
    }

    /// <summary>
    /// Moves to the next turn in the battle.
    /// </summary>
    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    /// <summary>
    /// Updates the battle state, handles character status changes, and determines the outcome of the battle.
    /// </summary>
    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHp < 0)
            {
                activeBattlers[i].currentHp = 0;
            }

            if (activeBattlers[i].currentHp == 0)
            {
                //Handle dead battler
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }

            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                StartCoroutine(GameOverCo());
            }

            /* battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false; */
        }
        else
        {
            while (activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    /// <summary>
    /// Coroutine that manages the enemy's turn in the battle.
    /// </summary>

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(2f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    /// <summary>
    /// Handles the enemy's attack logic.
    /// </summary>
    public void EnemyAttack()
    {
        // Create a list of player indices who are still alive
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHp > 0)
            {
                players.Add(i);
            }
        }
        // Select a random player to be the target
        int selectedTarget = players[Random.Range(0, players.Count)];

        //activeBattlers[selectedTarget].currentHp -= 30;

        // Calculate the power of the selected move
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                // Instantiate the effect of the selected move at the target position
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        // Instantiate the enemy attack effect
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    /// <summary>
    /// Calculates and applies damage to a target battler.
    /// </summary>
    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        float defPwr = activeBattlers[target].defence + activeBattlers[target].armrPower;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);

        activeBattlers[target].currentHp -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }

    /// <summary>
    /// Updates the UI to display current player and enemy stats.
    /// </summary>
    public void UpdateUIStats()
    {
        int j = 0; 

        // Update player stats
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].charName != "Tim")
            {
                BattleChar playerData = activeBattlers[i];

                playerName[j].gameObject.SetActive(true);
                playerName[j].text = playerData.charName;
                playerHP[j].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHP;
                playerMP[j].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                j++;
            }
        }

        int enemyIndex = j; 

        // Update enemy stats
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer && enemyIndex < playerHP.Length)
            {
                BattleChar enemyData = activeBattlers[i];

                playerName[enemyIndex].gameObject.SetActive(true);  
                playerName[enemyIndex].text = enemyData.charName;  
                playerHP[enemyIndex].text = Mathf.Clamp(enemyData.currentHp, 0, int.MaxValue) + "/" + enemyData.maxHP; 
                playerMP[enemyIndex].text = "";  // In case if enemies have MP

                enemyIndex++;
            }
        }

        // Hide unused UI elements
        for (int k = enemyIndex; k < playerName.Length; k++)
        {
            playerName[k].gameObject.SetActive(false); 
            playerHP[k].text = "-";  
            //playerMP[k].text = "-";  //commented because enemies don't have MP
        }
    }

    /// <summary>
    /// Handles player attacks.
    /// </summary>
    /// <param name="moveName">The name of the move being used.</param>
    /// <param name="selectedTarget">The index of the target battler.</param>
    public void PlayerAttack(string moveName, int selectedTarget)
    {
        // Find the move details
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                // Instantiate the effect of the move at the target position
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        // Instantiate the enemy attack effect
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        // Calculate and apply damage to the target
        DealDamage(selectedTarget, movePower);

        // Disable UI buttons and target menu
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        // Move to the next turn
        NextTurn();

    }

    /// <summary>
    /// Opens the target selection menu.
    /// </summary>
    /// <param name="moveName">The name of the move to be used.</param>
    public void OpenTargetMenu(string moveName)
    {
        // Activate the target menu
        targetMenu.SetActive(true);

        // Create a list of enemy indices
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        // Enable/disable target buttons based on available targets
        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHp > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Opens the magic menu.
    /// </summary>
    public void OpenMagicMenu()
    {
        // Activate the magic menu
        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                // Set the spell name and cost
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                // Find the corresponding move in the movesList and set its cost
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }

            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Attempts to flee from the current battle.
    /// </summary>
    public void Flee()
    {
        if (cannotFlee)
        {
            // Display message indicating inability to flee
            battleNotice.theText.text = "Can not flee this battle!";
            battleNotice.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                //end the battle
                //battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.theText.text = "Couldn't escape!";
                battleNotice.Activate();
            }
        }

    }

    /// <summary>
    /// Coroutine to end the current battle and reset game state.
    /// </summary>
    public IEnumerator EndBattleCo()
    {
        //Debug.Log("Saving Players");
    
        //for(int i=0; i< activeBattlers.Count; i++)
        //{
        //    if (activeBattlers[i].isPlayer)
        //    {
        //        Debug.Log("Player Found!");
        //        for (int k = 0; k < GameManager.instance.playerStats.Length; k++)
        //        {
        //            if (activeBattlers[i].charName == GameManager.instance.playerStats[k].charName)
        //            {
        //                Debug.Log("Stats Saved!" + k);
        //                GameManager.instance.playerStats[k].currentHP = activeBattlers[i].currentHp;
        //                GameManager.instance.playerStats[k].currentMP = activeBattlers[i].currentMP;
        //            }
        //        }
        //    }
        //}





        Debug.Log("Ending battle");
        
        // Deactivate battle-related UI elements
        PlayerController.instance.ActivateJoystick(true);
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        // Reset player stats and destroy battle objects
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHp;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();

        // Deactivate battle scene and reset game state
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if (fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            // Open reward screen for successful battle completion
            BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
        }

        // Play background music
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    /// <summary>
    /// Coroutine to handle game over scenario.
    /// </summary>
    public IEnumerator GameOverCo()
    {
        // Set battleActive flag to false to indicate the battle has ended
        battleActive = false;
        
        // Fade to black screen
        UIFade.instance.FadeToBlack();
        
        // Wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);
        
        // Deactivate the battle scene
        battleScene.SetActive(false);
        
        // Load the game over scene
        SceneManager.LoadScene(gameOverScene);
    }
}
