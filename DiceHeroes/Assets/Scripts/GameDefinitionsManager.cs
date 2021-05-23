using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GameDefinitionsManager : SingletonTemplate<GameDefinitionsManager>
{
    public TextAsset lootTablesJson;
    public TextAsset equipmentJson;
    public TextAsset abilitiesJson;
    public TextAsset characterStatsJson;
    public TextAsset enemiesStatsJson;

    AssetReference test;
    public string guid;
    public Image icon;
    public TextAsset text;
    public Dictionary<string,LootTable> lootTablesDefinitions=new Dictionary<string, LootTable>();
    public Dictionary<string, Equipment> equipmentDefinitions = new Dictionary<string, Equipment>();
    public Dictionary<string, CharacterStats> classStatsDefinition = new Dictionary<string, CharacterStats>();
    public Dictionary<string, CharacterStats> enemiesStatsDefinitions = new Dictionary<string, CharacterStats>();
    public Dictionary<string, Ability> abilityDefinitions = new Dictionary<string, Ability>();
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        LoadGameData<LootTable>(lootTablesJson,ref lootTablesDefinitions);
        LoadGameData<Equipment>(equipmentJson, ref equipmentDefinitions);
        LoadGameData<CharacterStats>(characterStatsJson, ref classStatsDefinition);
        LoadGameData<CharacterStats>(enemiesStatsJson, ref enemiesStatsDefinitions);
        LoadGameData<Ability>(abilitiesJson, ref abilityDefinitions);
       
    }
    public void LoadGameData<T>(TextAsset gameDataArchive, ref Dictionary<string, T> dict) where T : GameDefition
    {

        string jsonInfo = gameDataArchive.text;
        JSON json = JSON.ParseString(jsonInfo);
        dict = new Dictionary<string, T>();
        dict = json.Deserialize<Dictionary<string, T>>();
    }
}
