
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
    public static ItemSaveManager Instance { get; private set; }
    public ItemDatabase ItemDatabase;
    private const string INVENTORY_KEY = "Inventory";
    private int inventoryMaxSize;
    private const string EQUIPMENT_KEY = "Equipment";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (ItemDatabase == null)
        {
            ItemDatabase = Resources.Load<ItemDatabase>("Items/ItemDatabase");
        }
    }

    public InventorySaveData SaveInventory(Inventory inventory)
    {
        inventoryMaxSize = inventory.MaxSlots;
        return SaveItems(inventory.GetInventorySlots());
    }

    public InventorySaveData SaveEquipment(EquipmentPanel equipmentPanel)
    {
        return SaveItems(equipmentPanel.GetEquipmentSlots());
    }

    private InventorySaveData SaveItems(IList<InventorySlot> inventorySlots)
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

        return saveData;
    }

    public InventorySaveData LoadInventory(Dictionary<string, object> data)
    {
        return LoadItem(data);
    }

    public InventorySaveData LoadEquipment(Dictionary<string, object> data)
    {
        return LoadItem(data);
    }

    private InventorySaveData LoadItem(Dictionary<string, object> data)
    {
        InventorySaveData inventorySaveData = new InventorySaveData(Convert.ToInt32(data["numItems"]));
        if (!data.ContainsKey("savedSlots"))
            return null;
        List<object> savedSlots = (List<object>)data["savedSlots"];

        for (int i = 0; i < savedSlots.Count; i++)
        {
            if (savedSlots[i].ToString() == InventorySaveData.NullPlaceholder)
            {
                inventorySaveData.SavedSlots[i] = null;
            }
            else
            {
                Dictionary<string, object> slotData = (Dictionary<string, object>)savedSlots[i];
                Dictionary<string, object> itemData = (Dictionary<string, object>)slotData["item"];

                Item item = ItemDatabase.GetItemCopy((string)itemData["ID"]);
                int amount = Convert.ToInt32(slotData["amount"]);
                inventorySaveData.SavedSlots[i] = new InventorySlotSaveData(item, amount);
            }
        }
        return inventorySaveData;
    }
}