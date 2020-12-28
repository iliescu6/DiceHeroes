using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacter 
{
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
        button.text = "Name: " + characterStats.name +
            "\n Health: " + characterStats.health + "\n Armour: " + characterStats.armour + "\n Damage: " + characterStats.attrition;
    }

    virtual public void SetCharacterStats()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Characters/Enemies", "*.json");
        for (int i = 0; i < files.Length; i++)
        {
            string text = File.ReadAllText(files[i]);
            CharacterStats a = JsonUtility.FromJson<CharacterStats>(text);
            characterStats = a;
        }
    }

    public void AddDice(Dictionary<string, int> dices, CombatBehaviour combatBehaviour)
    {
        foreach (KeyValuePair<string, int>dice in dices)
        {
            for (int i = 0; i < dice.Value; i++)
            {
                if (characterStats.dicePool.ContainsKey(dice.Key))
                {
                    characterStats.dicePool[dice.Key] += dice.Value;
                }
                else
                {
                    characterStats.dicePool[dice.Key]= dice.Value;
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
            characterStats.dicePool[dice.Key]-=dice.Value;

        }
    }

}
