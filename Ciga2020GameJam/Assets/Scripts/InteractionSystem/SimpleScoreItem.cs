using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScoreItem : BasicTouchItem
{
    public override void ItemFunction()
    {
        base.ItemFunction();
        EventKit.Broadcast<int, Vector3>("Add Score", 100, this.transform.position);
    }
}
