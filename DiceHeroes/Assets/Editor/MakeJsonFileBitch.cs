using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class MakeJsonFileBitch : EditorWindow
{
    public Ability ability=new Ability();
    public CharacterStats enemy=new CharacterStats();
    Vector2 scrollPos;
    int selectedIndex = -1;
    bool drawList = true;
    int tab = 0;

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
         tab = GUILayout.Toolbar(tab, new string[] { "Abilities", "Enemies","Characters"});
        switch (tab)
        {
            case 0:
                DrawAbilitiesTab();
                break;

            case 1:
                DrawEnemiesTab();
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

    void DrawEnemiesTab()
    {
        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Enemies/", "*.json");
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
                CharacterStats a = JsonUtility.FromJson<CharacterStats>(text);
                if (GUILayout.Button(a.name))
                {
                    selectedIndex = i;
                    enemy = a;
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.Label("Enemies", EditorStyles.boldLabel);

        enemy.name = EditorGUILayout.TextField("Name", enemy.name);
        enemy.health = EditorGUILayout.IntField("Health", enemy.health);
        enemy.armour = EditorGUILayout.IntField("Armour", enemy.armour);
        enemy.damage = EditorGUILayout.IntField("Damage", enemy.damage);
        enemy.dices = EditorGUILayout.IntField("Dices", enemy.damage);


        if (GUILayout.Button("Create"))
        {
            string path = enemy.name + ".json";
            string jsonData = JsonUtility.ToJson(enemy, true);
            File.WriteAllText(Application.dataPath + "/Resources/Enemies/" + enemy.name + ".json", jsonData);
            drawList = true;
        }
    }
}




