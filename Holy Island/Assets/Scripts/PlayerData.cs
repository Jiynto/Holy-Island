using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    [SerializeField]
    private float maxHealth;

    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    private float health;

    public float Health { get { return health; } set { health = value; } }

    [SerializeField]
    private float moveSpeed = 4f;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField]
    private Transform attackPoint;

    public Transform AttackPoint { get { return attackPoint; } }

    [SerializeField]
    private float attackRadius;

    public float AttackRadius { get { return attackRadius; } }

    [SerializeField]
    private float damage;

    public float Damage { get { return damage; } set { damage = value; } }

    private int gold = 0;

    public int Gold { get { return gold; } set { gold = value; } }


    private void Start()
    {
        health = MaxHealth;
    }

    public void UpdateAttack()
    {
        
    }


    public void SetData(SaveData data)
    {
        maxHealth = data.maxHealth;
        health = data.health;
        moveSpeed = data.moveSpeed;
        attackRadius = data.attackRadius;
        damage = data.damage;
        gold = data.gold;
        gameObject.transform.position = data.position;
        gameObject.transform.rotation = Quaternion.Euler(data.rotation);
    }

    public SaveData Save()
    {
        SaveData data = new SaveData();
        data.maxHealth = maxHealth;
        data.health = health;
        data.moveSpeed = moveSpeed;
        data.attackRadius = attackRadius;
        data.damage = damage;
        data.gold = gold;
        data.position = gameObject.transform.position;
        data.rotation = gameObject.transform.rotation.eulerAngles;

        return data;
    }


    [Serializable]
    public struct SaveData
    {
        public float maxHealth;
        public float health;
        public float moveSpeed;
        public float attackRadius;
        public float damage;
        public int gold;
        public Vector3 position;
        public Vector3 rotation;

    }


}
