using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public enum BattleState { Standyby, Roll, WaitEndOfRoll, PostRoll, EndRoll }

public class CombatBehaviour : MonoBehaviour
{
    public BattleState battleState = BattleState.Standyby;

    List<CharacterObject> characterObject=new List<CharacterObject>();
    List<BaseCharacter> characters = new List<BaseCharacter>();
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
        characterObject.OrderByDescending(e => e.baseCharacter.characterStats.initiative).ToList();
        foreach (CharacterObject co in characterObject)
        {
            characters.Add(co.baseCharacter);
        }
        characters = characters.OrderByDescending(e => e.characterStats.initiative).ToList();
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
        rollButton.onClick.AddListener(delegate { SetPlayerDicePool(player.selectedClass.characterStats.dices); });
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
        ActivateDices(characters[turnIndex].characterStats.dices);
        RollDicePool();
        //UpdateTurn();
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
        if (turnIndex >= characters.Count - 1)
        {
            turnIndex = 0;
        }
        else
        {
            turnIndex += 1;
        }
        finishedRoll = false;
        didDamage = false;
        //currentTurn = "Enemy";        
        SetPlayerDicePool(characters[turnIndex].characterStats.dices);
        DeactivateDices(dicePool.Count);
        StartCoroutine(Wait());
        if (characters[turnIndex].owner == "Player")
        {
            //player.RemoveDice(player.characterStats.dicePool.Count, player.combatBehaviour);
            player.selectedClass.characterStats.dicePool = new Dictionary<string, int>();
            rollButton.onClick.AddListener(RollDicePool);
            CombatAbilityPanel.Instance.ResetAbilityButtons();
        }
        // RollDicePool();
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
            SetPlayerDicePool(characters[turnIndex].characterStats.dicePool.Count);//TODO make it to take into account types of dices
            for (int i = 0; i < dicePool.Count; i++)
            {
                dicePool[i].RollDice();
            }
            battleState = BattleState.WaitEndOfRoll;
            SetPlayerDicePool(characters[turnIndex].characterStats.dicePool.Count);
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

    void DealDamage(BaseCharacter target)
    {
        for (int i = 0; i < diceValues.Count; i++)
        {
            if (target.characterStats.armour < diceValues[i])
            {
                Debug.Log(currentTurn + diceValues[i] + " and inflicted " + (diceValues[i] - target.characterStats.armour) + " points of damage.");               
                target.characterStats.health -= (diceValues[i] - target.characterStats.armour);
                if (target.characterStats.health < 0)
                {
                    target.characterStats.health = 0;
                }
                target.UpdateStats(); //TODO make proper function
                if (target.characterStats.health == 0)
                {
                    characters.Remove(target);
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
        if (winner != "None")
        {
            Debug.Log("Winner is :" + winner);
            SceneManagerTransition.Instance.sceneLoaded = false;
            SceneManagerTransition.Instance.LoadLevelScene(combatScene);
        }
    }

    //TODO make it a coroutine to check when dices stopped
    void CheckIfDicesStopped()
    {
        if (finishedRoll && didDamage == false && battleState == BattleState.WaitEndOfRoll && characterObject[turnIndex].gameObject.tag=="Enemy")
        {
            DealDamage(player.selectedClass);
        }
        else if (finishedRoll && didDamage == false && battleState == BattleState.WaitEndOfRoll && characterObject[turnIndex].gameObject.tag == "Player")
        {
            rollButton.onClick.RemoveAllListeners();
            rollButton.onClick.AddListener(delegate { DealDamage(characters[1]); });
            battleState = BattleState.PostRoll;
        }
        else if (finishedRoll == false && battleState == BattleState.WaitEndOfRoll)
        {
            for (int i = 0; i < characters[turnIndex].characterStats.dicePool.Count; i++)
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

    void SetPlayerDicePool(int list)
    {

        diceValues = new List<int>();
        for (int i = 0; i < list; i++)
        {
            diceValues.Add(0);
        }
    }

    public void ActivateDices(int dices)
    {
        for (int i = 0; i < dices; i++)
        {
            dicePool[i].gameObject.SetActive(true);
            dicePool[i].transform.position = playerSpawningPoints[i].transform.position;
        }
    }

    public void DeactivateDices(int dice1)
    {       
        for (int i = dicePool.Count-1; i >=0; i--)
        {
            if (dicePool[i].gameObject.activeInHierarchy)
            {
                dicePool[i].gameObject.SetActive(false);
                dice1--;
            }
            if (dice1 <= 0)
            {
                break;
            }
        }
    }

    void SetPlayerDicePool(List<int> list)
    {
        diceValues = new List<int>(list);
    }
}
