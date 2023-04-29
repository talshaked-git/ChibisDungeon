using Firebase.Firestore;
using System;
using System.Collections.Generic;
using UnityEngine;


[FirestoreData]
public class InventorySaveData
{
    public const string NullPlaceholder = "NULL_PLACEHOLDER";
    [FirestoreProperty]
    public InventorySlotSaveData[] SavedSlots { get; set; }
    [FirestoreProperty]
    public int numItems { get; set; }

    public InventorySaveData() { }

    public InventorySaveData(int numItems)
    {
        this.numItems = numItems;
        SavedSlots = new InventorySlotSaveData[numItems];
    }

}

[FirestoreData]
public class InventorySlotSaveData
{
    [FirestoreProperty]
    public ItemSaveData itemSaveData { get; set; }
    [FirestoreProperty]
    public int amount { get; set; }

    public InventorySlotSaveData() { }

    public InventorySlotSaveData(InventorySlot slot)
    {
        if(slot.item.uniqueID == null)
        {
            slot.item.uniqueID = Guid.NewGuid().ToString();
        }
        itemSaveData = new ItemSaveData(slot.item);
        amount = slot.Amount;
    }

    public InventorySlotSaveData(Item item, int amount)
    {
        this.itemSaveData = new ItemSaveData(item);
        this.amount = amount;
    }
}

[FirestoreData]
public class ItemSaveData {
    [FirestoreProperty]
    public string ID { get; set; }

    [FirestoreProperty]
    protected string uniqueID { get; set; }

    [FirestoreProperty]
    public string ItemName { get; set; }

    public ItemSaveData() { }


    public ItemSaveData(Item item)
    {
        ID = item.ID;
        uniqueID = item.uniqueID;
        ItemName = item.ItemName;
    }

    public virtual Item ToItem()
    {
        Item item = ItemSaveManager.Instance.ItemDatabase.GetItemCopy(ID);
        item.uniqueID = uniqueID;
        item.ItemName = ItemName;
        // Assign values from this class to the item for any other properties you want to store in Firestore

        return item;
    }
}

[FirestoreData]
public class EquippableItemSaveData : ItemSaveData
{
    [FirestoreProperty]
    public int STRBonus { get; set; }
    [FirestoreProperty]
    public int AGIBonus { get; set; }
    [FirestoreProperty]
    public int INTBonus { get; set; }
    [FirestoreProperty]
    public int VITBonus { get; set; }


    [FirestoreProperty]
    public float STRPercentAddBonus { get; set; }
    [FirestoreProperty]
    public float AGIPercentAddBonus { get; set; }
    [FirestoreProperty]
    public float INTPercentAddBonus { get; set; }
    [FirestoreProperty]
    public float VITPercentAddBonus { get; set; }

    public EquippableItemSaveData() : base() { }

    public EquippableItemSaveData(EquippableItem item) : base(item)
    {
        STRBonus = item.STRBonus;
        AGIBonus = item.AGIBonus;
        INTBonus = item.INTBonus;
        VITBonus = item.VITBonus;
        STRPercentAddBonus = item.STRPercentAddBonus;
        AGIPercentAddBonus = item.AGIPercentAddBonus;
        INTPercentAddBonus = item.INTPercentAddBonus;
        VITPercentAddBonus = item.VITPercentAddBonus;
    }



    public override Item ToItem()
    {
        EquippableItem item = ItemSaveManager.Instance.ItemDatabase.GetItemCopy(ID) as EquippableItem;
        if (item == null)
        {
            Debug.LogError("ItemSaveData.ToItem(): EquippableItem " + ID + " could not be found in the ItemDatabase.");
            return null;
        }
        item.uniqueID = uniqueID;
        item.ItemName = ItemName;
        item.STRBonus = STRBonus;
        item.AGIBonus = AGIBonus;
        item.INTBonus = INTBonus;
        item.VITBonus = VITBonus;
        item.STRPercentAddBonus = STRPercentAddBonus;
        item.AGIPercentAddBonus = AGIPercentAddBonus;
        item.INTPercentAddBonus = INTPercentAddBonus;
        item.VITPercentAddBonus = VITPercentAddBonus;

        return item;
    }
}