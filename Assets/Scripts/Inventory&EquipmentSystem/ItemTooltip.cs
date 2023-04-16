using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text ItemNameText;
    [SerializeField] TMP_Text ItemTypeText;
    [SerializeField] TMP_Text ItemDescriptionText;
    [SerializeField] TMP_Text ItemLevelText;
    [SerializeField] TMP_Text ItemClassText;



    public void ShowTooltip(Item item)
    {
        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.GetItemType();
        ItemLevelText.text = item.EquipableLV.ToString();
        ItemClassText.text = item.EquipableClass.ToString();
        ItemDescriptionText.text = item.GetDescription();
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
