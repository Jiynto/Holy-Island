using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class MobEvent : UnityEvent<EnemyController>
{

}


public class EnemyController : MonoBehaviour
{
    //TODO: Create variational behaviours, and enemy types (including bosses)

    public MobEvent DeathFlag;


    [SerializeField]
    private float maxHealth;

    private float health;

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    private int minDist;

    [SerializeField]
    private int maxDist;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private int attackTime;

    private float coolDown;

    [SerializeField]
    private int goldValue;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip clip;


    public PlayerController playerController { get; set; }

    public int Damage { get { return damage; } set { damage = value; } }

    private void Start()
    {
        coolDown = attackTime;
        health = maxHealth;
    }





    // Update is called once per frame
    public void Update()
    {
        if(health <= 0)
        {
            Die();
        }
        else
        {
            if(coolDown < attackTime)
            {
                coolDown = coolDown + Time.deltaTime;
            }    
            float distance = Vector3.Distance(transform.position, playerController.transform.position);
            
            if (distance <= maxDist)
            {
                agent.SetDestination(playerController.transform.position);

                if(agent.remainingDistance > agent.stoppingDistance)
                {
                    agent.updateRotation = true;
                    animator.SetBool("targetAquired", true);
                    //transform.position += transform.forward * moveSpeed * Time.deltaTime;
                }
                else
                {
                    agent.updateRotation = false;

                    if (coolDown >= attackTime)
                    {
                        animator.SetBool("targetAquired", false);
                        animator.SetTrigger("attack");
                        Attack();
                        coolDown = 0;
                    }
                    else if (coolDown >= 1)
                    {
                        var lookPos = playerController.transform.position - transform.position;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

                        //transform.LookAt(playerController.transform);
                    }



                }
            }
            else animator.SetBool("targetAquired", false);

            transform.GetChild(0).transform.rotation = Camera.main.transform.rotation;


        }

       

    }


    public virtual void Die()
    {
        playerController.AddGold(goldValue);
        playerController.AddKill();
        DeathFlag.Invoke(this);
        Destroy(this.gameObject);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            this.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
    */


    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius, playerLayer);

        foreach (Collider hit in hits)
        {
            hit.GetComponent<PlayerController>().TakeDamage(damage, this.transform.forward);
        }

    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        //transform.position += forward * Time.deltaTime * 2;
        healthBar.fillAmount = health / maxHealth;
        audioSource.PlayOneShot(clip);
    }


    public SaveData Save()
    {
        SaveData data = new SaveData();
        data.health = health;
        data.position = gameObject.transform.position;
        data.rotation = gameObject.transform.rotation.eulerAngles;

        return data;
    }

    public void SetData(SaveData data)
    {
        health = data.health;
        //gameObject.GetComponent<NavMeshAgent>().nextPosition = data.position;
        //gameObject.transform.rotation = Quaternion.Euler(data.rotation);
    }


    [Serializable]
    public struct SaveData
    {
        public float health;
        public Vector3 position;
        public Vector3 rotation;

    }



}
