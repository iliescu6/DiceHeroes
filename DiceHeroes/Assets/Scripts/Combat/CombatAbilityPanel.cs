using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CombatAbilityPanel : MonoBehaviour
{
    public static CombatAbilityPanel Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private CombatAbilityButton ButtonPrefab;
    List<CombatAbilityButton> abilityButtons = new List<CombatAbilityButton>();
    List<CombatAbilityButton> equipmentButton = new List<CombatAbilityButton>();
    private List<Ability> abilities=new List<Ability>();
    List<Equipment> equipment = new List<Equipment>();
    // Start is called before the first frame update
    void Start()
    {
        GetAbilities();
        Initialize();
    }

    void Initialize()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            CombatAbilityButton g=Instantiate(ButtonPrefab);
            g.Initialize(abilities[i]);
            //g.transform.GetChild(0).GetComponent<Text>().text = abilities[i].name;
            g.transform.parent = this.transform;
            abilityButtons.Add(g);
        }

        //for (int i = 0; i < equipment.Count; i++)
        //{
        //    if (!string.IsNullOrEmpty(equipment[i].imageAddress))
        //    {
        //        CombatAbilityButton g = Instantiate(ButtonPrefab);
        //        g.Initialize(equipment[i]);
        //        // g.transform.GetChild(0).GetComponent<Text>().text = abilities[i]._name;
        //        g.transform.parent = this.transform;
        //        equipmentButton.Add(g);
        //    }
        //}
    }

    public void ResetAbilityButtons()
    {
        foreach (CombatAbilityButton c in abilityButtons)
        {
            c.Selected = false;
        }
    }

    void GetAbilities()
    {
        Ability[] tempAbilities = PlayerProfile.Instance.characterObject.StartingAbilities;
        for (int i = 0; i < tempAbilities.Length; i++)
        {
            if (tempAbilities[i] != null)
            {
                abilities.Add(PlayerProfile.Instance.characterObject.StartingAbilities[i]);
            }
        }

        for (int i = 0; i < PlayerProfile.Instance.equipmentSlots.Count;i++)
        {
            equipment.Add(PlayerProfile.Instance.equipmentSlots[i]);
        }
    }
}
