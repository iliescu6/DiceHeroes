using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAbilityButton : AbilityButton
{

    [SerializeField]
    private PlayerProfile player;
    CombatBehaviour cb;

    public override void Initialize(Ability a)
    {
        base.Initialize(a);
        GameObject g = GameObject.Find("PlayerStats");
        player = GameObject.FindObjectOfType<PlayerProfile>();
        cb = GameObject.FindObjectOfType<CombatBehaviour>();
        button.onClick.AddListener(SelectAbility);
    }

    void SelectAbility()
    {
        if (ability._manaCost <= player.characterObject.characterStats.mana && Selected == false)
        {
            player.characterObject.AddDice(ability.dices, cb);
            Selected = true;
        }
        else if (Selected == true)
        {
            player.characterObject.characterStats.mana += ability._manaCost;
            player.characterObject.RemoveDice(ability.dices, cb);
            // player.characterStats.dicePool.Remove(player.characterStats.dicePool.Count);

            Selected = false;
        }
    }
}
