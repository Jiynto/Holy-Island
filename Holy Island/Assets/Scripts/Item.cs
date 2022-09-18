using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Item : MonoBehaviour
{

    [SerializeField]
    protected int cost;

    [SerializeField]
    protected TextMeshProUGUI text;

    protected PlayerData playerData;

    public float Cost { get { return cost; } }

    public virtual void Action(PlayerController player)
    {
        playerData = player.playerData;
    }

    protected void Sold()
    {
        playerData.Gold -= cost;
        Destroy(this.gameObject);
    }

    private void Start()
    {
        text.text = "" + cost;
    }

    private void Update()
    {
        transform.GetComponentInChildren<Canvas>().transform.rotation = Camera.main.transform.rotation;
    }

}
