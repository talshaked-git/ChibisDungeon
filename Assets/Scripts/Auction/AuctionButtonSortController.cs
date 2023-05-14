using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionButtonSortController : MonoBehaviour
{
    public Button HeadButtons;
    public Button ChestButtons;
    public Button BootsButtons;
    public Button GlovesButtons;
    public Button WeaponButtons;

    private Color colorActive = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color colorDefault = Color.white;

    private void Start()
    {
        HeadButtons.onClick.AddListener(ShowHeadGear);
        ChestButtons.onClick.AddListener(ShowChestGear);
        BootsButtons.onClick.AddListener(ShowBootsGear);
        GlovesButtons.onClick.AddListener(ShowGlovesGear);
        WeaponButtons.onClick.AddListener(ShowWeaponGear);
    }

    private void ShowHeadGear()
    {
        if (GetAll(HeadButtons))
            return;
        HeadButtons.image.color = colorActive;
        GetGearByType(EquipmentType.Helmet.ToString());

    }

    private void ShowChestGear()
    {

        if(GetAll(ChestButtons))
            return;
        ChestButtons.image.color = colorActive;
        GetGearByType(EquipmentType.Chest.ToString());
    }

    private void ShowBootsGear()
    {
        if(GetAll(BootsButtons))
            return;
        BootsButtons.image.color = colorActive;
        GetGearByType(EquipmentType.Boots.ToString());
    }

    private void ShowGlovesGear()
    {
        if(GetAll(GlovesButtons))
            return;
        GlovesButtons.image.color = colorActive;
        GetGearByType(EquipmentType.Gloves.ToString());
    }

    private void ShowWeaponGear()
    {
        if(GetAll(WeaponButtons))
            return;
        WeaponButtons.image.color = colorActive;
        GetGearByType(EquipmentType.MainHand.ToString());
    }

    private void ResetButtonsToDefault()
    {
        HeadButtons.image.color = colorDefault;
        ChestButtons.image.color = colorDefault;
        BootsButtons.image.color = colorDefault;
        GlovesButtons.image.color = colorDefault;
        WeaponButtons.image.color = colorDefault;
    }

    public bool GetAll(Button button)
    {
        if (button.image.color == colorActive)
        {
            ResetButtonsToDefault();
            GetGearByType("All");
            return true;
        }
        ResetButtonsToDefault();
        return false;
    }

    private void GetGearByType(string gearType)
    {
        PlayerManager.instance.GetGearByType(gearType);
    }
}
