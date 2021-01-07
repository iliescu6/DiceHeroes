using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public enum BattleState { Standyby, Roll, WaitEndOfRoll, PostRoll, EndRoll, CombatOutcome }

public class CombatBehaviour : MonoBehaviour
{
    public CombatUIScreen playerUI;
    public BattleState battleState = BattleState.Standyby;

    List<CharacterObject> characterObject = new List<CharacterObject>();
    PlayerProfile player;
    string currentTurn;//TODO Remove,only debug
    Button rollButton;
    string winner = "None";
    [SerializeField]
    private Transform playerSpawner;

    [SerializeField]
    GameObject combatScene;

    private List<Transform> playerSpawningPoints = new List<Transform>();
    //private List<Transform> aiSpawningPoints = new List<Transform>();
    [SerializeField]
    GameObject[] dicePrefab;
    List<Dice> dicePool = new List<Dice>();
    List<int> diceValues = new List<int>();
    bool finishedRoll = false;
    bool didDamage = false;
    public int turnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform point in playerSpawner)
        {
            playerSpawningPoints.Add(point);
        }

        characterObject = FindObjectsOfType<CharacterObject>().ToList();
        characterObject.OrderByDescending(e => e.baseCharacterStats.initiative).ToList();
        player = FindObjectOfType<PlayerProfile>();
        currentTurn = "Player";

        for (int i = 0; i < 9; i++)
        {
            GameObject d = Instantiate(dicePrefab[0], playerSpawningPoints[i].position, Quaternion.identity);
            dicePool.Add(d.GetComponent<Dice>());
            dicePool[i].spawningPoint = playerSpawningPoints[i].position;
            d.SetActive(false);
        }
        GameObject g = GameObject.Find("PlayerButton");
        rollButton = g.GetComponent<Button>();
        rollButton.onClick.AddListener(delegate { SetPlayerDicePool(player.characterObject.baseCharacterStats.dicePool); });
        rollButton.onClick.AddListener(RollDicePool);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDicesStopped();
        if (turnIndex == 1 && battleState == BattleState.Standyby)
        {
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Yes,I iz enemy");
        //TODO aici vine logica inamicului
        ActivateDices(characterObject[turnIndex].baseCharacterStats.dicePool);
        RollDicePool();
        yield return new WaitUntil(() => didDamage == true);
        UpdateTurn();
        yield return null;
    }

    //Makes them kinetic again and sets stoped bools to false 
    void ResetDices()
    {
        for (int i = 0; i < diceValues.Count; i++)
        {
            dicePool[i].ResetDice();
            diceValues[i] = 0;
        }
    }

    //Increment turn index and deactivates and activates dices based on next character(will need to be AI controlled)
    void UpdateTurn()
    {
        ResetDices();
        if (turnIndex >= characterObject.Count - 1)
        {
            turnIndex = 0;
        }
        else
        {
            turnIndex += 1;
        }
        finishedRoll = false;
        didDamage = false;
        currentTurn = "Enemy";
        SetPlayerDicePool(characterObject[turnIndex].baseCharacterStats.dicePool);
        DeactivateAllDices();
        StartCoroutine(Wait());
        if (characterObject[turnIndex].owner == "Player")
        {
            //player.RemoveDice(player.characterStats.dicePool.Count, player.combatBehaviour);
            player.characterObject.ResetDicePool();
            rollButton.onClick.AddListener(RollDicePool);
            CombatAbilityPanel.Instance.ResetAbilityButtons();
            battleState = BattleState.Standyby;
        }
        else
        {
            //RollDicePool();
            //TODO decide whether they use dices (add to editor) or deal flat dmg
            characterObject[turnIndex].baseCharacterStats.dicePool["SixSided"] = characterObject[turnIndex].baseCharacterStats.dices;
            battleState = BattleState.Standyby;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }

    void RollDicePool()
    {
        rollButton.onClick.RemoveListener(RollDicePool);
        if (battleState == BattleState.Standyby)
        {
            SetPlayerDicePool(characterObject[turnIndex].baseCharacterStats.dicePool);//TODO make it to take into account types of dices
            for (int i = 0; i < dicePool.Count; i++)
            {
                dicePool[i].RollDice();
            }
            battleState = BattleState.WaitEndOfRoll;
            SetPlayerDicePool(characterObject[turnIndex].baseCharacterStats.dicePool);
        }
        else if (battleState == BattleState.Standyby) //enemy
        {
            //SetPlayerDicePool(player.dicePool);
            for (int i = 0; i < dicePool.Count; i++)
            {
                dicePool[i].RollDice();
            }
            battleState = BattleState.WaitEndOfRoll;
            //rollButton.onClick.RemoveListener(delegate { UpdateTurn(); });
            //rollButton.onClick.AddListener(RollDicePool);
        }
    }

    void DealDamage(CharacterObject target)
    {
        for (int i = 0; i < diceValues.Count; i++)
        {
            if (target.baseCharacterStats.armour < diceValues[i])
            {
                Debug.Log(currentTurn + diceValues[i] + " and inflicted " + (diceValues[i] - target.baseCharacterStats.armour) + " points of damage.");
                target.currentHP -= (diceValues[i] - target.baseCharacterStats.armour);
                if (target.currentHP < 0)
                {
                    target.currentHP = 0;
                }
                if (target.gameObject.tag == "Player")
                {
                    playerUI.UpdatePlayerUI(player.characterObject.currentMana, player.characterObject.currentHP, player.characterObject.baseCharacterStats);
                }
                else
                {
                    target.combatStatsUI.UpdatePlayerUI(target.currentMana, target.currentHP, target.baseCharacterStats); //TODO make proper function
                }
                if (target.currentHP <= 0)
                {
                    characterObject.Remove(target);
                    CheckWinner(winner);
                }
            }
        }
        
        didDamage = true;
        rollButton.onClick.RemoveAllListeners();
    }

    void CheckWinner(string winner)
    {
        foreach (CharacterObject owner in characterObject)
        {
            if (winner == "None")
            {
                winner = owner.gameObject.tag;
            }
            else if (owner.gameObject.tag == winner)
            {
                continue;
            }
            else
            {
                winner = "None";
                break;
            }
        }
        if (winner != "None" && battleState != BattleState.CombatOutcome)
        {
            Debug.Log("Winner is :" + winner);
            battleState = BattleState.CombatOutcome;
            PostBattleScreen screen = UIScreens.PushScreen<PostBattleScreen>();
            screen.Initialize(winner, "5", "10", "Biggus Swordus", () =>
                {
                    SceneManagerTransition.Instance.sceneLoaded = false;
                    SceneManagerTransition.Instance.LoadLevelScene(combatScene);
                    UIScreens.Instance.PopScreen();
                }
            );
        }
    }

    //TODO make it a coroutine to check when dices stopped
    void CheckIfDicesStopped()
    {
        if (finishedRoll && didDamage == false && battleState == BattleState.WaitEndOfRoll && characterObject[turnIndex].gameObject.tag == "Enemy")
        {
            DealDamage(player.characterObject);
        }
        else if (finishedRoll && didDamage == false && battleState == BattleState.WaitEndOfRoll && characterObject[turnIndex].gameObject.tag == "Player")
        {
            rollButton.onClick.RemoveAllListeners();
            rollButton.onClick.AddListener(delegate { DealDamage(characterObject[1]); });
            battleState = BattleState.PostRoll;
        }
        else if (finishedRoll == false && battleState == BattleState.WaitEndOfRoll)
        {//TODO something is off but it's late for past me to think
            for (int i = 0; i < diceValues.Count;i++) // characterObject[turnIndex].baseCharacterStats.dicePool.Count; i++)
            {
                if (dicePool[i].stopped)
                {
                    diceValues[i] = dicePool[i].diceValue;
                    finishedRoll = true;
                    dicePool[i].MoveDiceToScreen();
                }
                else
                {
                    finishedRoll = false;
                    break;
                }
                //if (i == diceValues.Count - 1)//&& currentTurn == "Player")
                //{
                //    battleState = BattleState.WaitEndOfRoll; //player
                //    //rollButton.onClick.AddListener(delegate { UpdateTurn(); });
                //}
            }
        }
        else if (finishedRoll && didDamage)
        {
            //  StartCoroutine(Wait());
            UpdateTurn();
            finishedRoll = false;
            didDamage = false;
            battleState = BattleState.Standyby;
        }
    }

    void SetPlayerDicePool(Dictionary<string,int> dict)
    {
        diceValues = new List<int>();
        foreach (KeyValuePair<string, int> dicetype in dict)
        {           
            for (int i = 0; i < dicetype.Value; i++)
            {
                diceValues.Add(0);
            }
        }
    }

    public void ActivateDices(Dictionary<string, int> dict)
    {
        int diceIndex = 0;
        foreach (KeyValuePair<string, int> pair in dict)
        {
            //TODO make this work for multiple dices and change list of dices to dictionary + fix issues that will appear for sure
            for (int i = 0; i < pair.Value; i++)
            {
                dicePool[diceIndex].gameObject.SetActive(true);
                dicePool[diceIndex].transform.position = playerSpawningPoints[diceIndex].transform.position;
                diceIndex++;
            }
        }
    }

    public void DeactivateDices(KeyValuePair<string, int> pair)
    {
        int disabled = pair.Value;
        for (int i = dicePool.Count - 1; i >= 0; i--)
        {
            //TODO make it check for matching dice side type and disable
            if (dicePool[i].gameObject.activeInHierarchy)
            {
                dicePool[i].gameObject.SetActive(false);
                disabled--;
            }
            if (disabled <= 0)
            {
                break;
            }
        }
    }

    public void DeactivateAllDices()
    {
        for (int i = dicePool.Count - 1; i >= 0; i--)
        {
            if (dicePool[i].gameObject.activeInHierarchy)
            {
                dicePool[i].gameObject.SetActive(false);
            }
        }
    }

    void SetPlayerDicePool(List<int> list)
    {
        diceValues = new List<int>(list);
    }
}
