﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class CharacterStats
{
    public string name;
    public string description;
    public int health;
    public int mana;
    public int armour;
    public int attrition;
    public int initiative;
    public int dices;
    public Dictionary<string,int> dicePool=new Dictionary<string,int>();
    public Ability[] startingAbilities = new Ability[3];
}
