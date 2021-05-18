using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LootTable : GameDefition
{
    public List<string> equipmentId = new List<string>();
    public List<int> equipmentDropChance = new List<int>();
}
