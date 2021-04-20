using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPanelsController : MonoBehaviour
{
    [SerializeField]
    PathEventPanel pathEventPanel;
    [SerializeField]
    public GameObject scene;

    public static WorldPanelsController Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPathEventPanel(PathPiece p)
    {
        pathEventPanel.Show(p);
    }

    public void ToggleInventoryScreen()
    {
        GameScreenInventory screen = UIScreens.PushScreen<GameScreenInventory>();
        screen.Initialize();
    }
}
