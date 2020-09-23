using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MakeJsonFileBitch : EditorWindow
{
    public Ability ability = new Ability();
    public CharacterStats enemy = new CharacterStats();
    Vector2 scrollPos;
    int selectedIndex = -1;
    bool drawList = true;
    int tab = 0;
    string oldFileName;
    string newFileName;

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
                Ability a = JsonUtility.FromJson<Ability>(text);
                if (GUILayout.Button(a._name))
                {
                    selectedIndex = i;
                    ability = a;
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.Label("Ability", EditorStyles.boldLabel);

        //foreach (var thisVar in ability.GetType().GetProperties())
        //{
        //    thisVar = EditorGUILayout.ObjectField(thisVar);
        //}
        ability._name = EditorGUILayout.TextField("Name", ability._name);
        ability._level = EditorGUILayout.IntField("Level", ability._level);
        ability._description = EditorGUILayout.TextField("Description", ability._description);
        ability._numberOfDices = EditorGUILayout.IntField("Number of Dices", ability._numberOfDices);
        ability._diceType = EditorGUILayout.IntField("Dice Type", ability._diceType);

        if (GUILayout.Button("Create"))
        {
            string path = ability._name + ".json";
            string jsonData = JsonUtility.ToJson(ability, true);
            File.WriteAllText(Application.dataPath + "/Resources/Abilities/" + ability._name + ".json", jsonData);
            drawList = true;
        }
    }

    void DrawEnemiesTab(string characters)
    {
        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);
        List<string> files = new List<string>(Directory.GetFiles(Application.dataPath + "/Resources/Characters/"+ characters +"/ ", "*.json"));
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
                    enemy = a;
                    oldFileName = enemy.name;
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.Label("Enemies", EditorStyles.boldLabel);

        enemy.name = EditorGUILayout.TextField("Name", enemy.name);
        enemy.health = EditorGUILayout.IntField("Health", enemy.health);
        enemy.armour = EditorGUILayout.IntField("Armour", enemy.armour);
        enemy.attrition = EditorGUILayout.IntField("Damage", enemy.attrition);
        enemy.dices = EditorGUILayout.IntField("Dices", enemy.attrition);


        if (GUILayout.Button("Save"))
        {
            string path = enemy.name + ".json";
            string jsonData = JsonUtility.ToJson(enemy, true);
            if (oldFileName != enemy.name)
            {
                if (oldFileName == "")
                {
                    oldFileName = "new";
                }
                string newPath = Application.dataPath + "/Resources/Characters/"+ characters +"/ " + enemy.name + ".json";
                string oldPath = Application.dataPath + "/Resources/Characters/" + characters + "/" + oldFileName + ".json";
                AssetDatabase.RenameAsset(oldPath, newPath);
                AssetDatabase.SaveAssets();
                oldFileName = enemy.name;
            }
            else
            {
                File.WriteAllText(Application.dataPath + "/Resources/Characters/" + characters + "/" + enemy.name + ".json", jsonData);
            }
            drawList = true;
        }
        if (GUILayout.Button("New File"))
        {
            enemy = new CharacterStats();
            oldFileName = "new";
            string jsonData = JsonUtility.ToJson(enemy, true);
            System.IO.File.WriteAllText(Application.dataPath + "/Resources/Characters/" + characters + "/new.json", jsonData);
        }
        if (GUILayout.Button("Delete File"))
        {
            if (enemy.name == "")
            {
                enemy.name = "new";
            }
            File.Delete(Application.dataPath + "/Resources/Characters/" + characters + "/" + enemy.name + ".json");
        }
    }
}




