using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Ability : GameDefition
{
    public int _level;
    public string _description;
    public int _manaCost;
    public Dictionary<string, int> dices = new Dictionary<string, int>();
    public string imageGUID;
}

[System.Flags]
public enum DiceType { FourSided, SixSided, EightSided, TenSided }
