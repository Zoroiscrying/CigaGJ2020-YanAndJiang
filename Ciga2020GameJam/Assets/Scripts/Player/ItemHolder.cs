using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityCore.AudioSystem;
using UnityEngine;
using UnityCore.AudioSystem;
using AudioType = UnityCore.AudioSystem.AudioType;

public class ItemHolder : MonoBehaviour, IPositionable
{
    [SerializeField] private GameObject itemHoldPosition;
    [SerializeField] private BoolReference playerHoldingItem;
    private Animator _animator;
    private BasicItem _item;
    public BasicItem HoldingItem => _item;

    private void OnEnable()
    {
        EventKit.Subscribe<BasicItem, bool>("Item Interact", HandleItemInteract);
        EventKit.Subscribe("Drop Item", DropItem);
    }

    private void OnDisable()
    {
        EventKit.Unsubscribe<BasicItem, bool>("Item Interact", HandleItemInteract);
        EventKit.Unsubscribe("Drop Item", DropItem);
    }

    private void Start()
    {
        playerHoldingItem.Value = false;
        _animator = this.GetComponentInChildren<Animator>();
    }

    private void HandleItemInteract(BasicItem item, bool item_positioned)
    {
        // Debug.Log("Handle Item Interact");
        if (item_positioned)
        {
            //拿着物品，且对应某个被放置的物品交互 -- 检测是否能放上物品
            if (playerHoldingItem.Value == true)
            {
                // Debug.Log("Try Put Item");
                if (item.CanPutItem(this._item))
                {
                    // Debug.Log("Successfully Put Item");
                    item.PutItemOn(this._item);
                    PutHandItemTo(item);
                }
                else
                {
                    AudioController.Instance.RestartAudio(AudioType.ItemSFX_CannotDoThis);
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
        _animator.SetTrigger("pick up");
        _animator.SetBool("pick", true);
        
        ImpulseManager.Instance.GenerateImpulse(1);
        AudioController.Instance.RestartAudio(AudioType.ItemSFX_Compose);
        playerHoldingItem.Value = true;
        _item = item;
        _item.ClearBlock();
        _item.FreezePosition();
        _item.ChangeCollider(true);

        item.OnWithInCheckRange();
        
        item.transform.position = this.CalculatePosition(item);
        item.transform.SetParent(this.transform, true);
    }

    public void DropItem()
    {
        if (!HasItem)
        {
            return;
        }
        _animator.SetBool("pick", false);
        
        _item.OnWithOutCheckRange();
        
        ImpulseManager.Instance.GenerateImpulse(1);
        AudioController.Instance.RestartAudio(AudioType.ItemSFX_DropDown);
        playerHoldingItem.Value = false;
        _item.ThawPosition();
        _item.ChangeCollider(false);
        _item.transform.SetParent(null, true);
        _item = null;
    }

    //put item to somewhere and freeze it, and disable the ui and interaction
    public void PutHandItemTo(BasicItem putTo)
    {
        if (!HasItem)
        {
            return;
        }
        
        _animator.SetBool("pick", false);
        
        ImpulseManager.Instance.GenerateImpulse(1);
        Vector3 rawDir = this.transform.position - putTo.transform.position;
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
        Vector3 localScale = _item.transform.localScale;
        playerHoldingItem.Value = false;
        _item.transform.SetParent(putTo.transform, false);
        _item.transform.localScale = new Vector3(1 / putTo.transform.localScale.x * localScale.x,
            1 / putTo.transform.localScale.y * localScale.y, 1 / putTo.transform.localScale.z * localScale.z);
        _item.transform.position = putTo.CalculatePosition(_item);
        _item.ThawPosition();
        // _item.transform.Rotate(this.transform.up, rotY + _item.DegreeRotY1);
        var rotation = _item.transform.eulerAngles;
        rotation = new Vector3(_item.DegreeRotXz.x, rotY + _item.DegreeRotY1, _item.DegreeRotXz.y);
        _item.transform.eulerAngles = rotation;
        _item.FreezePosition();
        _item.OnWithOutCheckRange();
        _item.ChangeCollider(false);
        _item = null;
        AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.ItemSFX_Compose);
    }

    //simply put item to somewhere and freeze it, no other things
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
        rotation = new Vector3(_item.DegreeRotXz.x, rotY + _item.DegreeRotY1, _item.DegreeRotXz.y);
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
