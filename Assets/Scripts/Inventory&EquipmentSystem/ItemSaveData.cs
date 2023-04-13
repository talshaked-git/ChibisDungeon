

public class InventorySlotSaveData
{
    public Item item;
    public int amount;

    public InventorySlotSaveData(InventorySlot slot)
    {
        item = slot.item;
        // amount = slot.amount;
    }
}

public class InventorySaveData
{
    public InventorySlotSaveData[] SavedSlots;

    public InventorySaveData(int numItems)
    {
        SavedSlots = new InventorySlotSaveData[numItems];
    }
}