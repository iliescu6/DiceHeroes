using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button fightButton;
    [SerializeField]
    private Button inventoryButton;
    [SerializeField]
    private Button spellBookButton;
    [SerializeField]
    private Button shopButton;


   
    private GameObject characterPanelPrefab;
    [SerializeField]
    private GameObject characterPanel;

    [SerializeField]
    private GameObject inventoryPanelPrefab;
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject shopPanelPrefab;
    private GameObject shopPanel;

    [SerializeField]
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        fightButton.onClick.AddListener(ShowCharacterSelection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCharacterSelection()
    {
        characterPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("CombatScene");
    }
}
