using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class RegisterItemWindow : ItemContainer
{
    [SerializeField] public InventorySlot inventorySlot;
    [SerializeField] public TMP_Dropdown timeLeft;
    [SerializeField] public TMP_InputField startingBid;
    [SerializeField] public TMP_InputField buyoutPrice;

    public void InitRegisterItemWindow()
    {
        inventorySlot.item = null;
        inventorySlot.Amount = 0;

        inventorySlots.Add(inventorySlot);
        Clear();
        InitContainer();
    }

    public async void RegisterNewItemLogic(Action<AuctionListingItem> callback)
    {
        if (inventorySlot.item == null)
        {
            Debug.Log("No item selected");
            return;
        }
        if (startingBid.text == "")
        {
            Debug.Log("No starting bid entered");
            return;
        }
        if (buyoutPrice.text == "")
        {
            Debug.Log("No buyout price entered");
            return;
        }
        string expirationTime = GetExpirationTime();
        Debug.Log("Registering new item");
        Debug.Log("Item: " + inventorySlot.item);
        Debug.Log("Amount: " + inventorySlot.Amount);
        Debug.Log("Starting Bid: " + startingBid.text);
        Debug.Log("Buyout Price: " + buyoutPrice.text);
        Debug.Log("Expiration Time: " + expirationTime);
        Debug.Log("Seller ID: " + PlayerManager.instance.CurrentPlayer.CID);


        PlayerManager.instance.SavePlayerData();
        string cid = PlayerManager.instance.CurrentPlayer.CID;
        string playerName = PlayerManager.instance.CurrentPlayer.name;
        AuctionListingItem auctionListingItem = new AuctionListingItem(cid,playerName,inventorySlot.item, inventorySlot.Amount, expirationTime,cid, startingBid.text, buyoutPrice.text);
        Task registerItemTask = FireBaseManager.instance.RegisterItemToAuction(auctionListingItem, PlayerManager.instance.CurrentPlayer);
        await registerItemTask;
        if( registerItemTask.IsCompletedSuccessfully)
        {
            Debug.Log("Item registered");
            ClearUI();
        }
        else
        {
            Debug.Log("Item not registered");
        }
    }
    
    private TimeSpan ParseTimeLeft(string timeLeftOption)
    {
        int hours = 0;
        if (timeLeftOption.EndsWith("H"))
        {
            if (int.TryParse(timeLeftOption.TrimEnd('H'), out hours))
            {
                return TimeSpan.FromHours(hours);
            }
        }
        return TimeSpan.Zero;
    }

    public string GetExpirationTime()
    {
        // Get the selected time left as a string
        string selectedTimeLeft = timeLeft.options[timeLeft.value].text;

        // Convert the selected time left string to a TimeSpan object
        TimeSpan timeLeftSpan = ParseTimeLeft(selectedTimeLeft);

        // Add the TimeSpan to the current time
        DateTime expirationTime = DateTime.UtcNow.Add(timeLeftSpan);

        // Convert the resulting DateTime object to a string
        string expirationTimeString = expirationTime.ToString("yyyy-MM-dd HH:mm");

        Debug.Log("Expiration Time: " + expirationTimeString);

        return expirationTimeString;
    }

    public void ClearUI()
    {
        inventorySlot.item = null;
        inventorySlot.Amount = 0;
        startingBid.text = "";
        buyoutPrice.text = "";
    }   
}
