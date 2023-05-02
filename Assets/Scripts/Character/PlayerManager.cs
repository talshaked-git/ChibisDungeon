using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip tooltip;
    [SerializeField] Image dragableItem;
    [SerializeField] DropItemArea dropItemArea;
    [SerializeField] QuestionDialog questionDialog;
    [SerializeField] AttributeAllocationPanel attributeAllocationPanel;
    [SerializeField] EmptyScreen emptyScreen;
    [SerializeField] Auction auction;


    private InventorySlot draggedSlot;

    public static bool isTooltipActive = false;
    private static InventorySlot _lastClickedSlot;

    private TimeSpan _maxDoubleClickTime = TimeSpan.FromSeconds(0.7);
    private DateTime _lastClickTime;

    private Player currentPlayer;

    public Player CurrentPlayer
    {
        get { return currentPlayer; }
        private set { currentPlayer = value; }
    }

    private void OnValidate()
    {
        if (tooltip == null)
        {
            tooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        currentPlayer = GameManager.instance.currentPlayer;

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
        
        LoadPlayerData();
        UpdateStatusPanel();
        currentPlayer.currentHP = (int)currentPlayer.HP.Value;
        currentPlayer.currentMP = (int)currentPlayer.MP.Value;
    }

    public void UpdateStatusPanel()
    {
        string text;
        float ratio;

        statPanel.UpdateStatValues();

        statPanel.UpdateLevel(currentPlayer.Level);

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

    private void Drop(InventorySlot dropItemslot)
    {
        if (draggedSlot == null || draggedSlot == dropItemslot) return;

        if (dropItemslot.CanAddStack(draggedSlot.item))
        {
            AddStacks(dropItemslot);
        }
        else if (draggedSlot.CanReciveItem(draggedSlot.item) && dropItemslot.CanReciveItem(draggedSlot.item))
        {
            SwapItems(dropItemslot);
        }
    }

    private void DropItemOutsideUI()
    {
        if (draggedSlot == null) return;

        questionDialog.Show();
        InventorySlot slot = draggedSlot;
        questionDialog.OnYesEvent += () => DestroyItemInSlot(slot);
    }

    private void DestroyItemInSlot(InventorySlot slot)
    {
        slot.item.Destroy();
        slot.item = null;

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
        }
        UpdateStatusPanel();
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

    public void Equip(EquippableItem item, out EquippableItem previousItem)
    {
        if (!isEquipabble(item))
        {
            previousItem = null;
            return;
        }
        if (inventory.RemoveItem(item))
        {
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(currentPlayer);
                }
                item.Equip(currentPlayer);
                UpdateStatusPanel();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
        else
        {
            previousItem = null;
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (inventory.CanAddItem(item) && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(currentPlayer);
            UpdateStatusPanel();
            inventory.AddItem(item);
        }
        UpdateStatusPanel();
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
                tooltip.ShowTooltip(inventorySlot.item, inventorySlot.transform as RectTransform);
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
                EquippableItem previousItem;
                Equip(equippableItem, out previousItem);
                equippableItem.isEquipped = true;
                if (previousItem != null)
                    previousItem.isEquipped = false;
            }
            else if (usableItem != null)
            {
                usableItem.Use(currentPlayer);
                if (usableItem.IsConsumable)
                    inventory.RemoveItem(usableItem);
                UpdateStatusPanel();
            }
            isTooltipActive = false;
            tooltip.HideTooltip();
            _lastClickTime = DateTime.MinValue;
            return;
        }

        _lastClickTime = currentTime;
    }

    private bool isEquipabble(EquippableItem item)
    {
        if (item != null && (item.EquipableClass == currentPlayer.classType || item.EquipableClass == CharClassType.Any) && item.EquipableLV <= currentPlayer.Level)
            return true;
        return false;
    }

    public bool AddItem(Item item)
    {
        if (inventory.CanAddItem(item))
        {
            return inventory.AddItem(item);
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        inventory.RemoveItem(item);
    }

    public void ShowDropItemArea()
    {
        dropItemArea.Show();
        dropItemArea.OnDropEvent += DropItemOutsideUI;
    }

    public void HideDropItemArea()
    {
        dropItemArea.Hide();
    }

    public void ShowAttributeAllocationPanel()
    {
        emptyScreen.gameObject.SetActive(true);
        attributeAllocationPanel.gameObject.SetActive(true);
        attributeAllocationPanel.InitAllocationPanel();
    }

    public void HideAttributeAllocationPanel()
    {
        emptyScreen.gameObject.SetActive(false);
        attributeAllocationPanel.gameObject.SetActive(false);
    }

    public void SaveInventory()
    {
        currentPlayer.InventorySaveData = ItemSaveManager.Instance.SaveInventory(inventory);
        currentPlayer.InventoryMaxSlots = inventory.MaxSlots;
    }

    public void SaveEquipment()
    {
        currentPlayer.EquipmentSaveData = ItemSaveManager.Instance.SaveEquipment(equipmentPanel);
    }

    public void LoadInventory()
    {
        inventory.MaxSlots = currentPlayer.InventoryMaxSlots;
        inventory.InitInventory();

        inventory.LoadInventory(currentPlayer.InventorySaveData);
    }

    public void LoadEquipment()
    {
        if (currentPlayer.EquipmentSaveData == null)
            return;
        Debug.Log("Loading Equipment");
        foreach (InventorySlotSaveData slot in currentPlayer.EquipmentSaveData.SavedSlots)
        {
            if (slot == null) continue;
            if (slot.itemSaveData != null)
            {
                EquippableItem equippableItem = slot.itemSaveData.ToItem() as EquippableItem;
                if (equippableItem != null)
                {
                    Debug.Log("Load Equipment: " + equippableItem);
                    equipmentPanel.AddItem(equippableItem, out EquippableItem previousItem);
                    equippableItem.Equip(currentPlayer);
                    equippableItem.isEquipped = true;
                }
            }
        }
        UpdateStatusPanel();
    }

    public void SavePlayerData()
    {
        SaveInventory();
        SaveEquipment();
    }

    public void LoadPlayerData()
    {
        LoadInventory();
        LoadEquipment();
    }

    public void SaveGame()
    {
        if (currentPlayer != null)
        {
            SavePlayerData();
            FireBaseManager.instance.SavePlayer(currentPlayer);
        }
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    private ItemContainer _openItemContainer;

    public void OpenItemContainer(ItemContainer container)
    {
        Debug.Log("Open Item Container");
        _openItemContainer = container;
        inventory.OnPressEvent -= HandlePressEvent;
        inventory.OnPressEvent += TransferToItemContainer;

        container.OnPressEvent += TransferToInventory;
        container.OnBeginDragEvent += BeginDrag;
        container.OnEndDragEvent += EndDrag;
        container.OnDragEvent += Drag;
        container.OnDropEvent += Drop;
        
    }


    public void CloseItemContainer(ItemContainer container)
    {
        Debug.Log("Close Item Container");
        _openItemContainer = null;
        inventory.OnPressEvent += HandlePressEvent;
        inventory.OnPressEvent -= TransferToItemContainer;

        container.OnPressEvent -= TransferToInventory;
        container.OnBeginDragEvent -= BeginDrag;
        container.OnEndDragEvent -= EndDrag;
        container.OnDragEvent -= Drag;
        container.OnDropEvent -= Drop;
    }

    private void TransferToInventory(InventorySlot inventorySlot)
    {
        Item item = inventorySlot.item;
        if (item == null)
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
            if (item != null && (isTooltipActive == false || inventorySlot.isTooltipActive == false))
            {
                isTooltipActive = true;
                inventorySlot.isTooltipActive = true;
                inventory.ResetIsTooltipActive(inventorySlot);
                _openItemContainer.ResetIsTooltipActive(inventorySlot);
                tooltip.ShowTooltip(item, inventorySlot.transform as RectTransform);
            }
            else
            {
                isTooltipActive = false;
                inventory.ResetIsTooltipActive(null);
                _openItemContainer.ResetIsTooltipActive(null);
                tooltip.HideTooltip();
            }
        }
        else
        {
            // Double-click detected
            Debug.Log("Double Click");
            if (inventory.CanAddItem(item))
            {
                _openItemContainer.RemoveItem(item);
                inventory.AddItem(item);
                isTooltipActive = false;
                tooltip.HideTooltip();
                _lastClickTime = DateTime.MinValue;
                return;
            }
        }

        _lastClickTime = currentTime;
    }

    private void TransferToItemContainer(InventorySlot inventorySlot)
    {
        Item item = inventorySlot.item;
        if (item == null)
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
            if (item != null && (isTooltipActive == false || inventorySlot.isTooltipActive == false))
            {
                isTooltipActive = true;
                inventorySlot.isTooltipActive = true;
                inventory.ResetIsTooltipActive(inventorySlot);
                _openItemContainer.ResetIsTooltipActive(inventorySlot);
                tooltip.ShowTooltip(item, inventorySlot.transform as RectTransform);
            }
            else
            {
                isTooltipActive = false;
                inventory.ResetIsTooltipActive(null);
                _openItemContainer.ResetIsTooltipActive(null);
                tooltip.HideTooltip();
            }
        }
        else
        {
            // Double-click detected
            Debug.Log("Double Click");
            if (_openItemContainer.CanAddItem(item))
            {
                inventory.RemoveItem(item);
                _openItemContainer.AddItem(item);
                isTooltipActive = false;
                tooltip.HideTooltip();
                _lastClickTime = DateTime.MinValue;
                return;
            }
            else
            {
                Item removedItem = _openItemContainer.inventorySlots[0].item;
                int removedItemAmount = _openItemContainer.inventorySlots[0].Amount;
                if(inventory.CanAddItem(removedItem, removedItemAmount))
                {
                    inventory.RemoveItem(item);
                    _openItemContainer.RemoveItem(removedItem, removedItemAmount);
                    _openItemContainer.AddItem(item);
                    inventory.AddItem(removedItem, removedItemAmount);
                    isTooltipActive = false;
                    tooltip.HideTooltip();
                    _lastClickTime = DateTime.MinValue;
                    return;

                }
            }
        }

        _lastClickTime = currentTime;
    }
}