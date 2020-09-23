using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseCharacter
{
    private List<Ability> spellbookAbilities = new List<Ability>();

    public List<Ability> SpellbokAbilities
    {
        get { return spellbookAbilities; }
        set { spellbookAbilities = value; }
    }

    public static Player Instance { get; set; }

    public override void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //[SerializeField]
    //public List<int> dicePool=new List<int>();

    [SerializeField]
    public CombatBehaviour combatBehaviour;

    [SerializeField]
    private int armour;

    public int Armour { get { return armour; } set { armour = value; } }

    [SerializeField]
    private int health;

    public int Health { get { return health; } set { health = value; } }

    [SerializeField]
    private int mana;

    public int Mana { get { return mana; } set { mana = value; } }

    public void UpdateStats()
    {
        //Debug.Log("HP:"+health);
        //button.transform.GetChild(0).GetComponent<Text>().text = "Name: " + enemy.name +
        //    "\n Health: " + enemy.health + "\n Armour: " + enemy.armour + "\n Damage: " + enemy.damage;
    }
}
