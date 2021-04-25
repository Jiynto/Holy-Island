using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType { Hallway, Room, StartRoom, BossRoom, Shop }
public class Room : MonoBehaviour
{
    
    public float Rotation { get; set; }


    public RoomType Type;


    private bool isset = true;
    public bool IsSet { get { return isset; } set { isset = value; }  }

    public void Set()
    {
        RoomValidation[] validators = gameObject.GetComponentsInChildren<RoomValidation>();
        foreach(RoomValidation validator in validators)
        {
            validator.GetComponent<BoxCollider>().isTrigger = false;
        }
    }


}
