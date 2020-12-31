using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Leguar.TotalJSON;

public class MakeJsonFileBitch : EditorWindow
{
    public Ability ability = new Ability();
    public List<Ability> abilities = new List<Ability>();
    public Ability[] selectedAbilities = new Ability[3];
    public List<string> abilitiesNames = new List<string>();
    public CharacterStats characterStats = new CharacterStats();
    Vector2 scrollPos;
    int selectedIndex = -1;
    int tab = 0;
    bool pickedNewDefinition = false;
    string oldFileName;
    string newFileName;
    int[] startingAbilityIndex = new int[3];
    Object obj;
    Dictionary<string, int> diceType = new Dictionary<string, int>();


    [MenuItem("Tools/JSon Editor")]
    public static void SuckIt()
    {
        EditorWindow.GetWindow(typeof(MakeJsonFileBitch));
    }

    [MenuItem("Tools/JSon Editor Close")]
    public static void FuckOff()
    {
        EditorWindow.GetWindow(typeof(MakeJsonFileBitch)).Close();
    }

    void OnGUI()
    {

        tab = GUILayout.Toolbar(tab, new string[] { "Abilities", "Enemies", "Characters" });
        switch (tab)
        {
            case 0:
                DrawAbilitiesTab();
                break;

            case 1:
                DrawEnemiesTab("Enemies");
                break;
            case 2:
                DrawEnemiesTab("Classes");
                break;
            default:
                break;
        }
    }

    void SetAbilities()
    {
        abilities = new List<Ability>();
        abilitiesNames = new List<string>();
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Abilities/", "*.json");
        foreach (string file in files)
        {
            string text = File.ReadAllText(file);
            JSON j = JSON.ParseString(text);
            Ability a = j.Deserialize<Ability>();
            abilities.Add(a);
            abilitiesNames.Add(a._name);
        }
    }

    //TODO Generalize it, sleepy atm
    void DrawAbilitiesTab()
    {

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Abilities/", "*.json");
        if (files.Length != 0)
        {
            EditorGUILayout.BeginHorizontal();
            scrollPos =
                EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(100));

            itemStyle.alignment = TextAnchor.MiddleLeft;
            itemStyle.active.background = itemStyle.normal.background;
            itemStyle.margin = new RectOffset(0, 0, 0, 0);



            for (int i = 0; i < files.Length; i++)
            {
                string text = File.ReadAllText(files[i]);
                GUI.backgroundColor = (selectedIndex == i) ? Color.gray : Color.clear;
                //Ability a = JsonUtility.FromJson<Ability>(text);
                JSON j = JSON.ParseString(text);
                Ability a = j.Deserialize<Ability>();
                if (GUILayout.Button(a._name))
                {
                    selectedIndex = i;
                    ability = a;
                    diceType = new Dictionary<string, int>();
                    if (!string.IsNullOrEmpty(ability.imageGUID))
                    {
                        obj = AssetDatabase.LoadAssetAtPath(ability.imageGUID, typeof(Texture2D));
                    }
                    else
                    {
                        obj = null;
                    }
                    if (ability.dices.Count > 0)
                    {
                        diceType.Add("FourSided", ability.dices["FourSided"]);
                        diceType.Add("SixSided", ability.dices["SixSided"]);
                        diceType.Add("EightSided", ability.dices["EightSided"]);
                        diceType.Add("TenSided", ability.dices["TenSided"]);
                    }
                    else
                    {
                        diceType.Add("FourSided", 0);
                        diceType.Add("SixSided", 0);
                        diceType.Add("EightSided", 0);
                        diceType.Add("TenSided", 0);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.Label("Ability", EditorStyles.boldLabel);

        ability._name = EditorGUILayout.TextField("Name", ability._name);
        ability._level = EditorGUILayout.IntField("Level", ability._level);
        ability._manaCost = EditorGUILayout.IntField("Mana Cost", ability._manaCost);
        ability._description = EditorGUILayout.TextField("Description", ability._description);

        obj = EditorGUILayout.ObjectField(obj, typeof(Texture2D), false);
        if (diceType.Count > 0)
        {
            diceType["FourSided"] = EditorGUILayout.IntField("Four Sided", diceType["FourSided"]); ;
            diceType["SixSided"] = EditorGUILayout.IntField("Six Sided", diceType["SixSided"]); ;
            diceType["EightSided"] = EditorGUILayout.IntField("Eight Sided", diceType["EightSided"]); ;
            diceType["TenSided"] = EditorGUILayout.IntField("Ten Sided", diceType["TenSided"]); ;
        }
        else
        {
            diceType = new Dictionary<string, int>();
            diceType.Add("FourSided", 0);
            diceType.Add("SixSided", 0);
            diceType.Add("EightSided", 0);
            diceType.Add("TenSided", 0);
        }


        if (GUILayout.Button("Create"))
        {
            string path = ability._name + ".json";
            ability.dices = diceType;
            ability.imageGUID = AssetDatabase.GetAssetPath(obj);
            //AssetDatabase.AssetPathToGUID();
            string jsonData = JSON.Serialize(ability).CreateString();
            //string jsonData = JsonUtility.ToJson(ability, true);
            File.WriteAllText(Application.dataPath + "/Resources/Abilities/" + ability._name + ".json", jsonData);

            //Make this for enemies as well
            //List<string> classes = new List<string>(Directory.GetFiles(Application.dataPath + "/Resources/Characters/Classes/", "*.json"));
            //foreach (string s in classes)
            //{
            //    string classInfo = File.ReadAllText(s);
            //    if (classInfo.Contains(ability._name))
            //    {
            //        CharacterStats character = JsonUtility.FromJson<CharacterStats>(classInfo);
            //        for (int a=0;a<3; a++)
            //        {
            //            if (character.startingAbilities[a]._name == ability._name)
            //            {
            //                character.startingAbilities[a] = ability;
            //            }
            //        }
            //        string jsonCharacter = JsonUtility.ToJson(character, true);
            //        File.WriteAllText(Application.dataPath + "/Resources/Characters/Classes/" + characterStats.name + ".json", jsonData);
            //    }             
            //}
        }
    }

    void DrawEnemiesTab(string characters)
    {
        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);
        List<string> files = new List<string>(Directory.GetFiles(Application.dataPath + "/Resources/Characters/" + characters + "/ ", "*.json"));
        if (files.Count != 0)
        {
            EditorGUILayout.BeginHorizontal();
            scrollPos =
                EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(100));

            itemStyle.alignment = TextAnchor.MiddleLeft;
            itemStyle.active.background = itemStyle.normal.background;
            itemStyle.margin = new RectOffset(0, 0, 0, 0);



            for (int i = 0; i < files.Count; i++)
            {
                string text = File.ReadAllText(files[i]);
                GUI.backgroundColor = (selectedIndex == i) ? Color.gray : Color.clear;
                CharacterStats a = JsonUtility.FromJson<CharacterStats>(text);
                if (GUILayout.Button(a.name))
                {
                    selectedIndex = i;
                    characterStats = a;
                    oldFileName = characterStats.name;
                    pickedNewDefinition = true;
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.Label("Enemies", EditorStyles.boldLabel);

        characterStats.name = EditorGUILayout.TextField("Name", characterStats.name);
        characterStats.health = EditorGUILayout.IntField("Health", characterStats.health);
        characterStats.mana = EditorGUILayout.IntField("Mana", characterStats.mana);
        characterStats.armour = EditorGUILayout.IntField("Armour", characterStats.armour);
        characterStats.attrition = EditorGUILayout.IntField("Damage", characterStats.attrition);
        characterStats.dices = EditorGUILayout.IntField("Dices", characterStats.attrition);

        //Make only for classes and save values
        if (characters == "Classes")
        {
            SetAbilities();

            if (pickedNewDefinition)
            {
                foreach (string s in abilitiesNames)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (characterStats.startingAbilities[i] != null && characterStats.startingAbilities[i]._name == s)
                        {
                            startingAbilityIndex[i] = EditorGUILayout.Popup(abilitiesNames.IndexOf(s), abilitiesNames.ToArray(), EditorStyles.toolbarPopup);
                        }
                    }
                }
                pickedNewDefinition = false;
            }
            for (int j = 0; j < startingAbilityIndex.Length; j++)
            {
                startingAbilityIndex[j] = EditorGUILayout.Popup(startingAbilityIndex[j], abilitiesNames.ToArray(), EditorStyles.toolbarPopup);
            }
        }
        if (GUILayout.Button("Save"))
        {
            string jsonData = "";
            if (characters == "Classes")
            {
                CharacterObject playerClass = new CharacterObject();
                playerClass.characterStats = characterStats;
                for (int i = 0; i < 3; i++)
                {
                    playerClass.characterStats.startingAbilities[i] = abilities[startingAbilityIndex[i]];
                }

                jsonData = JSON.Serialize(playerClass.characterStats).CreateString(); ;// JsonUtility.ToJson(playerClass.characterStats, true);
            }
            else
            {
                jsonData = JsonUtility.ToJson(characterStats, true);
            }
            if (oldFileName != characterStats.name)
            {
                if (oldFileName == "")
                {
                    oldFileName = "new";
                }
                string newPath = Application.dataPath + "/Resources/Characters/" + characters + "/ " + characterStats.name + ".json";
                string oldPath = Application.dataPath + "/Resources/Characters/" + characters + "/" + oldFileName + ".json";
                AssetDatabase.RenameAsset(oldPath, newPath);
                AssetDatabase.SaveAssets();
                oldFileName = characterStats.name;
            }
            else
            {
                File.WriteAllText(Application.dataPath + "/Resources/Characters/" + characters + "/" + characterStats.name + ".json", jsonData);
            }
        }
        if (GUILayout.Button("New File"))
        {
            characterStats = new CharacterStats();
            oldFileName = "new";
            string jsonData = JsonUtility.ToJson(characterStats, true);
            System.IO.File.WriteAllText(Application.dataPath + "/Resources/Characters/" + characters + "/new.json", jsonData);
        }
        if (GUILayout.Button("Delete File"))
        {
            if (characterStats.name == "")
            {
                characterStats.name = "new";
            }
            File.Delete(Application.dataPath + "/Resources/Characters/" + characters + "/" + characterStats.name + ".json");
        }
    }
}




