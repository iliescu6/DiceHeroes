using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PathEventPanel : MonoBehaviour
{
    public static PathEventPanel instance;

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

    private void Start()
    {
        Initialize(path);
    }

    public void Initialize(PathPiece path)
    {
        if (path.PathEventType == PathEventType.Combat)
        {
            accept = LoadCombatScene;
            decline = TogglePanel;
        }
        else if (path.PathEventType == PathEventType.FreeChest)
        {
            accept = GetChestReward;
        }

        acceptButton.onClick.AddListener(accept);
        if (decline != null)
        {
            declineButton.onClick.AddListener(decline);
        }
        else
        {
            declineButton.gameObject.SetActive(false);
        }
    }

    public void GetChestReward()
    {
        Debug.Log("U've got gold");
        TogglePanel();
    }

    public void LoadCombatScene()
    {
        SceneManager.LoadScene("CombatScene");
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

}
