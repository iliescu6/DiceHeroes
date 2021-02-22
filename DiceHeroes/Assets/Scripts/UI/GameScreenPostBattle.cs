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
    public void Initialize(string outcome,string gold,string xp,string item,UnityAction action)
    {
        outcomeText.text = outcome;
        goldText.text = gold;
        xpText.text = xp;
        itemText.text = item;
        okButton.onClick.AddListener(action);
    }
}
