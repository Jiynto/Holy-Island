using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomValidation : MonoBehaviour
{
    private bool hasCollided = false;
    public bool HasCollided { get { return hasCollided; } }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponentInParent<Room>().IsSet = false;
    }


}
