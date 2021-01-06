using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighlightCombatCard : MonoBehaviour
{
    //maybe we'll need to show the enemy stats in a card as well
    //then make another container card with their stats, if we'll tap and hold anything in the future, the input control might need to be added somewhere else
    [SerializeField] GameObject container;
    [SerializeField] TMP_Text manaCostText;
    [SerializeField] TMP_Text abilityNameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image abilitySprite;

    float tapTimer;
    bool selectedObject;
    CombatAbilityButton button;

    public void SetUpHighlightImage(Ability ability)
    {
        manaCostText.text = ability._manaCost.ToString();
        abilityNameText.text = ability._name;
        descriptionText.text = GetDescription(ability);
        ability.imageGUID = ability.imageGUID.Replace("Assets/Resources/", "");
        ability.imageGUID = ability.imageGUID.Replace(".png", "");
        Sprite s = Resources.Load<Sprite>(ability.imageGUID);
        abilitySprite.sprite = s;
        container.SetActive(true);
    }

    string GetDescription(Ability ability)
    {
        string description="Add: ";
        bool hasDices = false;
        foreach (KeyValuePair<string, int> pair in ability.dices)
        {
            if (pair.Value != 0)
            {
                switch (pair.Key)
                {
                    case "FourSided":
                        description += pair.Value + " d4 ";
                        break;
                    case "SixSided":
                        description += pair.Value + " d6 ";
                        break;
                    case "EightSided":
                        description += pair.Value + " d8 ";
                        break;
                    case "TenSided":
                        description += pair.Value + " d10 ";
                        break;
                    case "TwentySided":
                        description += pair.Value + " d20 ";
                        break;
                }
                hasDices = true;
            }
        }

        if (!hasDices)
        {
            description = "";
        }
        else
        {
            description += "to your dice pool.";
        }


        return description;
    }

    private void Update()
    {
        if (container.activeInHierarchy && InputController.IsButtonUp())
        {
            container.SetActive(false);
        }
        else if (InputController.IsOverUI())
        {
            if (InputController.IsButtonDown())
            {
                GameObject currentObject = InputController.GetPressedObject();
                button = currentObject.GetComponent<CombatAbilityButton>();
            }
            if (InputController.IsButtonPressed())
            {
                tapTimer += Time.deltaTime;
            }

            if (InputController.IsButtonUp())
            {
                if (tapTimer >= 0.5f)
                {
                    if (button != null)
                    {
                        SetUpHighlightImage(button.ability);
                    }
                }
                else if (tapTimer <= .1f)
                {
                    if (button != null)
                    {
                        button.SelectAbility();
                    }
                }
                tapTimer = 0;
            }
        }
    }
}
