using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameScreenInventory : GameScreen
{
    //todo change Gameobject with whatever class I make
    [SerializeField] GameObject inventoryPrefab;
    GameObject[] inventoryItems = new GameObject[20];
    GameObject[] equipedItems = new GameObject[4];
    [SerializeField] TMP_Text goldText;
    //Stats
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text armourText;
    [SerializeField] TMP_Text initiativeText;
    [SerializeField] TMP_Text atritionText;
    [SerializeField] TMP_Text fourSidedText;
    [SerializeField] TMP_Text sixSidedText;
    [SerializeField] TMP_Text eightSidedText;
    [SerializeField] TMP_Text tenSidedText;
    [SerializeField] TMP_Text twentySidedText;

    public void Initialize(CharacterObject player)
    {
        healthText.text = "Health:" + player.currentHP + "/" + player.baseCharacterStats.health;
    }
}
