using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //TODO: Create variational behaviours, and enemy types (including bosses)

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

    public PlayerController PlayerController { get; set; }

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
            float distance = Vector3.Distance(transform.position, PlayerController.transform.position);
            
            if (distance <= maxDist)
            {
                agent.SetDestination(PlayerController.transform.position);

                if(agent.remainingDistance > agent.stoppingDistance)
                {
                    agent.updateRotation = true;
                    animator.SetBool("targetAquired", true);
                    //transform.position += transform.forward * moveSpeed * Time.deltaTime;
                }
                else
                {
                    agent.updateRotation = false;
                    
                    if(coolDown >= attackTime)
                    {
                        animator.SetBool("targetAquired", false);
                        animator.SetTrigger("attack");
                        Attack();
                        coolDown = 0;
                    }
                    else if(coolDown >= 1) transform.LookAt(PlayerController.transform);


                }
            }
            else animator.SetBool("targetAquired", false);

            transform.GetChild(0).transform.rotation = Camera.main.transform.rotation;


        }

       

    }


    public virtual void Die()
    {
        Destroy(this.gameObject);
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == PlayerController.gameObject)
        {
            PlayerController.TakeDamage(damage, this.transform.forward);
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
    }



}
