using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public Equipment equipment;
    [SerializeField]
    Image iconImage;

    public void Initialize(Equipment equipment =null)
    {
        if (equipment == null || string.IsNullOrEmpty(equipment.imageGUID))
        {
            iconImage.sprite = null;
        }
        else
        {
            equipment.imageGUID = equipment.imageGUID.Replace("Assets/Resources/", "");
            equipment.imageGUID = equipment.imageGUID.Replace(".png", "");
            Sprite s = Resources.Load<Sprite>(equipment.imageGUID);
            iconImage.sprite = s;
        }
    }
}
