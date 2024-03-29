using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Windows;
using UnityEngine.EventSystems;

public class AuctionListing : MonoBehaviour, IPointerClickHandler
{
    public string ListingId;
    [SerializeField]
    public InventorySlot inventorySlot;
    [SerializeField]
    TMP_Text itemName;
    [SerializeField]
    TMP_Text timeLeft;
    [SerializeField]
    TMP_Text sellerName;
    [SerializeField]
    TMP_Text currentBid;
    [SerializeField]
    TMP_Text buyoutPrice;

    public event Action<InventorySlot> OnPressEvent;

    public void SetListing(AuctionListingItem listing)
    {
        ListingId = listing.ListingId;
        Item item = listing.Item.ToItem();
        inventorySlot.AddToSlot(item, listing.ItemAmount);
        inventorySlot.OnPressEvent += slot => EventHelper(slot,OnPressEvent);
        itemName.text = listing.ItemName;
        timeLeft.text = CalculateTimeLeft(listing.ExpirationTime);
        sellerName.text = listing.SellerName;
        currentBid.text = listing.TopBid + "G";
        buyoutPrice.text = listing.BuyoutPrice + "G";
    }

    public void EventHelper(InventorySlot slot, Action<InventorySlot> action)
    {
        if (action != null)
        {
            action(slot);
        }
    }

    public void UpdateListing(AuctionListingItem listing)
    {
        timeLeft.text = CalculateTimeLeft(listing.ExpirationTime);
        currentBid.text = listing.TopBid + "G";
    }

    public string CalculateTimeLeft(string expirationDate)
    {
        DateTime dateTime;

        // Parse the input string to a DateTime object
        if (DateTime.TryParseExact(expirationDate, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out dateTime))
        {
            // Get the current time
            DateTime currentTime = DateTime.UtcNow;

            // Calculate the time difference
            TimeSpan timeDifference = dateTime - currentTime;

            // Display the time difference
            if (timeDifference.TotalHours >= 1)
            {
                return $"{Math.Floor(timeDifference.TotalHours)} hours";
            }
            else
            {
                return $"{Math.Floor(timeDifference.TotalMinutes)} minutes";
            }
        }
        else
        {
            Console.WriteLine("Invalid date format");
            return "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<Auction>().UpdateSlectedListing(this);

        Debug.Log("Selected listing: " + ListingId);
    }
}
