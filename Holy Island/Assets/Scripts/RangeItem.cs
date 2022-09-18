using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeItem : Item
{
    [SerializeField]
    private float attackRange = 0;

    [SerializeField]
    private float attackWidth = 0;

    public override void Action(PlayerController player)
    {
        base.Action(player);
        playerData.AttackDepth += attackRange;
        playerData.AttackWidth += attackWidth;
        Vector3 attackPosition = playerData.AttackPoint.transform.localPosition;
        Vector3 newPosition = new Vector3(attackPosition.x, attackPosition.y, attackPosition.z + attackRange);
        playerData.AttackPoint.transform.localPosition = newPosition;
        Sold();
    }
}
