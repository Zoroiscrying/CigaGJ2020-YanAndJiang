using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class RoomBlockManager : Singleton<RoomBlockManager>
{
    public IntReference DestroyedBlockCount;
    // [SerializeField] private List<RoomBlock> _roomBlocks;
    private Dictionary<Vector2, RoomBlock> _blockDictionary;
    private List<RoomBlock> _destroyedBlocks;

    #region Unity Functions
    
    void Start()
    {
        _destroyedBlocks = new List<RoomBlock>();
        _blockDictionary = new Dictionary<Vector2, RoomBlock>();
        InitializeDictionary();
    }
    
    void Update()
    {
        
    }

    #endregion
    
    #region Public Functions

    /// <summary>
    /// 根据玩家当前交互的对象，确定这个物体所需要的所有Block
    /// </summary>
    /// <param name="beginInteract">是否开始或者结束Interact</param>
    /// <param name="blockPos">直接交互的Block位置</param>
    /// <param name="positionInfo">Item对应的PositionInfo，对应 X Z 长 宽</param>
    /// <param name="faceDirXZ">玩家所面朝的方向，四个方向东南西北</param>
    public void CheckBlockAtPos(bool beginInteract, Vector2 blockPos, Vector2 positionInfo, Vector2 faceDirXZ)
    {
        Vector2 detectionDir = new Vector2(1 * faceDirXZ.x, 1 * faceDirXZ.y);
        var xLong = positionInfo.x;
        var zLong = positionInfo.y;
        for (int x = 0; x < xLong; x++)
        {
            for (int y = 0; y < zLong; y++)
            {
                var newPos = new Vector2(detectionDir.x * x + blockPos.x, detectionDir.y * y + blockPos.y);
                RoomBlock targetBlock = GetBlockAtPos(newPos);
                if (targetBlock)
                {
                    targetBlock.OnBlockCanInteractVisual(beginInteract);
                }
            }   
        }
    }

    public bool CanPutItem(Vector2 blockPos, Vector2 positionInfo, Vector2 faceDirXZ)
    {
        Vector2 detectionDir = new Vector2(1 * faceDirXZ.x, 1 * faceDirXZ.y);
        var xLong = positionInfo.x;
        var zLong = positionInfo.y;
        for (int x = 0; x < xLong; x++)
        {
            for (int y = 0; y < zLong; y++)
            {
                var newPos = new Vector2(detectionDir.x * x + blockPos.x, detectionDir.y * y + blockPos.y);
                RoomBlock targetBlock = GetBlockAtPos(newPos);
                if (!targetBlock)
                {
                    return false;
                }else if (!targetBlock.CanInteract)
                {
                    return false;
                }
            }   
        }
        return true;
    }

    public List<RoomBlock> GetNeededBlockForItemPut(Vector2 blockPos, Vector2 positionInfo, Vector2 faceDirXZ)
    {
        List<RoomBlock> roomBlocks = new List<RoomBlock>();
        // var originalBlock = _blockDictionary[blockPos];
        // if (originalBlock)
        // {
        //     roomBlocks.Add(originalBlock);   
        // }
        Vector2 detectionDir = new Vector2(1 * faceDirXZ.x, 1 * faceDirXZ.y);
        var xLong = positionInfo.x;
        var zLong = positionInfo.y;
        for (int x = 0; x < xLong; x++)
        {
            for (int y = 0; y < zLong; y++)
            {
                var newPos = new Vector2(detectionDir.x * x + blockPos.x, detectionDir.y * y + blockPos.y);
                RoomBlock targetBlock = GetBlockAtPos(newPos);
                if (targetBlock)
                {
                    roomBlocks.Add(targetBlock);
                }
            }   
        }
        return roomBlocks;
    }
    
    public RoomBlock GetBlockAtPos(Vector2 blockPos)
    {
        if (!_blockDictionary.ContainsKey(blockPos))
        {
            Debug.LogWarning("The block you are trying to access is not registered!");
            return null;
        }
        RoomBlock block = _blockDictionary[blockPos];
        return block;
    }

    public void RecoverOneBlock()
    {
        if (_destroyedBlocks.Count > 0)
        {
            int RandomIndex = Random.Range(0, _destroyedBlocks.Count);
            //Recover The Block
            var block = _destroyedBlocks[RandomIndex];
            _destroyedBlocks.RemoveAt(RandomIndex);
            block.RecoverBlock();
            this._blockDictionary.Add(block.BlockPosition, block);
            DestroyedBlockCount.Value--;
        }
    }

    public bool DestroyOneBlock()
    {
        if (_blockDictionary.Keys.Count <= 0)
        {
            return false;
        }
        var keys = _blockDictionary.Keys.ToList();
        int randomIndex = Random.Range(0, keys.Count);
        var randomKey = keys[randomIndex];
        if (!_blockDictionary.ContainsKey(randomKey))
        {
            Debug.LogWarning("The block you are trying to access is not registered!");
            return false;
        }
        RoomBlock block = _blockDictionary[randomKey];
        block.DestroyBlock();
        this._blockDictionary.Remove(block.BlockPosition);
        _destroyedBlocks.Add(block);
        DestroyedBlockCount.Value++;
        return true;
    }

    #endregion
    
    #region Private Functions

    private void InitializeDictionary()
    {
        List<RoomBlock> roomBlocks = FindObjectsOfType<RoomBlock>().ToList();
        foreach (var roomBlock in roomBlocks)
        {
            var blockPos = roomBlock.BlockPosition;
            if (_blockDictionary.ContainsKey(blockPos))
            {
                var block = _blockDictionary[blockPos];
                Debug.LogWarning("[Room Block Manager]:" + "A block " + roomBlock.gameObject.name +
                                 " is sharing the same blockPos with another block:" + block.gameObject.name);
            }
            else
            {
                _blockDictionary.Add(blockPos, roomBlock);
            }
        }
    }

    #endregion
}
