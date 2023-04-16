using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    [SerializeField]
    private StatsDisplay[] statDisplays;
    [SerializeField]
    string[] statNames;
    [SerializeField]
    private TMP_Text LevelText;
    [SerializeField]
    private SliderValueText ExpSlider;
    [SerializeField]
    private SliderValueText HPSlider;
    [SerializeField]
    private SliderValueText MPSlider;

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

    public void UpdateLevel(int level)
    {
        LevelText.text = level.ToString();
    }

    public void UpdateExp(float exp, string expText)
    {
        ExpSlider.UpdateText(expText);
        ExpSlider.UpdateSlider(exp);
    }

    public void UpdateHP(float hp, string hpText)
    {
        HPSlider.UpdateText(hpText);
        HPSlider.UpdateSlider(hp);
    }

    public void UpdateMP(float mp, string mpText)
    {
        MPSlider.UpdateText(mpText);
        MPSlider.UpdateSlider(mp);
    }


}
