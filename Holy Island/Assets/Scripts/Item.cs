using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField]
    private float cost;

    [SerializeField]
    private TextMeshProUGUI text;

    public float Cost { get { return cost; } }

    public void Action(PlayerData playerData, Transform playerTransform)
    {
        playerData.Damage += power;
        playerData.MoveSpeed += moveSpeed;
        Debug.Log("Item Picked Up");
        playerData.AttackDepth += attackRange;
        playerData.AttackWidth += attackWidth;
        Vector3 attackPosition = playerData.AttackPoint.transform.localPosition;
        Vector3 newPosition = new Vector3(attackPosition.x, attackPosition.y, attackPosition.z + attackRange);
        playerData.AttackPoint.transform.localPosition = newPosition;

        Destroy(this.gameObject);
    }

    private void Start()
    {
        text.text = "" + cost;
    }

    private void Update()
    {
        transform.GetComponentInChildren<Canvas>().transform.rotation = Camera.main.transform.rotation;
    }

}
