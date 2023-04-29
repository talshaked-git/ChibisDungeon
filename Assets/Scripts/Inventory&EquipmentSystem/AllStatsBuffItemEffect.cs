using System.Collections;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "AllStatsBuffItemEffect", menuName = "Chibis and Dungeons/Item Effect/All Stats Buff")]
public class AllStatsBuffItemEffect : UsableItemEffect
{
    public int STRBuff;
    public int VITBuff;
    public int INTBuff;
    public int AGIBuff;
    public float Duration;

    public override void ExecuteEffect(UsableItem parentItem, Player player)
    {
        StatModifier STRModifier = new StatModifier(STRBuff, StatModType.Flat, parentItem);
        StatModifier VITModifier = new StatModifier(VITBuff, StatModType.Flat, parentItem);
        StatModifier INTModifier = new StatModifier(INTBuff, StatModType.Flat, parentItem);
        StatModifier AGIModifier = new StatModifier(AGIBuff, StatModType.Flat, parentItem);

        player.STR.AddModifier(STRModifier);
        player.VIT.AddModifier(VITModifier);
        player.INT.AddModifier(INTModifier);
        player.AGI.AddModifier(AGIModifier);

        PlayerManager.instance.UpdateStatusPanel();
        PlayerManager.instance.StartCoroutine(RemoveBuff(player, STRModifier, VITModifier, INTModifier, AGIModifier, Duration));
    }

    public override string GetDescription()
    {
        StringBuilder sb = new StringBuilder();
        sb.Length = 0;
        if (STRBuff != 0)
            sb.AppendLine(" STR +" + STRBuff);
        if (VITBuff != 0)
            sb.AppendLine(" VIT +" + VITBuff);
        if (INTBuff != 0)
            sb.AppendLine(" INT +" + INTBuff);
        if (AGIBuff != 0)
            sb.AppendLine(" AGI +" + AGIBuff);
        sb.Append(" for " + Duration + " seconds.");
        return sb.ToString();
    }

    private static IEnumerator RemoveBuff(Player player, StatModifier strModifier, StatModifier vitModifier, StatModifier intModifier, StatModifier agiModifier, float Duration)
    {
        yield return new WaitForSecondsRealtime(Duration);
        player.STR.RemoveModifier(strModifier);
        player.VIT.RemoveModifier(vitModifier);
        player.INT.RemoveModifier(intModifier);
        player.AGI.RemoveModifier(agiModifier);
        PlayerManager.instance.UpdateStatusPanel();
    }

}
