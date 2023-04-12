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

    private void OnValidate(){
        if(tooltip == null){
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Awake() {
        // currentPlayer = GameManager.instance.currentPlayer; //uncomment after tesing
        currentPlayer = new Player("TestPlayer", "1" ,CharClassType.Archer); //for testing delete later
        statPanel.SetStats(new CharcterStat(currentPlayer.level)
        ,currentPlayer.HP, currentPlayer.MP, currentPlayer.STR, currentPlayer.INT, currentPlayer.VIT, currentPlayer.AGI);
        statPanel.UpdateStatValues();

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

    //fix equip defualt icon bug
    private void Drop(InventorySlot dropItemslot)
    {
        if(draggedSlot == null) return;
        if(draggedSlot.CanReciveItem(draggedSlot.item) && dropItemslot.CanReciveItem(draggedSlot.item)){
            EquippableItem dragItem = draggedSlot.item as EquippableItem;
            EquippableItem dropItem = dropItemslot.item as EquippableItem;
            if(draggedSlot is EquipmentSlot){
                if(dragItem != null) {
                    dragItem.Unequip(currentPlayer);
                    dragItem.isEquipped = false;
                    // draggedSlot.icon.sprite = ((EquipmentSlot)draggedSlot).defualtIcon;
                }
                if(dropItem != null) {
                    dropItem.Equip(currentPlayer);
                    dropItem.isEquipped = true;
                }
            }
            if(dropItemslot is EquipmentSlot){
                if(dragItem != null) {
                    dragItem.Equip(currentPlayer);
                    dragItem.isEquipped = true;
                }
                if(dropItem != null) {
                    dropItem.Unequip(currentPlayer);
                    dropItem.isEquipped = false;
                    // draggedSlot.icon.sprite = ((EquipmentSlot)draggedSlot).defualtIcon;
                }
            }
            statPanel.UpdateStatValues();
            
            Item draggedItem = draggedSlot.item;
            draggedSlot.item = dropItemslot.item;
            dropItemslot.item = draggedItem;
        }
    }

    private void Drag(InventorySlot slot)
    {
        if(dragableItem.enabled)
            dragableItem.transform.position = Input.mousePosition;
    }

    private void EndDrag(InventorySlot slot)
    {
        draggedSlot = null;
        dragableItem.enabled = false;
    }

    private void BeginDrag(InventorySlot slot)
    {
        if(slot.item != null){
            draggedSlot = slot;
            dragableItem.sprite = slot.item.Icon;
            dragableItem.transform.position = Input.mousePosition;
            dragableItem.enabled = true;
        }
    }

    public void Equip(EquippableItem item) {
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

    private void HandlePressEvent(InventorySlot inventorySlot) {
        if(inventorySlot.item ==null)
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
            if(inventorySlot.item !=null && (isTooltipActive == false || inventorySlot.isTooltipActive == false))
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
            if(equippableItem !=null && equippableItem.isEquipped){
                Unequip(equippableItem);
                equippableItem.isEquipped = false;
            }
            else if(equippableItem !=null && !equippableItem.isEquipped){
                Equip(equippableItem);
                equippableItem.isEquipped = true;
            }
            isTooltipActive = false;
            tooltip.HideTooltip(); 
        }

        _lastClickTime = currentTime;
    }
}