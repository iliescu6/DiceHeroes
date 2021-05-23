using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerTransition : SingletonTemplate<SceneManagerTransition>
{
    public bool sceneLoaded;
    [SerializeField]
    GameObject levelScene;
    GameObject combatScene;
    public List<CharacterStats> enemies=new List<CharacterStats>();

    public void LoadCombatScene()
    {
        StartCoroutine(WaitUntilLoaded());
    }

    IEnumerator WaitUntilLoaded()
    {
        if (!sceneLoaded)
        {
            SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);
            sceneLoaded = true;
        }
        else
        {
            combatScene.gameObject.SetActive(true);
        }
        yield return new WaitUntil(() => PlayerProfile.Instance != null);
        if (PlayerProfile.Instance != null)
        {
            levelScene.SetActive(false);
        }
    }

    public void LoadLevelScene(GameObject scene)
    {
        if (combatScene == null)
        {
            combatScene = scene;
        }
        levelScene.SetActive(true);
        combatScene.SetActive(false);
    }
}
