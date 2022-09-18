using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedItem : Item
{

    [SerializeField]
    private float attackSpeed = 0;

    public override void Action(PlayerController player)
    {
        base.Action(player);
        playerData.AttackSpeed += attackSpeed;
        player.UpdateAnimator();
        Sold();
    }
}
