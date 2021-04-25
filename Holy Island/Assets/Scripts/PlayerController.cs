using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float Maxhealth;

    private float health;

    [SerializeField]
    float moveSpeed = 4f;

    Vector3 forward, right;

    [SerializeField]
    private Image healthBar;

    public UnityEvent DeathFlag;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LayerMask enemyLayers;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private float damage;

    [SerializeField]
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        health = Maxhealth;
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();
        if(health <= 0)
        {
            Destroy(this);
        }

        if(Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") != 0)
        {
            Move();
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
            Attack();
        }

        //transform.GetChild(1).transform.rotation = Camera.main.transform.rotation;
        transform.GetComponentInChildren<Canvas>().transform.rotation = Camera.main.transform.rotation;
    }


    private void Move()
    {
        /*
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
        */
        Vector3 rightMovement = right  * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;

        Vector3 move = forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");
        characterController.Move(moveSpeed * Time.deltaTime * heading);

    }


    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayers);

        foreach(Collider hit in hits)
        {
            hit.GetComponent<EnemyController>().TakeDamage(damage);
        }

    }    

    public void TakeDamage(float damage, Vector3 forward)
    {
        health -= damage;
        transform.position += forward * Time.deltaTime * 2;
        healthBar.fillAmount = health / Maxhealth;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        TakeDamage(enemy.Damage, enemy.transform.forward);
    }
    */

    private void OnDestroy()
    {
        DeathFlag.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
