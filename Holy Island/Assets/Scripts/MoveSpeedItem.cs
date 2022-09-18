using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedItem : Item
{
    [SerializeField]
    private float moveSpeed = 0;

    public override void Action(PlayerController player)
    {
        base.Action(player);
        playerData.MoveSpeed += moveSpeed;
        Sold();
    }
}
