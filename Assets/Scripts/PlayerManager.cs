using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;

    private Player currentPlayer;

    private void Awake() {
        // currentPlayer = GameManager.instance.currentPlayer; //uncomment after tesing
        currentPlayer = new Player("TestPlayer", "1" ,CharClassType.Archer); //for testing delete later
        statPanel.SetStats(new CharcterStat(currentPlayer.level)
        ,currentPlayer.HP, currentPlayer.MP, currentPlayer.STR, currentPlayer.INT, currentPlayer.VIT, currentPlayer.AGI);
        statPanel.UpdateStatValues();
        inventory.OnItemPressEvent += EquipFromInventory;
        equipmentPanel.OnItemPressEvent += UnequipFromEquipmentPanel;
    }

    private void UnequipFromEquipmentPanel(Item item)
    {
        Unequip((EquippableItem)item);
    }

    private void EquipFromInventory(Item item) {
        if (item is EquippableItem) {
            Equip((EquippableItem)item);
        }
    }
    

    public void Equip(EquippableItem item) {
        //To do: check if item matches CHarType And Min Lv
        if(currentPlayer.classType != item.EquipableClass || currentPlayer.level < item.EquipableLV)
            return;
        if (inventory.RemoveItem(item)) {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem)) {
                if (previousItem != null) {
                    inventory.AddItem(previousItem);
                    item.Unequip(currentPlayer);
                    statPanel.UpdateStatValues();
                }
                item.Equip(currentPlayer);
                statPanel.UpdateStatValues();
            } else {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item) {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item)) {
            item.Unequip(currentPlayer);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }
}
