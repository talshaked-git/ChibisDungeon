
using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
    private const string INVENTORY_KEY = "Inventory";
    private const string EQUIPMENT_KEY = "Equipment";

    public void SaveInventory(Inventory inventory)
    {
        SaveItems(inventory.GetInventorySlots());
    }

    public void SaveEquipment(EquipmentPanel equipmentPanel)
    {
        SaveItems(equipmentPanel.GetEquipmentSlots());
    }

    private void SaveItems(IList<InventorySlot> inventorySlots)
    {
        var saveData = new InventorySaveData(inventorySlots.Count);

        for (int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            InventorySlot invSlot = inventorySlots[i];
            if (invSlot.item == null)
            {
                saveData.SavedSlots[i] = null;
            }
            else
            {
                saveData.SavedSlots[i] = new InventorySlotSaveData(invSlot);
            }
        }
    }
}
