 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    [SerializeField]
    private StatsDisplay[] statDisplays;
    [SerializeField]
    string[] statNames;

    private CharcterStat[] stats;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatsDisplay>();
        UpdateStatNames();

    }

    public void SetStats(params CharcterStat[] charStats)
    {
        stats = charStats;

        if (stats.Length > statDisplays.Length)
        {
            Debug.Log("Not enough stat displays");
            return;
        }

        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(i < stats.Length);
        }
    }

    public void UpdateStatValues()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].ValueText.text = stats[i].Value.ToString();
        }
    }

    public void UpdateStatNames()
    {
        
        for (int i = 0; i < statNames.Length; i++)
        {

            statDisplays[i].NameText.text = statNames[i];
        }
    }
}
