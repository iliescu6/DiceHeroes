using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : SingletonTemplate<PlayerProfile>
{
    public CharacterObject characterObject;
    public string selectedClassName;
    private List<Ability> spellbookAbilities = new List<Ability>();    
    public List<Equipment> equipmentSlots;
    public Equipment[] inventory;
    public LootTable currentLootTable;
    public override void Awake()
    {
        base.Awake();
        equipmentSlots = new List<Equipment>(4);
        for (int i = 0; i < Enum.GetNames(typeof(EquipmentType)).Length; i++)
        {
            equipmentSlots.Add(new Equipment(i));
        }
        inventory = new Equipment[20];
    }
    public List<Ability> SpellbokAbilities
    {
        get { return spellbookAbilities; }
        set { spellbookAbilities = value; }
    }

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

    public bool GainXP(int xp)
    {
        characterObject.currentXP += xp;
        if (characterObject.baseCharacterStats.requiredXP!=null && characterObject.currentXP >= characterObject.baseCharacterStats.requiredXP[characterObject.currentLevel].requiredXP)
        {
            characterObject.currentLevel++;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void UpdateEquipment(Equipment slot)
    {
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            if (equipmentSlots[i]._slot == slot._slot)
            {
                equipmentSlots[i] = slot;
            }
        }
    }
}

