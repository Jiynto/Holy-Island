using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{

    Vector3 forward, right;

    [SerializeField]
    private Image healthBar;

    public UnityEvent DeathFlag;

    public UnityEvent LevelSwitch;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LayerMask enemyLayers;

    [SerializeField]
    private CharacterController characterController;

    public bool isEnabled = false;


    public PlayerData playerData;



    // Start is called before the first frame update
    void Start()
    {
        /*
        if(PlayerPrefs.HasKey("playerHealth"))
        {
            playerData.Health = PlayerPrefs.GetFloat("playerHealth");
        }
        */

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();
        if(playerData.Health <= 0)
        {
            Die();
        }
        if(isEnabled)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Move();
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("attack");
                Attack();
            }
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
        characterController.Move(playerData.MoveSpeed * Time.deltaTime * heading);

    }


    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(playerData.AttackPoint.position, playerData.AttackRadius, enemyLayers);

        foreach(Collider hit in hits)
        {
            hit.GetComponent<EnemyController>().TakeDamage(playerData.Damage);
        }

    }    

    public void TakeDamage(float damage, Vector3 forward)
    {
        playerData.Health -= damage;
        transform.position += forward * Time.deltaTime * 2;
        healthBar.fillAmount = playerData.Health / playerData.MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        LevelSwitch.Invoke();
    }

    /*
    private void SaveData()
    {
        PlayerPrefs.SetFloat("playerHealth", playerData.Health);
    }
    */

    public void AddGold(int amount)
    {
        playerData.Gold += amount;
    }




    private void Die()
    {
        DeathFlag.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (playerData.AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(playerData.AttackPoint.position, playerData.AttackRadius);
    }





}
