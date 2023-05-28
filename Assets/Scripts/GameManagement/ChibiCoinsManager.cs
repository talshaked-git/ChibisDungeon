using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibiCoinsManager : MonoBehaviour
{
    public static ChibiCoinsManager instance;

    public delegate void CoinsIncreaseHandler(int amount);
    public delegate bool CoinsDecreaseHandler(int amount);
    public delegate void CoinsChangeHandler();
    public event CoinsIncreaseHandler OnChibiCoinsIncrease;
    public event CoinsDecreaseHandler OnChibiCoinsDecrease;
    public event CoinsChangeHandler OnChibiCoinsChange;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddChibiCoins(int amount)
    {
        OnChibiCoinsIncrease?.Invoke(amount);
        ChangeCoins();
    }

    public bool RemoveChibiCoins(int amount)
    {
        bool result = OnChibiCoinsDecrease?.Invoke(amount) ?? false;
        ChangeCoins();
        return result;
    }

    public void ChangeCoins()
    {
        OnChibiCoinsChange?.Invoke();
    }
}
