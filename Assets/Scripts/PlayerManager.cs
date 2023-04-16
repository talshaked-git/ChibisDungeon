using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip tooltip;
    [SerializeField] Image dragableItem;

    private InventorySlot draggedSlot;

    public static bool isTooltipActive = false;
    private static InventorySlot _lastClickedSlot;

    // private bool _isWaitingForDoubleClick;
    private TimeSpan _maxDoubleClickTime = TimeSpan.FromSeconds(0.7);
    private DateTime _lastClickTime;

    private Player currentPlayer;

    private void OnValidate()
    {
        if (tooltip == null)
        {
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Awake()
    {
        currentPlayer = GameManager.instance.currentPlayer; //uncomment after tesing
        // currentPlayer = new Player("TestPlayer", "1" ,CharClassType.Archer); //for testing delete later

        //Setup events:
        //click(Tooltip) and double click(equip) on inventory item
        inventory.OnPressEvent += HandlePressEvent;
        equipmentPanel.OnPressEvent += HandlePressEvent;
        //begin drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        //end drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        //drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        //drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
    }

    private void Start()
    {
        statPanel.SetStats(currentPlayer.STR, currentPlayer.INT, currentPlayer.VIT, currentPlayer.AGI);
        statPanel.UpdateStatValues();
        statPanel.UpdateLevel(currentPlayer.level);

        UpdateOnLevelChange();
        // currentPlayer.LevelChanged += OnLevelChanged;
    }

    private void UpdateOnLevelChange()
    {
        string text;
        float ratio;

        statPanel.UpdateLevel(currentPlayer.level);

        UpdateEXPStat(out text, out ratio);

        UpdateHPStat(out text, out ratio);

        UpdateMPStat(out text, out ratio);
    }

    private void UpdateMPStat(out string text, out float ratio)
    {
        ratio = (float)currentPlayer.currentMP / currentPlayer.MP.Value;
        text = currentPlayer.currentMP + " / " + (int)currentPlayer.MP.Value;
        statPanel.UpdateMP(ratio, text);
    }

    private void UpdateHPStat(out string text, out float ratio)
    {
        ratio = (float)currentPlayer.currentHP / currentPlayer.HP.Value;
        text = currentPlayer.currentHP + " / " + (int)currentPlayer.HP.Value;
        statPanel.UpdateHP(ratio, text);
    }

    private void UpdateEXPStat(out string text, out float ratio)
    {
        ratio = (float)currentPlayer.CurrentExp / (float)currentPlayer.requiredExpForNextLevel;
        text = currentPlayer.CurrentExp + " / " + currentPlayer.requiredExpForNextLevel;
        statPanel.UpdateExp(ratio, text);
    }

    private void OnLevelChanged(object sender, EventArgs e)
    {
        UpdateOnLevelChange();
    }

    //fix equip defualt icon bug
    private void Drop(InventorySlot dropItemslot)
    {
        if (draggedSlot == null) return;

        if (dropItemslot.CanAddStack(draggedSlot.item))
        {
            AddStacks(dropItemslot);
        }
        else if (draggedSlot.CanReciveItem(draggedSlot.item) && dropItemslot.CanReciveItem(draggedSlot.item))
        {
            SwapItems(dropItemslot);
        }
    }

    private void SwapItems(InventorySlot dropItemslot)
    {
        EquippableItem dragItem = draggedSlot.item as EquippableItem;
        EquippableItem dropItem = dropItemslot.item as EquippableItem;
        EquipmentSlot draggedSlotEquipmentSlot = draggedSlot as EquipmentSlot;
        bool isChanged = false;
        //Stat Panels => INV
        if (draggedSlotEquipmentSlot != null && (dropItemslot.item == null || isEquipabble(dropItem) && draggedSlotEquipmentSlot.equipmentType == dropItem.equipmentType))
        {
            isChanged = true;
            if (dragItem != null)
            {
                dragItem.Unequip(currentPlayer);
                dragItem.isEquipped = false;
            }
            if (dropItem != null)
            {
                dropItem.Equip(currentPlayer);
                dropItem.isEquipped = true;
            }
            UpdateOnLevelChange();
        }
        //INV => Stat Panels
        if (dropItemslot is EquipmentSlot && isEquipabble(dragItem))
        {
            isChanged = true;
            if (dragItem != null)
            {
                dragItem.Equip(currentPlayer);
                dragItem.isEquipped = true;
            }
            if (dropItem != null)
            {
                dropItem.Unequip(currentPlayer);
                dropItem.isEquipped = false;
            }
            UpdateOnLevelChange();
        }
        statPanel.UpdateStatValues();
        if (!isChanged && (dropItemslot is EquipmentSlot || draggedSlot is EquipmentSlot))
            return;

        Item draggedItem = draggedSlot.item;
        int draggedItemAmount = draggedSlot.Amount;

        draggedSlot.item = dropItemslot.item;
        draggedSlot.Amount = dropItemslot.Amount;

        dropItemslot.item = draggedItem;
        dropItemslot.Amount = draggedItemAmount;
    }

    private void AddStacks(InventorySlot dropItemslot)
    {
        int numAddableStacks = dropItemslot.item.MaxStack - dropItemslot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStacks, draggedSlot.Amount);

        dropItemslot.Amount += stacksToAdd;
        draggedSlot.Amount -= stacksToAdd;
    }

    private void Drag(InventorySlot slot)
    {
        if (dragableItem.enabled)
            dragableItem.transform.position = Input.mousePosition;
    }

    private void EndDrag(InventorySlot slot)
    {
        draggedSlot = null;
        dragableItem.enabled = false;
    }

    private void BeginDrag(InventorySlot slot)
    {
        isTooltipActive = false;
        inventory.ResetIsTooltipActive(null);
        equipmentPanel.ResetIsTooltipActive(null);
        tooltip.HideTooltip();
        if (slot.item != null)
        {
            draggedSlot = slot;
            dragableItem.sprite = slot.item.Icon;
            dragableItem.transform.position = Input.mousePosition;
            dragableItem.enabled = true;
        }
    }

    public void Equip(EquippableItem item)
    {
        if (!isEquipabble(item))
            return;
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    item.Unequip(currentPlayer);
                    statPanel.UpdateStatValues();
                }
                item.Equip(currentPlayer);
                statPanel.UpdateStatValues();
                UpdateOnLevelChange();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(currentPlayer);
            statPanel.UpdateStatValues();
            UpdateOnLevelChange();
            inventory.AddItem(item);
        }
    }


    //fix double click bug in the change amount when equip item
    private void HandlePressEvent(InventorySlot inventorySlot)
    {
        if (inventorySlot.item == null)
            return;
        if (_lastClickedSlot != null && _lastClickedSlot != inventorySlot)
        {
            _lastClickTime = DateTime.MinValue;
        }

        _lastClickedSlot = inventorySlot;
        DateTime currentTime = DateTime.UtcNow;

        if (currentTime - _lastClickTime > _maxDoubleClickTime)
        {
            // Single-click detected
            Debug.Log(name + " Game Object Clicked!");
            if (inventorySlot.item != null && (isTooltipActive == false || inventorySlot.isTooltipActive == false))
            {
                isTooltipActive = true;
                inventorySlot.isTooltipActive = true;
                inventory.ResetIsTooltipActive(inventorySlot);
                equipmentPanel.ResetIsTooltipActive(inventorySlot);
                tooltip.ShowTooltip(inventorySlot.item);
            }
            else
            {
                isTooltipActive = false;
                inventory.ResetIsTooltipActive(null);
                equipmentPanel.ResetIsTooltipActive(null);
                tooltip.HideTooltip();
            }
        }
        else
        {
            // Double-click detected
            Debug.Log("Double Click");
            EquippableItem equippableItem = inventorySlot.item as EquippableItem;
            UsableItem usableItem = inventorySlot.item as UsableItem;
            if (equippableItem != null && equippableItem.isEquipped)
            {
                Unequip(equippableItem);
                equippableItem.isEquipped = false;
            }
            else if (equippableItem != null && !equippableItem.isEquipped)
            {
                Equip(equippableItem);
                equippableItem.isEquipped = true;
            }
            else if (usableItem != null)
            {
                usableItem.Use(currentPlayer);
                if (usableItem.IsConsumable)
                    inventory.RemoveItem(usableItem);
                UpdateOnLevelChange();
            }
            isTooltipActive = false;
            tooltip.HideTooltip();
        }

        _lastClickTime = currentTime;
    }

    private bool isEquipabble(EquippableItem item)
    {
        if (item != null && item.EquipableClass == currentPlayer.classType && item.EquipableLV <= currentPlayer.level)
            return true;
        return false;
    }

    public bool AddItem(Item item)
    {
        if (!inventory.IsFull())
        {
            return inventory.AddItem(item);
        }
        return false;
    }

    // public void SaveInventory(){
    //     string json = JsonUtility.ToJson(inventory);
    //     PlayerPrefs.SetString("Inventory", json);
    //     json = JsonUtility.ToJson(equipmentPanel);
    //     PlayerPrefs.SetString("Equipment", json);
    // }
}