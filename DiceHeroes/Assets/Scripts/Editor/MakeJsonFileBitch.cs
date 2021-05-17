using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Leguar.TotalJSON;
using System.Reflection;
using System;

public class MakeJsonFileBitch : EditorWindow
{
    public const string classesPath = "D:/Unity Projects/Resources/Characters/Classes/";
    public const string enemiesPath = "";
    public const string equipmentPathFinal = "Equipment.json";
    public const string equipmentPath = "";
    public const string abilitiesPath = "";
    public const string ignoreString = "imageGUID";

    Dictionary<string, Equipment> equipmentDict = new Dictionary<string, Equipment>();
    Dictionary<string, Ability> abilitiesDict = new Dictionary<string, Ability>();

    public Ability ability = new Ability();
    public Equipment equipment = new Equipment();
    public LootTable lootTable = new LootTable();
    public List<Ability> abilities = new List<Ability>();
    public Ability[] selectedAbilities = new Ability[3];
    public List<string> abilitiesNames = new List<string>();
    public CharacterStats characterStats = new CharacterStats();
    List<FieldInfo> listOfFields = new List<FieldInfo>();
    Vector2 scrollPos;
    int selectedIndex = -1;
    int tab = 0;
    bool pickedNewDefinition = false;
    string oldFileName;
    string newFileName;
    int[] startingAbilityIndex = new int[3];
    List<int> genericXIndex = new List<int>();
    List<string> genericString = new List<string>();

    List<Equipment> equipmentList = new List<Equipment>();
    List<LootDrops> localDrops = new List<LootDrops>();
    List<FileInfo> fileInfos = new List<FileInfo>();
    List<string> stringList = new List<string>();
    UnityEngine.Object obj;
    Dictionary<string, int> diceType = new Dictionary<string, int>();
    GameDefition temp;

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

        tab = GUILayout.Toolbar(tab, new string[] { "Abilities", "Equipment", "LootTables", "Enemies", "Characters" });
        switch (tab)
        {
            case 0:
                DrawAbilitiesTab();
                break;
            case 1:
                DrawEquipmentTab();
                break;
            case 2:
                DrawDropTableTab();
                break;
            case 3:
                DrawEnemiesTab("Enemies");
                break;
            case 4:
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
            abilitiesNames.Add(a.name);
        }
    }

    void DrawDropTableTab()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/LootTables/", "*.json");
        if (files.Length != 0)
        {
            EditorGUILayout.BeginHorizontal();
            scrollPos =
                EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(100));


            for (int i = 0; i < files.Length; i++)
            {
                string text = File.ReadAllText(files[i]);
                GUI.backgroundColor = (selectedIndex == i) ? Color.gray : Color.clear;
                JSON j = JSON.ParseString(text);
                LootTable a = j.Deserialize<LootTable>();
                if (GUILayout.Button(a._name))
                {
                    selectedIndex = i;
                    lootTable = a;
                    DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Equipment/");
                    stringList = new List<string>();
                    genericXIndex = new List<int>();
                    equipmentList = new List<Equipment>();
                    fileInfos = dir.GetFiles("*.*").ToList();
                    foreach (FileInfo fi in fileInfos)
                    {
                        if (!fi.ToString().Contains("meta"))
                        {
                            stringList.Add(fi.ToString());
                        }
                    }
                    foreach (Equipment e in lootTable.equipment)
                    {
                        equipmentList.Add(e);
                    }

                    pickedNewDefinition = true;
                    foreach (Equipment e in equipmentList)
                    {
                        genericXIndex.Add(0);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }

        GUILayout.Label("Loot Tables", EditorStyles.boldLabel);

        DrawFields<LootTable>(lootTable);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Loot Table"))
        {
            equipmentList.Add(new Equipment());
            genericXIndex.Add(0);
        }
        if (GUILayout.Button("Remove last Loot table"))
        {
            equipmentList.Remove(equipmentList[equipmentList.Count - 1]);
            genericXIndex.Remove(genericXIndex[genericXIndex.Count - 1]);
        }
        EditorGUILayout.EndHorizontal();

        ResetGenerics(equipmentList);
        DrawDropDown(equipmentList, stringList, genericXIndex);

        if (GUILayout.Button("Save"))
        {
            string path = lootTable._name + ".json";

            for (int i = 0; i < equipmentList.Count; i++) //TODO name used to be path, will reset all equipments in editor to dagger_of_pop due to name not containing path anymore,fix
            {
                string text = File.ReadAllText(equipmentList[i].name);
                JSON j = JSON.ParseString(text);
                Equipment a = j.Deserialize<Equipment>();
                equipmentList[i] = a;
            }
            lootTable.equipment = equipmentList;
            string jsonData = JSON.Serialize(lootTable).CreateString();
            File.WriteAllText(Application.dataPath + "/Resources/LootTables/" + lootTable._name + ".json", jsonData);
        }

        if (GUILayout.Button("Delete File"))
        {
            if (lootTable._name == "")
            {
                lootTable._name = "new";
            }
            File.Delete(Application.dataPath + "/Resources/LootTables/" + lootTable._name + ".json");
        }
    }

    public void DrawGeneric<T>(string path,ref Dictionary<string, T> dict,ref T temp) where T : GameDefition
    {
        string s = File.ReadAllText(path);
        EditorGUILayout.BeginHorizontal();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(100));

        if (string.IsNullOrEmpty(s))
        {
            dict = new Dictionary<string, T>();
        }
        else
        {
            JSON j = JSON.ParseString(s);
            dict = new Dictionary<string, T>();
            dict = j.Deserialize<Dictionary<string, T>>();
        }
        int inter = 0;
        foreach (KeyValuePair<string, T> equip in dict)
        {
            if (GUILayout.Button(equip.Value.name))
            {
                pickedNewDefinition = true;
                selectedIndex = inter;
                temp = equip.Value;
                diceType = new Dictionary<string, int>();
                
                //SetUpDiceTypes(temp.dices);
                inter++;
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    void DrawEquipmentTab()
    {
        string s = File.ReadAllText(Application.dataPath + "/Resources/Equipments.json"); //Don't use resource.load to modify a json cuz it won't load it proeperly after saving,needs editor to reload...fml
        DrawGeneric<Equipment>(Application.dataPath + "/Resources/Equipments.json",ref equipmentDict,ref equipment);
        SetUpDiceTypes(equipment.dices);
        if (!string.IsNullOrEmpty(equipment.imageGUID) && pickedNewDefinition)
        {
            obj = AssetDatabase.LoadAssetAtPath(equipment.imageGUID, typeof(Texture2D));
        }
        else if(pickedNewDefinition)
        {
            obj = null;
            pickedNewDefinition = false;
        }

        GUILayout.Label("Equipment", EditorStyles.boldLabel);

        DrawFields<Equipment>(equipment);

        obj = EditorGUILayout.ObjectField(obj, typeof(Texture2D), false);
        DrawDiceTypesInputFields();

        if (GUILayout.Button("Add Entry"))
        {
            equipment = new Equipment();
            equipment.id = Guid.NewGuid().ToString();
            equipment.name = "New Entry";
            equipmentDict[equipment.id] = equipment;
            string jsonData = JSON.Serialize(equipmentDict).CreateString();
            File.WriteAllText(Application.dataPath + "/Resources/Equipments.json", jsonData);
        }

        if (GUILayout.Button("Save"))
        {
            equipment.dices = diceType;
            equipment.imageGUID = AssetDatabase.GetAssetPath(obj);
            Debug.Log(equipment.bonusMana);
            if (string.IsNullOrEmpty(equipment.id))
            {
                equipment.id = Guid.NewGuid().ToString();
            }
            equipmentDict[equipment.id] = equipment;
            string jsonData = JSON.Serialize(equipmentDict).CreateString();
            File.WriteAllText(Application.dataPath + "/Resources/Equipments.json", jsonData);
        }

        if (GUILayout.Button("Delete File"))
        {
            if (equipment.name == "")
            {
                equipment.name = "new";
            }
            File.Delete(Application.dataPath + "/Resources/Equipment/" + equipment.name + ".json");
        }
    }

    //TODO Generalize it, sleepy atm
    void DrawAbilitiesTab()
    {
       // if (Directory.Exists(Application.dataPath + "/Resources/Abilities.json"))
        //{
            string s = File.ReadAllText(Application.dataPath + "/Resources/Abilities.json");
        //}
        //else
        //{
        //    string jsonData = JSON.Serialize(abilitiesDict).CreateString();
        //    File.WriteAllText(Application.dataPath + "/Resources/Abilities.json", jsonData);
        //}
        DrawGeneric<Ability>(Application.dataPath + "/Resources/Abilities.json",ref abilitiesDict, ref ability);
        SetUpDiceTypes(ability.dices);
        if (!string.IsNullOrEmpty(ability.imageGUID) && pickedNewDefinition)
        {
            obj = AssetDatabase.LoadAssetAtPath(ability.imageGUID, typeof(Texture2D));
        }
        else if (pickedNewDefinition)
        {
            obj = null;
            pickedNewDefinition = false;
        }
        //string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Abilities/", "*.json");
        //if (files.Length != 0)
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    scrollPos =
        //        EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(100));


        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        string text = File.ReadAllText(files[i]);
        //        GUI.backgroundColor = (selectedIndex == i) ? Color.gray : Color.clear;
        //        JSON j = JSON.ParseString(text);
        //        Ability a = j.Deserialize<Ability>();
        //        if (GUILayout.Button(a.name))
        //        {
        //            selectedIndex = i;
        //            ability = a;
        //            diceType = new Dictionary<string, int>();
        //            if (!string.IsNullOrEmpty(ability.imageGUID))
        //            {
        //                obj = AssetDatabase.LoadAssetAtPath(ability.imageGUID, typeof(Texture2D));
        //            }
        //            else
        //            {
        //                obj = null;
        //            }
        //            SetUpDiceTypes(ability.dices);
        //        }
        //    }
        //    EditorGUILayout.EndScrollView();
        //    EditorGUILayout.EndHorizontal();

        //}
        GUILayout.Label("Ability", EditorStyles.boldLabel);

        DrawFields<Ability>(ability);

        obj = EditorGUILayout.ObjectField(obj, typeof(Texture2D), false);
        DrawDiceTypesInputFields();

        if (GUILayout.Button("Add Entry"))
        {
            ability = new Ability();
            ability.id = Guid.NewGuid().ToString();
            ability.name = "New Entry";
            abilitiesDict[ability.id] = ability;
            string jsonData = JSON.Serialize(abilitiesDict).CreateString();
            File.WriteAllText(Application.dataPath + "/Resources/Abilities.json", jsonData);
        }

        if (GUILayout.Button("Save"))
        {
            ability.dices = diceType;
            ability.imageGUID = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(ability.id))
            {
                ability.id = Guid.NewGuid().ToString();
            }
            abilitiesDict[ability.id] = ability;
            string jsonData = JSON.Serialize(abilitiesDict).CreateString();
            File.WriteAllText(Application.dataPath + "/Resources/Abilities.json", jsonData);
        }
        //DrawNewItemButton<Ability>(ability, Application.dataPath + "/Resources/Abilities/");
        //if (GUILayout.Button("Save"))
        //{
        //    string path = ability.name + ".json";
        //    ability.dices = diceType;
        //    ability.imageGUID = AssetDatabase.GetAssetPath(obj);
        //    string jsonData = JSON.Serialize(ability).CreateString();
        //    File.WriteAllText(Application.dataPath + "/Resources/Abilities/" + ability.name + ".json", jsonData);
        //}
        if (GUILayout.Button("Delete File"))
        {
            if (ability.name == "")
            {
                ability.name = "new";
            }
            File.Delete(Application.dataPath + "/Resources/Abilities/" + ability.name + ".json");
        }
    }

    void DrawEnemiesTab(string characters)
    {
        List<string> files = new List<string>(Directory.GetFiles(Application.dataPath + "/Resources/Characters/" + characters + "/ ", "*.json"));
        if (files.Count != 0)
        {
            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(100));

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
                    diceType = new Dictionary<string, int>();
                    localDrops = new List<LootDrops>(characterStats.lootDrops);

                    DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Equipment/");
                    stringList = new List<string>();
                    fileInfos = dir.GetFiles("*.*").ToList();
                    foreach (FileInfo fi in fileInfos)
                    {
                        stringList.Add(fi.ToString());
                    }
                    if (characterStats != null)
                    {
                        SetUpDiceTypes(characterStats.dicePool);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.Label("Enemies", EditorStyles.boldLabel);

        DrawFields<CharacterStats>(characterStats);

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
                        if (characterStats.startingAbilities[i] != null && characterStats.startingAbilities[i].name == s)
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
        else
        {
            DrawDiceTypesInputFields();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Drop Loot"))
            {
                localDrops.Add(new LootDrops());
            }
            if (GUILayout.Button("Remove last Drop Loot"))
            {
                localDrops.Remove(localDrops[localDrops.Count - 1]);
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < localDrops.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                selectedIndex = EditorGUILayout.Popup(selectedIndex, stringList.ToArray());
                localDrops[i].equipmentName = stringList[selectedIndex];
                localDrops[i].chanceToDrop = EditorGUILayout.FloatField(localDrops[i].chanceToDrop);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (GUILayout.Button("Save"))
        {
            string jsonData = "";
            if (characters == "Classes")
            {
                CharacterObject playerClass = new CharacterObject();
                playerClass.baseCharacterStats = characterStats;
                for (int i = 0; i < 3; i++)
                {
                    playerClass.baseCharacterStats.startingAbilities[i] = abilities[startingAbilityIndex[i]];
                }

                jsonData = JSON.Serialize(playerClass.baseCharacterStats).CreateString(); ;// JsonUtility.ToJson(playerClass.characterStats, true);
            }
            else
            {
                characterStats.dicePool = diceType;
                jsonData = JSON.Serialize(characterStats).CreateString();
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
        DrawNewItemButton<CharacterStats>(characterStats, Application.dataPath + "/Resources/Characters/" + characters);
        if (GUILayout.Button("Delete File"))
        {
            if (characterStats.name == "")
            {
                characterStats.name = "new";
            }
            File.Delete(Application.dataPath + "/Resources/Characters/" + characters + "/" + characterStats.name + ".json");
        }
    }

    public void ResetGenerics<T>(List<T> listOfTypes)
    {
        genericString = new List<string>();

        foreach (T generic in listOfTypes)
        {
            listOfFields = typeof(T).GetFields().ToList();
            foreach (FieldInfo f in listOfFields)
            {
                if (f.Name == "_name")
                {
                    //genericString.Add(f.GetValue(generic));
                    genericString.Add((string)(generic.GetType().GetField(f.Name).GetValue(generic)));
                }
            }
        }
    }

    public void DrawDropDown<T>(List<T> objectList, List<string> listOfNames, List<int> genericIndex)
    {
        List<string> currentNames = new List<string>();
        if (pickedNewDefinition)
        {
            foreach (T generic in objectList)
            {
                listOfFields = typeof(T).GetFields().ToList();
                foreach (FieldInfo f in listOfFields)
                {
                    if (f.Name == "_name")
                    {
                        currentNames.Add((string)(generic.GetType().GetField(f.Name).GetValue(generic)));
                    }
                }
            }
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList != null && listOfNames.Contains(currentNames[i]))
                {
                    genericIndex[i] = EditorGUILayout.Popup(listOfNames.IndexOf(currentNames[i]), listOfNames.ToArray(), EditorStyles.toolbarPopup);
                    objectList[i].GetType().GetField("_name").SetValue(objectList[i], listOfNames[listOfNames.IndexOf(currentNames[i])]);
                }
            }
            pickedNewDefinition = false;
        }
        for (int j = 0; j < objectList.Count; j++)
        {
            genericIndex[j] = EditorGUILayout.Popup(genericIndex[j], listOfNames.ToArray(), EditorStyles.toolbarPopup);
            objectList[j].GetType().GetField("_name").SetValue(objectList[j], listOfNames[genericIndex[j]]);
        }
    }

    public void DrawNewItemButton<T>(T item, string path) where T : new()
    {
        if (GUILayout.Button("New File"))
        {
            item = new T();
            oldFileName = "new";
            string jsonData = JsonUtility.ToJson(item, true);
            System.IO.File.WriteAllText(path + "/new.json", jsonData);
        }
    }

    //public void NewItemButton<T>(T item,Dictionary<string,T>dict,string path)
    //{
    //    if (GUILayout.Button("Add Entry"))
    //    {
    //        item = default;
    //        item.id = Guid.NewGuid().ToString();
    //        item.name = "New Entry";
    //        dict[item.id] = item;
    //        string jsonData = JSON.Serialize(equipmentDict).CreateString();
    //        File.WriteAllText(path, jsonData);
    //    }
    //}


    public void SetUpDiceTypes(Dictionary<string, int> dict)
    {
        if (dict.Count > 0 && diceType.Count<1)
        {
            diceType.Add("FourSided", dict["FourSided"]);
            diceType.Add("SixSided", dict["SixSided"]);
            diceType.Add("EightSided", dict["EightSided"]);
            diceType.Add("TenSided", dict["TenSided"]);
        }
        else if(diceType.Count<1)
        {
            diceType.Add("FourSided", 0);
            diceType.Add("SixSided", 0);
            diceType.Add("EightSided", 0);
            diceType.Add("TenSided", 0);
        }
    }

    public void DrawDiceTypesInputFields()
    {
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
    }

    public void DrawFields<T>(T obj)
    {
        listOfFields = typeof(T).GetFields().ToList();

        for (int i = 0; i < listOfFields.Count; i++)
        {
            if (listOfFields[i].FieldType == typeof(string))
            {
                if (listOfFields[i].Name != ignoreString)
                {
                    string s = (string)(obj.GetType().GetField(listOfFields[i].Name).GetValue(obj));
                    s = EditorGUILayout.TextField(listOfFields[i].Name, s);
                    obj.GetType().GetField(listOfFields[i].Name).SetValue(obj, s);
                }
            }
            else if (listOfFields[i].FieldType == typeof(int))
            {
                int s = (int)(obj.GetType().GetField(listOfFields[i].Name).GetValue(obj));
                s = EditorGUILayout.IntField(listOfFields[i].Name, s);
                obj.GetType().GetField(listOfFields[i].Name).SetValue(obj, s);
            }
        }
    }

    
}


