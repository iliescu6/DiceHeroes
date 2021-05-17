using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GameDefinitionsManager : SingletonTemplate<GameDefinitionsManager>
{
    public AssetReference lootTablesDefinitions;
    public AssetReference equipmentJson;
    public AssetReference abilitiesJson;
    public AssetReference characterStatsJson;
    
    public string guid;
    public Image icon;
    public TextAsset text;
    public Dictionary<string, Equipment> equipmentDefinitions = new Dictionary<string, Equipment>();
    public Dictionary<string, CharacterStats> charactersStatsDefinition = new Dictionary<string, CharacterStats>();
    public Dictionary<string, Ability> abilityDefinitions = new Dictionary<string, Ability>();
    // Start is called before the first frame update
    private void Awake()
    {
         LoadGameData(equipmentJson);
    }
    private async Task LoadGameData(AssetReference gameDataArchive)
    {
        AssetReference test= new AssetReference(guid);
        var s= test.LoadAssetAsync<Sprite>();
        await s.Task;
        icon.sprite = s.Result;
        var archiveTask = gameDataArchive.LoadAssetAsync<Sprite>();
        await archiveTask.Task;
        icon.sprite = archiveTask.Result;

        string json = gameDataArchive.editorAsset.name;
        //_gameManager.GameDefinitions.LoadDefinitionsFromString(json);
    }
}
