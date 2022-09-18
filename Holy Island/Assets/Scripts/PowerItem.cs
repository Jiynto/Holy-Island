using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItem : Item
{
    [SerializeField]
    private float power = 0;

    public override void Action(PlayerController player)
    {
        base.Action(player);
        playerData.Damage += power;
        Sold();
    }
}
