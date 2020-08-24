using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlockInstall : MonoBehaviour
{
    public BasicItem Item;
    public RoomBlock Block;

    // Start is called before the first frame update
    void Start()
    {
        // Vector3 localScale = Item.transform.localScale;
        // Item.transform.position = Block.CalculatePosition(Item);
        // Item.transform.SetParent(Block.transform, true);
        // Item.transform.localScale = localScale;
        // Item.ThawPosition();
        // var rotation = Item.transform.eulerAngles;
        // rotation = new Vector3(0, 0 + Item.DegreeRotY1, 0);
        // Item.transform.eulerAngles = rotation;
        // Item.FreezePosition();
        // Item.ChangeCollider(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 localScale = Item.transform.localScale;
            Item.transform.position = Block.CalculatePosition(Item);
            Item.transform.SetParent(Block.transform, true);
            Item.transform.localScale = localScale;
            Item.ThawPosition();
            Item.transform.Rotate(this.transform.up, 90 + Item.DegreeRotY1);
            Item.FreezePosition();
            Item.ChangeCollider(false);
        }
    }
}
