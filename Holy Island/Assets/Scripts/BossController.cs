using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    private Room bossRoom;
    public Room BossRoom { get { return bossRoom; } set { bossRoom = value; } }

    public override void Die()
    {
        Transform exit = bossRoom.transform.Find("ExitPortal");
        exit.GetComponentInChildren<BoxCollider>().enabled = true;
        exit.GetComponentInChildren<MeshRenderer>().enabled = true;
        Destroy(this.gameObject);
    }



}
