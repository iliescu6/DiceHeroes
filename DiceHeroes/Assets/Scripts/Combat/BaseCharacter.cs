using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacter : MonoBehaviour
{
    public CharacterStats characterStats;
    public string owner;
    [SerializeField]
    private TMP_Text button;
    private List<Ability> startingAbilities = new List<Ability>();
    private List<Ability> equipedAbilities = new List<Ability>();

    public List<Ability> EquipedAbilities//TODO maybe make it array, have a limit
    {
        get { return equipedAbilities; }
        set { equipedAbilities = value; }
    }

    public List<Ability> StartingAbilities
    {
        get { return startingAbilities; }
        set { startingAbilities = value; }
    }

    // Start is called before the first frame update
    virtual public void Start()
    {
        if (gameObject.tag != "Player")
        {
            SetCharacterStats();
            button.text = "Name: " + characterStats.name +
                "\n Health: " + characterStats.health + "\n Armour: " + characterStats.armour + "\n Damage: " + characterStats.attrition;
        }
    }

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

    public void AddDice(int d, CombatBehaviour combatBehaviour)
    {
        for (int i = 0; i < d; i++)
        {
            characterStats.dicePool.Add(1);
        }
        combatBehaviour.ActivateDices(characterStats.dicePool.Count);
    }

    public void RemoveDice(int d, CombatBehaviour combatBehaviour)
    {
        combatBehaviour.DeactivateDices(d);
        for (int i = 0; i < d; i++)
        {
            characterStats.dicePool.RemoveAt(0);
        }
    }
}
