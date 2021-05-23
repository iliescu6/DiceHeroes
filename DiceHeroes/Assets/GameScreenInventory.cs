using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameScreenInventory : GameScreen
{
    //todo change Gameobject with whatever class I make
    [SerializeField] UIInventoryItem inventoryPrefab;
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] List<EquipmentSlot> equipmentGameObject = new List<EquipmentSlot>();
    UIInventoryItem[] inventoryItems = new UIInventoryItem[20];   
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
            g.Initialize(PlayerProfile.Instance.inventory[i], equipmentGameObject, UpdatePlayerInventory);
            inventoryItems[i] = g;
        }

        for (int i = 0; i < equipmentGameObject.Count; i++)
        {
            equipmentGameObject[i].equipedItem = PlayerProfile.Instance.equipmentSlots[i];
            Equipment e = equipmentGameObject[i].equipedItem;
            e.imagePath = e.imagePath.Replace("Assets/Resources/", "");
            e.imagePath = e.imagePath.Replace(".png", "");
            Sprite s = Resources.Load<Sprite>(e.imagePath);
            equipmentGameObject[i].itemIcon.sprite = s;
        }

        CharacterObject player = PlayerProfile.Instance.characterObject;
        healthText.text = "Health:" + player.currentHP + "/" + player.baseCharacterStats.health;
        manaText.text = "Mana:" + player.currentMana + "/" + player.baseCharacterStats.mana;
        goldText.text = string.Format("Gold:{0}",(player.currentGold));
        expText.text = player.currentXP + "/" + (float)player.baseCharacterStats.requiredXP[player.currentLevel - 1].requiredXP;
        xpBarImage.fillAmount = (float)player.currentXP / (float)player.baseCharacterStats.requiredXP[player.currentLevel - 1].requiredXP;
    }

    public void UpdatePlayerInventory()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i]._equipment == null && PlayerProfile.Instance.inventory != null)
            {
                PlayerProfile.Instance.inventory[i] = null;
            }
        }
    }
}

[Serializable]
public class EquipmentSlot
{
    public EquipmentType type;
    public Equipment equipedItem;
    public Image itemIcon;
    public EquipmentSlot(int i)
    {
        type = (EquipmentType)i;
    }
}
