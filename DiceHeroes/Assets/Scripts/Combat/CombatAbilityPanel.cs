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
    private List<Ability> abilities=new List<Ability>();
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
            g.transform.GetChild(0).GetComponent<Text>().text = abilities[i]._name;
            g.transform.parent = this.transform;
            abilityButtons.Add(g);
        }
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
        //string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Abilities/", "*.json");
        for (int i = 0; i < PlayerProfile.Instance.selectedClass.characterStats.startingAbilities.Length; i++)
        {
            //string text = File.ReadAllText(files[i]);
            //Ability a = JsonUtility.FromJson<Ability>(text);
            abilities.Add(PlayerProfile.Instance.selectedClass.characterStats.startingAbilities[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
