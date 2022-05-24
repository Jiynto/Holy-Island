using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    private void AttackEnded()
    {
        PlayerController playerController = this.GetComponentInParent<PlayerController>();
        if (playerController != null) playerController.AttackEnded();
    }

}
