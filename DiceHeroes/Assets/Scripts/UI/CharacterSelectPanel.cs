using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Leguar.TotalJSON;

public class CharacterSelectPanel : MonoBehaviour
{
    [SerializeField]
    TMP_Text ClassText;
    [SerializeField]
    TMP_Text StatsText;
    [SerializeField]
    Image CharacterPortraitImage;
    [SerializeField]
    RectTransform AbilityPanel;
    [SerializeField]
    Button SelectButton;
    [SerializeField]
    AbilityButton abilityButtonPrefab;
    CharacterObject selectedClass;

    private List<CharacterObject> playerClasses=new List<CharacterObject>();
    // Start is called before the first frame update
    void Start()
    {
        GetClasses();
        Initialize();
    }

    void Initialize()
    {
       // for (int i = 0; i < playerClasses.Count; i++)
       // {
            MakeClassProfile(playerClasses[0]);
        //}
    }

    void MakeClassProfile(CharacterObject playerClass)
    {
        //TODO instantiate prefab
        ClassText.text = playerClass.baseCharacterStats.name;
        CharacterStats stats = playerClass.baseCharacterStats;
        StatsText.text = "Health: " + stats.health + "\n Mana: " + stats.mana + "\n Armour: " + stats.armour + "\n Attrition:" + stats.attrition;
        foreach (string a in playerClass.baseCharacterStats.startingAbilities)
        {
            AbilityButton g = Instantiate(abilityButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            g.transform.parent = AbilityPanel.transform;
            g.Initialize(GameDefinitionsManager.Instance.abilityDefinitions[a]);
        }
    }

    void GetClasses()
    {
        Dictionary<string, CharacterStats> dict = new Dictionary<string, CharacterStats>();
        dict = GameDefinitionsManager.Instance.classStatsDefinition;
        foreach (KeyValuePair<string,CharacterStats> pair in dict)
        {
            CharacterObject playerClass = new CharacterObject();
            playerClass.baseCharacterStats = pair.Value;
            playerClasses.Add(playerClass);
            selectedClass = playerClass;
            //TODO add to constructor and unify enemy and classes
            selectedClass.ResetDicePool();
        } 
    }

    public void LoadGame()
    {
        //TODO something's off here(mana and hp are not set up in get classes properly)
        PlayerProfile.Instance.characterObject.SetClassStats(selectedClass.baseCharacterStats);
        SceneManager.LoadScene("Level");
    }
}
