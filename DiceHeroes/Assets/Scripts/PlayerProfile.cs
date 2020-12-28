using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : SingletonTemplate<PlayerProfile>
{
    public BaseCharacter selectedClass;
    public CharacterObject characterObject;
    private List<Ability> spellbookAbilities = new List<Ability>();

    public List<Ability> SpellbokAbilities
    {
        get { return spellbookAbilities; }
        set { spellbookAbilities = value; }
    }

    //[SerializeField]
    //public List<int> dicePool=new List<int>();

    [SerializeField]
    public CombatBehaviour combatBehaviour;

    public void UpdateStats()
    {
        //Debug.Log("HP:"+health);
        //button.transform.GetChild(0).GetComponent<Text>().text = "Name: " + enemy.name +
        //    "\n Health: " + enemy.health + "\n Armour: " + enemy.armour + "\n Damage: " + enemy.damage;
    }

    Node currentMap;

    public Node CurrentMap
    {
        get { return currentMap; }
        set { currentMap = value; }
    }
}
