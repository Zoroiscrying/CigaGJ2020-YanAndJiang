using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityCore.AudioSystem;
using UnityEngine;
using AudioType = UnityCore.AudioSystem.AudioType;

public class BasicItem : InteractableObject, IMass, IPositionable, IBlockInfo
{
    [SerializeField] private int _mass;
    [SerializeField] private Vector2 _positionInfo;
    [SerializeField] private List<GameObject> positionPoints;
    [SerializeField] private Vector3 _localPositon3D;
    [SerializeField] private BoolReference _playerHoldingItem;
    // [SerializeField] private ItemRequireAttribute _itemRequireAttribute;
    [SerializeField] private IntReference CompletedTaskAllNum;
    [SerializeField] private IntReference CompltedTaskNum;
    [SerializeField] private float DegreeRotY = 0;
    
    private List<ItemType> _selfItemTypes = new List<ItemType>();
    private ItemUIHolder _itemUiHolder;

    public float DegreeRotY1 => DegreeRotY;

    private List<RoomBlock> attachedRoomBlocks = new List<RoomBlock>();
    public List<ItemType> requireItemTypes = new List<ItemType>();
    public List<ItemType> ItemTypes => _selfItemTypes;
    private Collider _collider;
    private Material _material;
    private Rigidbody _rigidbody;
    private bool m_canBeInteracted = true;
    private bool m_positioned = false;

    #region Unity Functions
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _material = GetComponent<Renderer>().material;
        EventKit.Broadcast<int>("Add Mass", this.Mass);
    }

    #endregion

    #region Public Functions

    public void OnSetBlockUI()
    {
        if (_itemUiHolder)
        {
            _itemUiHolder.gameObject.SetActive(true);   
        }
    }

    public void OnRemoveBlockUI()
    {
        if (_itemUiHolder)
        {
            _itemUiHolder.gameObject.SetActive(false);   
        }
    }

    public void Initialize(List<ItemType> itemTypes, List<ItemType> requireTypes)
    {
        _selfItemTypes = itemTypes.ToList();
        requireItemTypes = requireTypes.ToList();
        this._itemUiHolder = UIController.Instance.GenerateUIHolder(this);
        _itemUiHolder.InitializeUI(this);
        _itemUiHolder.gameObject.SetActive(false);
        
        //change material color based on colorType
        foreach (var type in itemTypes)
        {
            if (type.HasColor)
            {
                _material = GetComponent<Renderer>().material;
                this._material.SetColor("_Tint", type.Color);
                return;
            }
        }
    }
    
    public void RegisterBlock(RoomBlock block)
    {
        attachedRoomBlocks.Add(block);
        this.m_positioned = true;
    }

    public void ClearBlock()
    {
        this.OnRemoveBlockUI();
        this.ThawPosition();
        bool hasAttachedBefore = false;
        foreach (var block in attachedRoomBlocks)
        {
            hasAttachedBefore = true;
            block.DropItem();
        }

        if (hasAttachedBefore)
        {
            AudioController.Instance.RestartAudio(AudioType.ItemSFX_DropDown);
        }
        attachedRoomBlocks.Clear();
        this.m_positioned = false;
    }
    
    public override bool CanInteract { get => m_canBeInteracted; }
    public Vector2 PositionInfo { get => _positionInfo; }
    public int Mass { get => _mass; }
    
    public Vector3 AnchoredPosition3D { get; }
    public Vector3 LocalPosition3D { get => _localPositon3D; }

    public void ChangeCollider(bool disable)
    {
        _collider.enabled = !disable;
    }

    public void FreezePosition(bool changeInteracted = true)
    {
        if (changeInteracted)
        {
            this.m_canBeInteracted = false;   
        }
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ThawPosition()
    {
        this.m_canBeInteracted = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    public bool IsDecorator()
    {
        foreach (var itemType in _selfItemTypes)
        {
            if (itemType == GameManager.Instance.DecoratorType)
            {
                return true;
            }
        }
        return false;
    }

    public void PutItemOn(BasicItem item)
    {
        //把满足条件的消去
        foreach (var puttingOnType in item.ItemTypes)
        {
            for (int i = requireItemTypes.Count-1; i >= 0; i--)
            {
                if (puttingOnType == requireItemTypes[i])
                {
                    _itemUiHolder.UpdateUI(puttingOnType);
                    requireItemTypes.RemoveAt(i);
                    EventKit.Broadcast<int, Vector3>("Add Score", 50, this.transform.position);
                }

                if (CheckIfSatisfied())
                {
                    return;
                }
                
            }
        }
    }

    public bool CanPutItem(BasicItem item)
    {
        //我自己是什么属性（不能是Decorator）
        foreach (var itemType in _selfItemTypes)
        {
            if (itemType == GameManager.Instance.DecoratorType)
            {
                return false;
            }
        }
        
        //我还有空闲的位置吗？
        if (requireItemTypes.Count == 0 || positionPoints.Count == 0)
        {
            return false;
        }

        //对方物品是Decorator吗？
        if (!item.IsDecorator())
        {
            return false;
        }
        
        //我需求的属性满足Decorator的属性吗
        foreach (var itemType in item.ItemTypes)
        {
            if (this.requireItemTypes.Contains(itemType))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool HasNewPosition()
    {
        if (positionPoints.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 CalculatePosition(IPositionable installingObject)
    {
        if (HasNewPosition())
        {
            var obj = positionPoints[0];
            positionPoints.RemoveAt(0);
            return installingObject.LocalPosition3D + obj.transform.position;
        }
        else
        {
            return this.transform.position;
        }
    }

    public override void OnCanInteract()
    {
        base.OnCanInteract();
        //描边效果
        DoMaterialOutline(true);
        //UI显示
        DoUiHint(true);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        //UI消失
        DoUiHint(false);
        //是否被放置？
        EventKit.Broadcast<BasicItem, bool>("Item Interact", this, m_positioned);
    }

    public override void OnQuitInteract()
    {
        base.OnQuitInteract();
        //描边消失
        DoMaterialOutline(false);
        //UI消失
        DoUiHint(false);
    }

    public override void OnWithInCheckRange()
    {
        base.OnWithInCheckRange();
        if (this.IsDecorator())
        {
            OnSetBlockUI();
        }
    }

    public override void OnWithOutCheckRange()
    {
        base.OnWithOutCheckRange();
        if (this.IsDecorator())
        {
            OnRemoveBlockUI();
        }
    }

    #endregion

    #region Private Functions

    private bool CheckIfSatisfied()
    {
        if (requireItemTypes.Count == 0)
        {
            Satisfied();
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        if (ImpulseManager.Instance)
        {
            ImpulseManager.Instance.GenerateImpulse(2);   
        }
        EventKit.Broadcast<int>("Add Mass", -this.Mass);
        if (this.gameObject.activeSelf)
        {
            var obj = Instantiate(GameManager.Instance.ItemDestroyedParticle, this.transform.position,
                Quaternion.identity);
            Timer.Register(5.0f, (() => Destroy(obj.gameObject)));
            AudioController.Instance.RestartAudio(AudioType.ItemSFX_Destroy);
        }
    }

    private void Satisfied()
    {
        //Add Recovered Task
        EventKit.Broadcast<int, Vector3>("Add Score", 100 + 50 * this.transform.childCount, this.transform.position);
        ImpulseManager.Instance.GenerateImpulse(2);
        AudioController.Instance.RestartAudio(AudioType.ItemSFX_Finish);
        CompletedTaskAllNum.Value++;
        CompltedTaskNum.Value++;
        Timer.Register(2.0f, ()=>Destroy(this.transform.gameObject));
        this.transform.DOScale(Vector3.zero, 1.0f).OnComplete((() =>
        {
            this.ClearBlock();
            var obj = Instantiate(GameManager.Instance.ItemCompleteParticle, this.transform.position,
                Quaternion.identity);
            Timer.Register(5.0f, (() => Destroy(obj.gameObject)));
        }));
    }

    private void HandOverSelf()
    {
        EventKit.Broadcast<BasicItem>("Item Hand Over", this);
    }

    private void PlaceItem()
    {
        //tell the world that i can be placed！
        EventKit.Broadcast<BasicItem>("Place Item", this);
    }

    private void DoMaterialOutline(bool yes)
    {
        if (yes)
        {
            this._material.SetColor("_BaseColor", Color.blue);
            this._material.SetFloat("_Outline", .6f);
            this._material.SetColor("_OutlineColor", Color.yellow);
        }
        else
        {
            this._material.SetColor("_BaseColor", Color.white);
            this._material.SetFloat("_Outline", .0f);
        }
    }

    private void DoUiHint(bool yes)
    {
        EventKit.Broadcast<bool>("Can Item Interact", yes);
    }

    #endregion
    
}
