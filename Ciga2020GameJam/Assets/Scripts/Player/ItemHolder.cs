﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ItemHolder : MonoBehaviour, IPositionable
{
    [SerializeField] private GameObject itemHoldPosition;
    [SerializeField] private BoolReference playerHoldingItem;
    private BasicItem _item;
    public BasicItem HoldingItem => _item;

    private void OnEnable()
    {
        EventKit.Subscribe<BasicItem, bool>("Item Interact", HandleItemInteract);
    }

    private void OnDisable()
    {
        EventKit.Unsubscribe<BasicItem, bool>("Item Interact", HandleItemInteract);
    }

    private void Start()
    {
        playerHoldingItem.Value = false;
    }

    private void HandleItemInteract(BasicItem item, bool item_positioned)
    {
        // Debug.Log("Handle Item Interact");
        if (item_positioned)
        {
            //拿着物品，且对应某个被放置的物品交互 -- 检测是否能放上物品
            if (playerHoldingItem.Value == true)
            {
                Debug.Log("Try Put Item");
                if (item.CanPutItem(this._item))
                {
                    Debug.Log("Successfully Put Item");
                    item.PutItemOn(this._item);
                    PutHandItemTo(item);
                }
            }//玩家没拿着物品，导致此物品被拿起
            else
            {
                this.GetItem(item);
            }
        }//物品没有被放置
        else
        {
            //拿着物品，对着某个没有被放置的物品交互，交换物品
            if (playerHoldingItem.Value == true)
            {
                this.DropItem();
                this.GetItem(item);
            }
            //没拿着物品，对着某个没有被放置的物品交互，拿起物品
            else
            {
                this.GetItem(item);
            }
        }
        //玩家是否拿着Item？
    }

    #region Public Functions

    public bool HasItem
    {
        get => _item != null;
    }

    public void GetItem(BasicItem item)
    {
        if (HasItem)
        {
            return;
        }
        playerHoldingItem.Value = true;
        _item = item;
        _item.ClearBlock();
        _item.FreezePosition();
        _item.ChangeCollider(true);
        item.transform.position = this.CalculatePosition(item);
        item.transform.SetParent(this.transform, true);
    }

    public void DropItem()
    {
        if (!HasItem)
        {
            return;
        }
        playerHoldingItem.Value = false;
        _item.ThawPosition();
        _item.ChangeCollider(false);
        _item.transform.SetParent(null, true);
        _item = null;
    }

    public void PutHandItemTo(BasicItem putTo)
    {
        if (!HasItem)
        {
            return;
        }
        playerHoldingItem.Value = false;
        _item.transform.position = putTo.CalculatePosition(putTo);
        _item.transform.SetParent(putTo.transform, true);
        _item.FreezePosition();
        _item.ChangeCollider(false);
        _item = null;
    }

    public void PutHandItemTo(Vector3 position)
    {
        if (!HasItem)
        {
            return;
        }

        Vector3 rawDir = this.transform.position - position;
        Vector2 XZdir = new Vector2(rawDir.x, rawDir.z).normalized;
        float rotY = 0;
        if (Mathf.Abs(XZdir.x) > Mathf.Abs(XZdir.y))
        {
            // XZdir = new Vector2(Mathf.RoundToInt(XZdir.x), 0);
            if (Mathf.RoundToInt(XZdir.x) > 0)
            {
                rotY = 90;
            }
            else
            {
                rotY = 270;
            }
        }
        else
        {
            if (Mathf.RoundToInt(XZdir.y) > 0)
            {
                rotY = 0;
            }
            else
            {
                rotY = 180;
            }
            // XZdir = new Vector2(0, Mathf.RoundToInt(XZdir.y));
        }


        playerHoldingItem.Value = false;
        _item.OnSetBlockUI();
        _item.transform.position = position + _item.LocalPosition3D;
        _item.transform.SetParent(null, true);
        _item.ThawPosition();
        var rotation = _item.transform.eulerAngles;
        rotation = new Vector3(0, rotY + _item.DegreeRotY1, 0);
        _item.transform.eulerAngles = rotation;
        _item.ChangeCollider(false);
        _item.FreezePosition(false);
        _item = null;
    }

    public Vector3 AnchoredPosition3D { get => itemHoldPosition.transform.position; }
    public Vector3 LocalPosition3D { get => Vector3.zero; }
    
    public bool HasNewPosition()
    {
        return true;
    }

    public Vector3 CalculatePosition(IPositionable installingObject)
    {
        return AnchoredPosition3D + installingObject.LocalPosition3D;
    }

    #endregion
    

}