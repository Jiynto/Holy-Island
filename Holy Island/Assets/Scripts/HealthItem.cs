using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField]
    private float health = 0;

    public override void Action(PlayerController player)
    {
        base.Action(player);
        playerData.MaxHealth += health;
        playerData.Health += health;

        Sold();

    }
}
