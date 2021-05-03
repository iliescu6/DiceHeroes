using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameScreenPostBattle : GameScreen
{
    [SerializeField] TMP_Text outcomeText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text xpText;
    [SerializeField] TMP_Text itemText;
    [SerializeField] Button okButton;
    public void Initialize(string outcome,int gold, int xp,Equipment item,UnityAction action)
    {
        outcomeText.text = outcome;
        goldText.text = "Gold:"+gold;
        PlayerProfile.Instance.characterObject.currentGold += gold;
        xpText.text = "XP:"+xp;
        bool leveledUp=PlayerProfile.Instance.GainXP(xp);//TODO maybe add another popup or text
        itemText.text = item._name;
        PlayerProfile.Instance.inventory[0]=item;
        okButton.onClick.AddListener(action);
    }
}
