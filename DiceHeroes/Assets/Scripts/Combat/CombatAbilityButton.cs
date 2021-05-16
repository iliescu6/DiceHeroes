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
    public Dictionary<string, int> dices = new Dictionary<string, int>();
    public override void Initialize(Ability a)
    {
        base.Initialize(a);
        GameObject g = GameObject.Find("PlayerStats");
        player = GameObject.FindObjectOfType<PlayerProfile>();
        cb = GameObject.FindObjectOfType<CombatBehaviour>();
        dices = a.dices;
        //button.onClick.AddListener(SelectAbility);
    }

    public override void Initialize(Equipment a)
    {
        base.Initialize(a);
        GameObject g = GameObject.Find("PlayerStats");
        player = GameObject.FindObjectOfType<PlayerProfile>();
        cb = GameObject.FindObjectOfType<CombatBehaviour>();
        dices = a.dices;
        //button.onClick.AddListener(SelectAbility);
    }

    public void SelectAbility()
    {
        if ((ability != null && ability._manaCost <= player.characterObject.baseCharacterStats.mana)
            || (equipment != null) && Selected == false)
        {
            if (ability != null)
                player.characterObject.currentMana -= ability._manaCost;
            player.characterObject.AddDice(dices, cb);
            Selected = true;
        }
        else if (Selected == true)
        {
            if (ability != null)
                player.characterObject.currentMana += ability._manaCost;
            player.characterObject.RemoveDice(dices, cb);
            // player.characterStats.dicePool.Remove(player.characterStats.dicePool.Count);

            Selected = false;
        }
        cb.playerUI.UpdatePlayerUI(player.characterObject.currentMana, player.characterObject.currentHP, player.characterObject.baseCharacterStats);
    }
}
