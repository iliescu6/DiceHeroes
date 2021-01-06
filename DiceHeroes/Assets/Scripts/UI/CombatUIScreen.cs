using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIScreen : MonoBehaviour
{
    [SerializeField] Image manaImage;
    [SerializeField] Image hpImage;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text manaText;

    public void UpdatePlayerUI(int currentMana,int currentHp,CharacterStats characterStats)
    {
        manaImage.fillAmount = (float)currentMana / (float)characterStats.mana;
        hpImage.fillAmount = (float)currentHp / (float)characterStats.health;
        hpText.text = currentHp + "/" + characterStats.health;
        manaText.text = currentMana + "/" + characterStats.mana;
    }
}
