using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Ability ability;
    public Equipment equipment;
    public Button button;
    private bool selected;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text manaCost;
    [SerializeField] Image abilityImage;

    public bool Selected { get { return selected; } set { selected = value; } }
    // Start is called before the first frame update
    public async virtual Task Initialize(Ability a)
    {
        button = this.GetComponent<Button>();
        ability = a;
        Selected = false;
        manaCost.text = a._manaCost.ToString();
        AssetReference test = new AssetReference(a.imageAddress);
        var s = test.LoadAssetAsync<Sprite>();
         await s.Task;
        abilityImage.sprite = s.Result;
        //abilityImage.sprite = s;
    }

    public async virtual Task Initialize(Equipment a)
    {
        button = this.GetComponent<Button>();
        equipment = a;
        Selected = false;
        manaCost.gameObject.SetActive(false);
        AssetReference test = new AssetReference(a.imageAddress);
        var s = test.LoadAssetAsync<Sprite>();
        await s.Task;
        abilityImage.sprite = s.Result;
    }

}
