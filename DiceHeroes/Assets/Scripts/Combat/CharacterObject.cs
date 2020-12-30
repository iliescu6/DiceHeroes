using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public CharacterObject(string chosenClass)
    {
        SetClassStats(chosenClass);
    }

    public CharacterObject()
    {

    }

    public string testEnemy;
    // Start is called before the first frame update
    void Awake()
    {
        if (!string.IsNullOrEmpty(testEnemy))
        {
            SetCharacterStats(testEnemy);
        }
    }

    public CharacterStats characterStats;
    public string owner;
    [SerializeField]
    private TMP_Text button;
    private Ability[] startingAbilities = new Ability[3];
    private List<Ability> equipedAbilities = new List<Ability>();

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

    // Start is called before the first frame update
    //virtual public void Start()
    //{
    //    if (gameObject.tag != "Player")
    //    {
    //        SetCharacterStats();
    //        button.text = "Name: " + characterStats.name +
    //            "\n Health: " + characterStats.health + "\n Armour: " + characterStats.armour + "\n Damage: " + characterStats.attrition;
    //    }
    //}

    // Update is called once per frame
    virtual public void Update()
    {

    }

    virtual public void Initialize(string owner)
    {
        this.owner = owner;
    }

    public virtual void UpdateStats()
    {
        //button.text = "Name: " + characterStats.name +
        //    "\n Health: " + characterStats.health + "\n Armour: " + characterStats.armour + "\n Damage: " + characterStats.attrition;
    }

    virtual public void SetCharacterStats(string character = null)
    {
        if (character != null)
        {
            string file = File.ReadAllText(Application.dataPath + "/Resources/Characters/Enemies/" + character + ".json");
            CharacterStats a = JsonUtility.FromJson<CharacterStats>(file);
            characterStats = a;

        }
    }

    virtual public void SetClassStats(string character = null)
    {
        if (character != null)
        {
            string file = File.ReadAllText(Application.dataPath + "/Resources/Characters/Classes/" + character + ".json");
            CharacterStats a = JsonUtility.FromJson<CharacterStats>(file);
            characterStats = a;

        }
    }

    public void AddDice(Dictionary<string, int> dices, CombatBehaviour combatBehaviour)
    {
        foreach (KeyValuePair<string, int> dice in dices)
        {
            for (int i = 0; i < dice.Value; i++)
            {
                if (characterStats.dicePool.ContainsKey(dice.Key))
                {
                    characterStats.dicePool[dice.Key] += dice.Value;
                }
                else
                {
                    characterStats.dicePool[dice.Key] = dice.Value;
                }
            }
        }
        combatBehaviour.ActivateDices(characterStats.dicePool.Count);
    }

    public void RemoveDice(Dictionary<string, int> dices, CombatBehaviour combatBehaviour)
    {

        foreach (KeyValuePair<string, int> dice in dices)
        {
            combatBehaviour.DeactivateDices(dice.Value);//TODO update for future types of dice prefabs
            characterStats.dicePool[dice.Key] -= dice.Value;

        }
    }
}
