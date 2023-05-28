using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    public delegate void GoldIncreaseHandler(int amount);
    public delegate bool GoldDecreaseHandler(int amount);
    public delegate void GoldChangeHandler();
    public event GoldIncreaseHandler OnGoldIncrease;
    public event GoldDecreaseHandler OnGoldDecrease;
    public event GoldChangeHandler OnGoldChange;
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

    public void AddGold(int amount)
    {
        OnGoldIncrease?.Invoke(amount);
        OnGoldChange?.Invoke();
    }

    public bool RemoveGold(int amount)
    {
        bool result = OnGoldDecrease?.Invoke(amount) ?? false;
        OnGoldChange?.Invoke();
        return result;
    }

    public void ChangeGold()
    {
        OnGoldChange?.Invoke();
    }


}
