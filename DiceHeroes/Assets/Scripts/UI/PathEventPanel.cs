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
    //public delegate void OnAccept();
    //public OnAccept onAccept;
    //public delegate void OnDecline();
    //public OnDecline onDecline;
    UnityAction accept;
    UnityAction decline;

    [SerializeField] PathPiece path;//TODO delete this   

    public void Show(PathPiece path)
    {
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
        Debug.Log("U've got gold");
    }      

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

}
