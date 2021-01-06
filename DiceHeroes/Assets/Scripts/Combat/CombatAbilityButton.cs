using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatAbilityButton : AbilityButton
{

    [SerializeField]
    private PlayerProfile player;
    CombatBehaviour cb;
    float pressed = 0;
    public override void Initialize(Ability a)
    {
        base.Initialize(a);
        GameObject g = GameObject.Find("PlayerStats");
        player = GameObject.FindObjectOfType<PlayerProfile>();
        cb = GameObject.FindObjectOfType<CombatBehaviour>();
        //button.onClick.AddListener(SelectAbility);
    }

    public void SelectAbility()
    {
        if (ability._manaCost <= player.characterObject.baseCharacterStats.mana && Selected == false)
        {
            player.characterObject.currentMana -= ability._manaCost;
            player.characterObject.AddDice(ability.dices, cb);
            Selected = true;
        }
        else if (Selected == true)
        {
            player.characterObject.currentMana += ability._manaCost;
            player.characterObject.RemoveDice(ability.dices, cb);
            // player.characterStats.dicePool.Remove(player.characterStats.dicePool.Count);

            Selected = false;
        }
        cb.playerUI.UpdatePlayerUI(player.characterObject.currentMana, player.characterObject.currentHP, player.characterObject.baseCharacterStats);
    }
}
