﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Ability ability;
    public Button button;
    private bool selected;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text manaCost;
    [SerializeField] Image abilityImage;

    public bool Selected { get { return selected; } set { selected = value; } }
    // Start is called before the first frame update
    public virtual void  Initialize(Ability a)
    {        
        button = this.GetComponent<Button>();
        ability = a;
        Selected = false;
        manaCost.text = a._manaCost.ToString();
        a.imageGUID = a.imageGUID.Replace("Assets/Resources/", "");
        a.imageGUID = a.imageGUID.Replace(".png", "");
        Sprite s = Resources.Load<Sprite>(a.imageGUID);
        abilityImage.sprite = s;
    }

}
