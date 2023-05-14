using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : ItemContainer, IItemContainer
{
    [SerializeField] Transform itemsParent;
    [SerializeField] InventorySlot invSlotPrefab;
    public int MaxSlots { get; set; }

    [SerializeField] private TMP_Text gold;
    [SerializeField] private TMP_Text chibiCoins;

    private void Start()
    {
        gold.text = PlayerManager.instance.CurrentPlayer.gold.ToString();
        chibiCoins.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
    }

    private void FixedUpdate()
    {
        if(gold.text != PlayerManager.instance.CurrentPlayer.gold.ToString())
            gold.text = PlayerManager.instance.CurrentPlayer.gold.ToString();
        if(chibiCoins.text != PlayerManager.instance.CurrentPlayer.chibiCoins.ToString())
            chibiCoins.text = PlayerManager.instance.CurrentPlayer.chibiCoins.ToString();
    }

    public void InitInventory()
    {
        this.inventorySlots = new List<InventorySlot>();

        for (int i = 0; i < MaxSlots; i++)
        {
            inventorySlots.Add(Instantiate(invSlotPrefab, itemsParent));
        }
        Clear();
        InitContainer();
    }

    public void LoadInventory(InventorySaveData saveData)
    {
        Debug.Log(saveData);
        if (saveData == null) return;
        int index = 0;
        foreach (InventorySlotSaveData slot in saveData.SavedSlots)
        {
            
            if (slot == null) continue;
            if (slot.itemSaveData == null) continue;
            Item item = slot.itemSaveData.ToItem();
            inventorySlots[index].item = item;
            inventorySlots[index].Amount = slot.amount;
            index++;
        }
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return inventorySlots;
    }

}
