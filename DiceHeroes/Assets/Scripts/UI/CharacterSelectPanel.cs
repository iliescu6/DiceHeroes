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
    BaseCharacter selectedClass;

    private List<BaseCharacter> playerClasses=new List<BaseCharacter>();
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

    void MakeClassProfile(BaseCharacter playerClass)
    {
        //TODO instantiate prefab
        ClassText.text = playerClass.characterStats.name;
        CharacterStats stats = playerClass.characterStats;
        StatsText.text = "Health: " + stats.health + "\n Mana: " + stats.mana + "\n Armour: " + stats.armour + "\n Attrition:" + stats.attrition;
        foreach (Ability a in playerClass.characterStats.startingAbilities)
        {
            AbilityButton g = Instantiate(abilityButtonPrefab, new Vector3(0,0,0),Quaternion.identity);
            g.transform.parent = AbilityPanel.transform;
            g.Initialize(a);
        }
    }

    void GetClasses()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Characters/Classes", "*.json");
        for (int i = 0; i < files.Length; i++)
        {
            string text = File.ReadAllText(files[i]);
            CharacterStats a = new CharacterStats();
            JSON j = JSON.ParseString(text);
            a =j.Deserialize<CharacterStats>();
            BaseCharacter playerClass = new BaseCharacter();
            playerClass.characterStats = a;
            playerClasses.Add(playerClass);
            selectedClass = playerClass;
        }
    }

    public void LoadGame()
    {
        PlayerProfile.Instance.selectedClass = new BaseCharacter();
        PlayerProfile.Instance.selectedClass.characterStats = playerClasses[0].characterStats;
        PlayerProfile.Instance.characterObject.baseCharacter = new BaseCharacter();
        PlayerProfile.Instance.characterObject.baseCharacter.characterStats = playerClasses[0].characterStats;
        SceneManager.LoadScene("Level");
    }
}
