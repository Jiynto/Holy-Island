using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{


    [SerializeField]
    private RoomType[] validConnectionTypes;

    [SerializeField]
    private float rotation;

    private bool isConnected = false;

    private bool hasCollided = false;

    public bool HasCollided { get { return hasCollided; } }

    public RoomType[] ValidConnectionTypes { get { return validConnectionTypes; } }
    public float Rotation { get { return rotation; } set { rotation = value; } }
    public bool IsConnected { get { return isConnected; } set { isConnected = value; }  }

    private void OnCollisionEnter(Collision collision)
    {
        hasCollided = true;
    }

}
