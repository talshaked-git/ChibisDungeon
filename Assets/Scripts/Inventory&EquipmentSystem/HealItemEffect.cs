using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Item Effect", menuName = "Chibis and Dungeons/Item Effect/Heal Item Effect")]
public class HealItemEffect : UsableItemEffect
{
    public int healAmount;
    public override void ExecuteEffect(UsableItem parentItem, Player player)
    {
        if (player.currentHP + healAmount > player.HP.Value)
        {
            player.currentHP = Mathf.FloorToInt(player.HP.Value);
        }
        else
        {
            player.currentHP += healAmount;
        }
    }

    public override string GetDescription()
    {
        return "Heal +" + healAmount + "HP";
    }
}
