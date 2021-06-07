using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public Equipment _equipment;
    [SerializeField]
    Image iconImage;
    [SerializeField]
    Button button;
    List<EquipmentSlot> equipmentGameObject=new List<EquipmentSlot>();

    public async void Initialize(Equipment equipment, List<EquipmentSlot> list,UnityAction action)
    {
        equipmentGameObject = list;
        if (equipment == null || string.IsNullOrEmpty(equipment.imageAddress))
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }
        else
        {
            _equipment = equipment;            
            SetSprite(equipment.imageAddress);
        }
        button.onClick.AddListener(EquipItem);
        button.onClick.AddListener(action);
    }

    public void EquipItem()
    {
        if (_equipment != null)
        {
            foreach (EquipmentSlot go in equipmentGameObject)
            {
                if ((int)go.equipedItem._slot == _equipment._slot)
                {
                    PlayerProfile.Instance.UpdateEquipment(_equipment);//TODO remove this struct because we'll use the type from equipment
                    go.equipedItem = _equipment;
                    go.itemIcon.sprite = iconImage.sprite;
                    iconImage.sprite = null;
                    iconImage.color = new Color(1, 1, 1, 0);
                    _equipment = null;
                    break;
                }
            }
        }
    }

    public async void SetSprite(string address)
    {
        AssetReference test = new AssetReference(address);
        var s = test.LoadAssetAsync<Sprite>();
        await s.Task;
        iconImage.color = new Color(1, 1, 1, 1);
        iconImage.sprite = s.Result;
    }
}
