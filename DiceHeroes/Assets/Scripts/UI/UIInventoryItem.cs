using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public Equipment _equipment;
    [SerializeField]
    Image iconImage;
    [SerializeField]
    Button button;
    List<EquipmentGameObject> equipmentGameObject=new List<EquipmentGameObject>();

    public void Initialize(Equipment equipment, List<EquipmentGameObject> list)
    {
        equipmentGameObject = list;
        if (equipment == null || string.IsNullOrEmpty(equipment.imageGUID))
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }
        else
        {
            _equipment = equipment;
            equipment.imageGUID = equipment.imageGUID.Replace("Assets/Resources/", "");
            equipment.imageGUID = equipment.imageGUID.Replace(".png", "");
            Sprite s = Resources.Load<Sprite>(equipment.imageGUID);
            iconImage.color = new Color(1, 1, 1, 1);
            iconImage.sprite = s;
        }
        button.onClick.AddListener(EquipItem);
    }

    public void EquipItem()
    {
        if (_equipment != null)
        {
            foreach (EquipmentGameObject go in equipmentGameObject)
            {
                if ((int)go.slot.type == _equipment._slot)
                {
                    PlayerProfile.Instance.UpdateEquipment(go.slot);//TODO remove this struct because we'll use the type from equipment
                    go.slotGameobject.sprite = iconImage.sprite;
                    iconImage.sprite = null;
                    iconImage.color = new Color(1, 1, 1, 0);
                    _equipment = null;
                }
            }
        }
    }
}
