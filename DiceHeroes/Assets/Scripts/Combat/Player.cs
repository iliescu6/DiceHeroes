using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseCharacter
{
    public static Player Instance { get; set; }

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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public void UpdateStats()
    {
        //Debug.Log("HP:"+health);
        //button.transform.GetChild(0).GetComponent<Text>().text = "Name: " + enemy.name +
        //    "\n Health: " + enemy.health + "\n Armour: " + enemy.armour + "\n Damage: " + enemy.damage;
    }
}
