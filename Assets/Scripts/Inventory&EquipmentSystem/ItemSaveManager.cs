
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
}