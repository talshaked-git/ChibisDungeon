using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributeAllocationPanel : MonoBehaviour
{
    [SerializeField] private AttributeAllocateDisplayStat[] statDisplays;
    [SerializeField] private TMP_Text statPointsText;
    private int m_statPointsAvilableOriginal;
    private int m_statPointsAvilable;

    private void awake()
    {

    }

    private void Update()
    {
        foreach (AttributeAllocateDisplayStat statDisplay in statDisplays)
        {
            if (m_statPointsAvilable == 0)
            {
                statDisplay.DisableIncreaseButton();
            }
            else
            {
                statDisplay.EnableIncreaseButton();
            }

            if (statDisplay.StatsAdded == 0)
            {
                statDisplay.DisableDecreaseButton();
            }
            else
            {
                statDisplay.EnableDecreaseButton();
            }
        }
    }

    private void OnValidate()
    {
        if (statDisplays == null)
            statDisplays = GetComponentsInChildren<AttributeAllocateDisplayStat>(true);

        if (statPointsText == null)
            statPointsText = transform.Find("Points Amount").GetComponent<TMP_Text>();

    }

    private void Reset()
    {
        foreach (AttributeAllocateDisplayStat statDisplay in statDisplays)
        {
            statDisplay.Reset();
        }
    }

    private void ConfirmStats()
    {
        foreach (AttributeAllocateDisplayStat statDisplay in statDisplays)
        {
            statDisplay.ConfirmStats(PlayerManager.instance.CurrentPlayer);
        }
    }

    public void SetPointsAmount()
    {
        m_statPointsAvilableOriginal = PlayerManager.instance.CurrentPlayer.AttributePoints;
        m_statPointsAvilable = m_statPointsAvilableOriginal;
        statPointsText.text = "Points Left: " + m_statPointsAvilableOriginal.ToString();
    }

    private void SetStatText()
    {
        if (statDisplays.Length != 4)
        {
            Debug.LogError("Stat displays array is not the correct size (4).");
            return;
        }
        statDisplays[0].SetStatText("STR", PlayerManager.instance.CurrentPlayer.STR.BaseValue);
        statDisplays[1].SetStatText("INT", PlayerManager.instance.CurrentPlayer.INT.BaseValue);
        statDisplays[2].SetStatText("VIT", PlayerManager.instance.CurrentPlayer.STR.BaseValue);
        statDisplays[3].SetStatText("AGI", PlayerManager.instance.CurrentPlayer.STR.BaseValue);
    }

    public void IncreaseStatPoints()
    {
        m_statPointsAvilable++;
        statPointsText.text = m_statPointsAvilable.ToString();
    }

    public void DecreaseStatPoints()
    {
        m_statPointsAvilable--;
        statPointsText.text = m_statPointsAvilable.ToString();
    }

    public void InitAllocationPanel()
    {
        SetPointsAmount();
        SetStatText();
    }
}
