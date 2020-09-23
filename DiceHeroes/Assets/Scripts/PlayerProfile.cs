using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : SingletonTemplate<PlayerProfile>
{
    Node currentMap;

    public Node CurrentMap
    {
        get { return currentMap; }
        set { currentMap = value; }
    }
}
