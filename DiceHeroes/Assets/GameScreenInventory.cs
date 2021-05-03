using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameScreenInventory : GameScreen
{
    //todo change Gameobject with whatever class I make
    [SerializeField] UIInventoryItem inventoryPrefab;
    [SerializeField] GameObject inventoryContainer;
    GameObject[] inventoryItems = new GameObject[20];
    GameObject[] equipedItems = new GameObject[4];
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text expText;
    [SerializeField] Image xpBarImage;
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

    public void Initialize()
    {
        for (int i = 0; i < 20; i++)
        {
            UIInventoryItem g = Instantiate(inventoryPrefab, inventoryContainer.transform);
            g.Initialize(PlayerProfile.Instance.inventory[i]);
            inventoryItems[i] = g.gameObject;
        }
        CharacterObject player = PlayerProfile.Instance.characterObject;
        healthText.text = "Health:" + player.currentHP + "/" + player.baseCharacterStats.health;

        expText.text = player.currentXP + "/" + (float)player.baseCharacterStats.requiredXP[player.currentLevel - 1].requiredXP;
        xpBarImage.fillAmount = (float)player.currentXP / (float)player.baseCharacterStats.requiredXP[player.currentLevel - 1].requiredXP;
    }
}
