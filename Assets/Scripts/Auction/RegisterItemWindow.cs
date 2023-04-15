using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterItemWindow : MonoBehaviour
{
    [SerializeField] public InventorySlot inventorySlot;
    [SerializeField] public TMP_Dropdown timeLeft;
    [SerializeField] public TMP_InputField startingBid;
    [SerializeField] public TMP_InputField buyoutPrice;

    public event Action<InventorySlot> OnBeginDragEvent;
    public event Action<InventorySlot> OnEndDragEvent;
    public event Action<InventorySlot> OnDragEvent;
    public event Action<InventorySlot> OnDropEvent;

    public void Start()
    {
        inventorySlot.OnBeginDragEvent += OnBeginDragEvent;
        inventorySlot.OnEndDragEvent += OnEndDragEvent;
        inventorySlot.OnDragEvent += OnDragEvent;
        inventorySlot.OnDropEvent += OnDropEvent;
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
        DateTime expirationTime = DateTime.Now.Add(timeLeftSpan);

        // Convert the resulting DateTime object to a string
        string expirationTimeString = expirationTime.ToString("yyyy-MM-dd HH:mm");

        Debug.Log("Expiration Time: " + expirationTimeString);

        return expirationTimeString;
    }
}
