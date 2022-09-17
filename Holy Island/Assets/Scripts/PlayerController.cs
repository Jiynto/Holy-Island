using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

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
    private LayerMask itemLayer;

    [SerializeField]
    private CharacterController characterController;

    public bool isEnabled = false;

    private List<Collider> itemsInContract;

    public PlayerData playerData;

    private bool isAttacking = false;

    public UIUpdate GoldUpdate;

    // Start is called before the first frame update
    void Start()
    {

        itemsInContract = new List<Collider>();

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

        animator.SetFloat("attackSpeed", playerData.AttackSpeed);
    }


    public void AttackEnded()
    {
        isAttacking = false;
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
                if(!isAttacking)
                {
                    animator.SetTrigger("attack");
                    Attack();
                    isAttacking = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed");


                if(itemsInContract.Count > 0)
                {
                    Collider itemCollider = itemsInContract.First();
                    Item item = itemCollider.GetComponentInParent<Item>();
                    if(playerData.Gold >= item.Cost)
                    {
                        itemsInContract.Remove(itemCollider);
                        item.Action(playerData, transform);
                        GoldUpdate(playerData.Gold);

                        Debug.Log("item picked up");
                    }

                }


                //RaycastHit hit;
                /*
                Collider[] hits = Physics.OverlapBox(transform.position + transform.forward, new Vector3(1, 1, 1), Quaternion.identity, itemLayer);

                foreach(Collider hit in hits)
                {
                    if (hit.GetComponentInParent<Item>() != null)
                    {
                        hit.GetComponentInParent<Item>().Action(playerData, transform);
                    }
                }
                */
                /*
                if (Physics.Linecast(transform.position, transform.position + transform.forward * 4, out hit))
                {
                    if(hit.collider.GetComponentInParent<Item>() != null)
                    {
                        hit.collider.GetComponentInParent<Item>().Action(playerData, transform);
                    }
                }
                */
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
        Collider[] hits = Physics.OverlapBox(playerData.AttackPoint.position, new Vector3(playerData.AttackWidth, 1, playerData.AttackDepth), Quaternion.identity, enemyLayers);

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
        if (other.tag == "exit")
        {
            LevelSwitch.Invoke();
        }
        else if (other.tag == "item")
        {
            itemsInContract.Add(other);
            Debug.Log("item added to list");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "item")
        {
            itemsInContract.Remove(other);
            Debug.Log("item removed from list");
        }
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
        GoldUpdate(playerData.Gold);
    }

    public void AddKill()
    {
        playerData.Kills++;
    }




    private void Die()
    {
        DeathFlag.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (playerData.AttackPoint == null)
            return;

        Gizmos.DrawWireCube(playerData.AttackPoint.position, new Vector3(playerData.AttackWidth, 1, playerData.AttackDepth));
    }





}
