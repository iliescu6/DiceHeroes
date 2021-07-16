using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PathEventPanel : MonoBehaviour
{
    public static PathEventPanel Instance;

    [SerializeField]
    GameObject panel;
    [SerializeField]
    Button acceptButton;
    [SerializeField]
    Button declineButton;
    UnityAction accept;
    UnityAction decline;
    PathPiece currentPiece;


    public void Show(PathPiece path)
    {
        currentPiece = path;
        gameObject.SetActive(true);
        if (path.PathEventType == PathEventType.Combat)
        {
            accept = SceneManagerTransition.Instance.LoadCombatScene;
            //SceneManagerTransition.Instance.enemies=
            decline = TogglePanel;
        }
        else if (path.PathEventType == PathEventType.FreeChest)
        {
            accept = GetChestReward;
        }
        else if (path.PathEventType == PathEventType.Final)
        { 
        //Show shop to buy spells
        }

        acceptButton.onClick.AddListener(accept);
        acceptButton.onClick.AddListener(TogglePanel);
        acceptButton.onClick.AddListener(acceptButton.onClick.RemoveAllListeners);
        if (decline != null)
        {
            declineButton.onClick.AddListener(decline);
            declineButton.onClick.AddListener(TogglePanel);
            declineButton.onClick.AddListener(declineButton.onClick.RemoveAllListeners);
        }
        else
        {
            declineButton.gameObject.SetActive(false);
        }
    }

    public void GetChestReward()
    {
        PlayerProfile.Instance.characterObject.currentGold += Random.Range(5,11);
        currentPiece.chest.SetActive(false);
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

}
