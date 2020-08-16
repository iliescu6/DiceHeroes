using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAbilityButton : MonoBehaviour
{
    private Ability ability;
    private Button button;
    [SerializeField]
    private Player player;
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.Find("PlayerStats");
        player = g.GetComponent<Player>();
    }

    public bool Selected { get { return selected; } set { selected = value; } }

    public void Initialize(Ability a)
    {
        button = this.GetComponent<Button>();
        ability = a;
        button.onClick.AddListener(SelectAbility);
        selected = false;
    }

    void SelectAbility()
    {
        if (ability._manaCost <= player.Mana && selected == false)
        {
            player.AddDice(ability._numberOfDices, player.combatBehaviour);
            selected = true;
        }
        else if (selected == true)
        {
            player.Mana += ability._manaCost;
            player.RemoveDice(ability._numberOfDices, player.combatBehaviour);
            player.characterStats.dicePool.Remove(player.characterStats.dicePool.Count);

            selected = false;
        }
    }
}
