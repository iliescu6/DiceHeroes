using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class Equipment 
{
    public string imageGUID;
    public string _name;
    public int _level;
    public int _slot;
    public string _description;
    public int bonusHealth;
    public int bonusMana;
    public int bonusAttrition;
    public int bonusArmour;
    public int bonusInitiative;
    public Dictionary<string, int> dices = new Dictionary<string, int>();
   
}

[Serializable]
public enum EquipmentType { Head,Armour,Weapon}
