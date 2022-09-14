using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private float power = 0;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private float attackSpeed = 0;

    [SerializeField]
    private float attackRange = 0;


    [SerializeField]
    private float attackWidth = 0;

    [SerializeField]
    private float health = 0;

    public void Action(PlayerData playerData, Transform playerTransform)
    {
        playerData.Damage += power;
        playerData.MoveSpeed += moveSpeed;
        Debug.Log("Item Picked Up");
        /*
        playerData.AttackDepth += attackRange;
        playerData.AttackWidth += attackWidth;
        playerData.AttackPoint.transform.localPosition += playerTransform.forward * attackRange;
        */
        Destroy(this.gameObject);
    }

}
