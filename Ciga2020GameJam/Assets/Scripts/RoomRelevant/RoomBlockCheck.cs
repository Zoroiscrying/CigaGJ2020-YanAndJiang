using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlockCheck : MonoBehaviour
{
    private ItemHolder _itemHolder;
    private CharacterController _characterController;
    private Vector2 _playerFacingDir;

    #region Unity Functions

    private void OnEnable()
    {
        //EventKit.Broadcast<RoomBlock, bool>("Can Interact Room Block", this, true);
        EventKit.Subscribe<RoomBlock, bool>("Can Interact Room Block", HandleBlockCanInteract);
        EventKit.Subscribe<RoomBlock>("Interact Room Block", HandleItemPutBlock);
    }

    private void OnDisable()
    {
        EventKit.Unsubscribe<RoomBlock, bool>("Can Interact Room Block", HandleBlockCanInteract);
        EventKit.Unsubscribe<RoomBlock>("Interact Room Block", HandleItemPutBlock);
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _itemHolder = GetComponent<ItemHolder>();
    }

    private void Update()
    {
        UpdateCharacterFacingDir();
    }

    #endregion

    #region Private Functions

    private void UpdateCharacterFacingDir()
    {
        var RotY = this.transform.rotation.eulerAngles.y;
        RotY = RotY % 360;
        if (RotY >= 0 && RotY < 90)
        {
            _playerFacingDir = new Vector2(1,1);
        }else if (RotY >= 90 && RotY < 180)
        {
            _playerFacingDir = new Vector2(1, -1);
        }
        else if (RotY >= 180 && RotY < 270)
        {
            _playerFacingDir = new Vector2(-1, -1);
        }else if (RotY >= 270 && RotY < 360)
        {
            _playerFacingDir = new Vector2(-1, 1);
        }
    }
    
    //Do visuals
    private void HandleBlockCanInteract(RoomBlock roomBlock, bool canInteract)
    {
        if (_itemHolder.HasItem)
        {
            RoomBlockManager.Instance.CheckBlockAtPos(canInteract, roomBlock.BlockPosition,
                _itemHolder.HoldingItem.PositionInfo, this._playerFacingDir);
        }
    }

    //Do Item put
    private void HandleItemPutBlock(RoomBlock roomBlock)
    {
        // Debug.Log("Put item to block.");
        if (_itemHolder.HasItem)
        {
            if (!RoomBlockManager.Instance.CanPutItem(roomBlock.BlockPosition, _itemHolder.HoldingItem.PositionInfo, this._playerFacingDir))
            {
                CannotApplyItemHere(roomBlock);
                return;
            }
            // Debug.Log("Has Item.");
            List<RoomBlock> blocks = RoomBlockManager.Instance.GetNeededBlockForItemPut(roomBlock.BlockPosition,
                _itemHolder.HoldingItem.PositionInfo, this._playerFacingDir);
            int neededBlockNum =
                Mathf.RoundToInt(_itemHolder.HoldingItem.PositionInfo.x * _itemHolder.HoldingItem.PositionInfo.y);
            // Debug.Log("Needed:" + neededBlockNum);
            if (blocks.Count == neededBlockNum)
            {
                Debug.Log("Block Num Correct");
                Vector3 placePosition = Vector3.zero;
                foreach (var block in blocks)
                {
                    placePosition += block.AnchoredPosition3D;
                    _itemHolder.HoldingItem.RegisterBlock(block);
                    block.AddItemUpon(_itemHolder.HoldingItem);
                    block.OnQuitInteract();
                }
                placePosition /= blocks.Count;
                _itemHolder.PutHandItemTo(placePosition);
            }
            else
            {
                CannotApplyItemHere(roomBlock);
            }
        }
    }

    private void CannotApplyItemHere(RoomBlock roomBlock)
    {
        
    }
    
    #endregion


}
