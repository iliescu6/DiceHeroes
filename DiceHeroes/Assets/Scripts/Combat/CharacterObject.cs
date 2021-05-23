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
    public int currentLevel;
    public int currentXP;
    public int currentGold;
    public string owner;
    [SerializeField]
    private TMP_Text button;
    private Ability[] startingAbilities = new Ability[4];
    private List<Ability> equipedAbilities = new List<Ability>();
    public bool player;
    public CharacterObject()
    {

    }
    public CharacterObject(CharacterStats chosenClass)
    {
        SetClassStats(chosenClass);
    }

    // Start is called before the first frame update
    void Awake()
    {

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

    virtual public void SetEnemyStats(CharacterStats character)
    {
        if (character != null)
        {
            CharacterStats a = character;
            baseCharacterStats = a;           
            this.owner = "Enemy";
            currentHP = baseCharacterStats.health;
            currentMana = baseCharacterStats.mana;
        }
    }

    virtual public void SetClassStats(CharacterStats stats)
    {
        if (stats != null)
        {
            int index = 0;
            CharacterStats a = stats;
            baseCharacterStats = a;
            foreach (string ability in baseCharacterStats.startingAbilities)
            {
                startingAbilities[index]= GameDefinitionsManager.Instance.abilityDefinitions[ability];
                index++;
            }
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
                if (baseCharacterStats.dicePool.ContainsKey(dice.Key))
                {
                    baseCharacterStats.dicePool[dice.Key] +=dice.Value;
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
