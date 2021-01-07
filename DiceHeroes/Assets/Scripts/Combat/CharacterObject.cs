using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    //TODO will need to rethink current and max values
    [SerializeField] public CombatUIScreen combatStatsUI;
    public CharacterStats baseCharacterStats;
    public int currentHP;
    public int currentMana;
    public string owner;
    [SerializeField]
    private TMP_Text button;
    private Ability[] startingAbilities = new Ability[3];
    private List<Ability> equipedAbilities = new List<Ability>();   

    public CharacterObject()
    {

    }
    public CharacterObject(string chosenClass)
    {
        SetClassStats(chosenClass);
    }

    public CharacterObject(CharacterStats newStats)
    {
        baseCharacterStats = newStats;
        currentHP = newStats.health;
        currentMana = newStats.mana;
        owner = "Player";
    }

    public string testEnemy;
    // Start is called before the first frame update
    void Awake()
    {
        if (!string.IsNullOrEmpty(testEnemy))
        {
            //TODO there must be a better way to improve seting character object and stats but am rushing to make a demo/prototype
            SetEnemyStats(testEnemy);
            currentHP = baseCharacterStats.health;
            currentMana = baseCharacterStats.mana;
        }
    }

    public List<Ability> EquipedAbilities//TODO maybe make it array, have a limit
    {
        get { return equipedAbilities; }
        set { equipedAbilities = value; }
    }

    public Ability[] StartingAbilities
    {
        get { return startingAbilities; }
        set { startingAbilities = value; }
    }

    virtual public void Initialize(string owner)
    {
        this.owner = owner;
    }

    virtual public void SetEnemyStats(string character = null)
    {
        if (character != null)
        {
            string file = File.ReadAllText(Application.dataPath + "/Resources/Characters/Enemies/" + character + ".json");
            JSON j = JSON.ParseString(file);
            CharacterStats a = j.Deserialize<CharacterStats>();
            baseCharacterStats = a;
            ResetDicePool();
            this.owner = "Enemy";
        }
    }

    virtual public void SetClassStats(string character = null)
    {
        if (character != null)
        {
            string file = File.ReadAllText(Application.dataPath + "/Resources/Characters/Classes/" + character + ".json");
            //TODO hunt down every last on Jsonutily and use json plug in for dictionaries
            JSON j = JSON.ParseString(file);
            CharacterStats a = j.Deserialize<CharacterStats>();
            baseCharacterStats = a;
            ResetDicePool();
            currentHP = baseCharacterStats.health;
            currentMana = baseCharacterStats.mana;
            this.owner = "Player";
        }
    }

    public void AddDice(Dictionary<string, int> dices, CombatBehaviour combatBehaviour)
    {
        foreach (KeyValuePair<string, int> dice in dices)
        {
            for (int i = 0; i < dice.Value; i++)
            {
                if (baseCharacterStats.dicePool.ContainsKey(dice.Key))
                {
                    baseCharacterStats.dicePool[dice.Key] += dice.Value;
                }
                else
                {
                    baseCharacterStats.dicePool[dice.Key] = dice.Value;
                }
            }
        }
        combatBehaviour.ActivateDices(baseCharacterStats.dicePool);
    }

    public void RemoveDice(Dictionary<string, int> dices, CombatBehaviour combatBehaviour)
    {

        foreach (KeyValuePair<string, int> diceType in dices)
        {

            combatBehaviour.DeactivateDices(diceType);//TODO update for future types of dice prefabs
            baseCharacterStats.dicePool[diceType.Key] -= diceType.Value;

        }
    }

    public void ResetDicePool()
    {
        baseCharacterStats.dicePool["FourSided"] = 0;
        baseCharacterStats.dicePool["SixSided"] = 0;
        baseCharacterStats.dicePool["EightSided"] = 0;
        baseCharacterStats.dicePool["TenSided"] = 0;
    }
}
