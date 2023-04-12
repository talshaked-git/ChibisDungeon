using UnityEngine;
using TMPro;
using System.Text;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text ItemNameText;
    [SerializeField] TMP_Text ItemSlotsText;
    [SerializeField] TMP_Text ItemStatsText;
    [SerializeField] TMP_Text ItemLevelText;
    [SerializeField] TMP_Text ItemClassText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(Item item) {
        ItemNameText.text = item.ItemName;
        sb.Length = 0;

        if(item is EquippableItem) {
            EquippableItem equippableItem = (EquippableItem)item;
            ItemSlotsText.text = equippableItem.equipmentType.ToString();
            ItemLevelText.text = equippableItem.EquipableLV.ToString();
            ItemClassText.text = equippableItem.EquipableClass.ToString();

            AddStat(equippableItem.STRBonus,"STR");
            AddStat(equippableItem.INTBonus,"INT");
            AddStat(equippableItem.VITBonus,"VIT");
            AddStat(equippableItem.AGIBonus,"AGI");

            AddStat(equippableItem.STRPercentAddBonus,"STR",true);
            AddStat(equippableItem.INTPercentAddBonus,"INT",true);
            AddStat(equippableItem.VITPercentAddBonus,"VIT",true);
            AddStat(equippableItem.AGIPercentAddBonus,"AGI",true);

        }
        else {
            ItemSlotsText.text = "Consumable";
            ItemLevelText.text = item.EquipableLV.ToString();
            ItemClassText.text = item.EquipableClass.ToString();
        }

        ItemStatsText.text = sb.ToString();
        gameObject.SetActive(true);
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName, bool isPercentage = false) {
        if(value != 0) {
            if(sb.Length > 0) {
                sb.AppendLine();
            }

            if(value > 0) {
                sb.Append("+");
            }

            if(isPercentage) {
                sb.Append(value*100);
                sb.Append("% ");
            }
            else {
                sb.Append(value);
                sb.Append(" ");
            }

            sb.Append(statName);
        }
    }
}
