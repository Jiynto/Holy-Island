using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCamToSphere : MonoBehaviour
{

    public Transform target;

    [SerializeField]
    private LayerMask layerMask;
    
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Linecast(Camera.main.transform.position, target.position, out hit, layerMask))
        {
            if (hit.collider.gameObject  != Player)
            {
                target.localScale = new Vector3(4, 4, 4);
            }
            else
            {
                target.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            target.localScale = new Vector3(0, 0, 0);
        }


        
    }
}
