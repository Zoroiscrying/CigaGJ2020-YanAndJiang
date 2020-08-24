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
    [Header("重量")]
    [SerializeField] private int _mass;
    public int PositionPointCount => positionPoints.Count;
    [Header("放置相关")]
    [SerializeField] private Vector2 _positionInfo;
    [SerializeField] private List<GameObject> positionPoints;
    [SerializeField] private Vector3 _localPositon3D;
    [SerializeField] private float DegreeRotY = 0;
    [SerializeField] private Vector2 DegreeRotXZ = Vector2.zero;
    public Vector2 DegreeRotXz => DegreeRotXZ;
    [Header("任务相关")]
    [SerializeField] private IntReference CompletedTaskAllNum;
    [SerializeField] private IntReference CompltedTaskNum;
    [SerializeField] private List<ItemType> _selfItemTypes = new List<ItemType>();
    public List<ItemType> requireItemTypes = new List<ItemType>();
    
    private List<BasicItem> storedItems = new List<BasicItem>();
    private bool initialized = false;
    private ItemUIHolder _itemUiHolder;

    public float DegreeRotY1 => DegreeRotY;

    private List<RoomBlock> attachedRoomBlocks = new List<RoomBlock>();
    public List<ItemType> ItemTypes => _selfItemTypes;
    private Collider _collider;
    [Header("可变材质index")]
    public int variableMatIndex = 0;
    private Material _material;
    private Rigidbody _rigidbody;
    private bool m_canBeInteracted = true;
    private bool m_positioned = false;

    private Renderer outlineRenderer;

    #region Unity Functions
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _material = GetComponent<Renderer>().materials[variableMatIndex];
        EventKit.Broadcast<int>("Add Mass", this.Mass);

        if (!initialized)
        {
            this._itemUiHolder = UIController.Instance.GenerateUIHolder(this);
            _itemUiHolder.InitializeUI(this);
            _itemUiHolder.gameObject.SetActive(false);
        
            //change material color based on colorType
            foreach (var type in _selfItemTypes)
            {
                if (type.HasColor)
                {
                    _material = GetComponent<Renderer>().materials[variableMatIndex];
                    this._material.SetColor("_Tint", type.Color);
                    return;
                }
            }
        }
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

    public void Initialize(List<ItemType> itemTypes, List<ItemType> requireTypes, Material outlineMaterial)
    {
        initialized = true;
        _selfItemTypes = itemTypes.ToList();
        requireItemTypes = requireTypes.ToList();
        this._itemUiHolder = UIController.Instance.GenerateUIHolder(this);
        _itemUiHolder.InitializeUI(this);
        _itemUiHolder.gameObject.SetActive(false);
        
        var parentMesh = this.GetComponent<MeshFilter>().mesh;
        Debug.Log("Generate a new gameobject");
        var outlineObj = new GameObject();
        outlineObj.transform.SetParent(this.transform);
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localScale = Vector3.one;
        
        var meshFilter = outlineObj.AddComponent<MeshFilter>();
        meshFilter.mesh = parentMesh;
        outlineRenderer = outlineObj.AddComponent<MeshRenderer>();
        var length = this.GetComponent<MeshRenderer>().materials.Length;
        outlineRenderer.materials = new Material[length];
        var materials_copy = outlineRenderer.materials;
        for (int i = 0; i < length; i++)
        {
            materials_copy[i] = outlineMaterial;
        }
        outlineRenderer.materials = materials_copy;
        
        //change material color based on colorType
        foreach (var type in itemTypes)
        {
            if (type.HasColor)
            {
                _material = GetComponent<Renderer>().materials[variableMatIndex];
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
        if (!_rigidbody)
        {
            _rigidbody.GetComponent<Rigidbody>();
        }
        if (changeInteracted)
        {
            this.m_canBeInteracted = false;   
        }
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void ThawPosition()
    {
        if (!_rigidbody)
        {
            this.GetComponent<Rigidbody>();
        }
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
                    storedItems.Add(item);
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
        OnQuitInteract();
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
        // Debug.Log("Try do within range");
        base.OnWithInCheckRange();
        if (this.IsDecorator() && this.CanInteract)
        {
            OnSetBlockUI();
        }
    }

    public override void OnWithOutCheckRange()
    {
        // Debug.Log("Try do without range");
        base.OnWithOutCheckRange();
        if (this.IsDecorator() && this.CanInteract)
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

    public void DestroySelf()
    {
        if (storedItems != null && storedItems.Count>0)
        {
            foreach (var item in storedItems)
            {
                item.DestroySelf();
            }   
        }

        if (this.gameObject != null)
        {
            if (this.gameObject.activeSelf)
            {
                if (ImpulseManager.Instance)
                {
                    ImpulseManager.Instance.GenerateImpulse(2);   
                }
                EventKit.Broadcast<int>("Add Mass", -this.Mass);
                var obj = Instantiate(GameManager.Instance.ItemDestroyedParticle, this.transform.position,
                    Quaternion.identity);
                Timer.Register(5.0f, (() => Destroy(obj.gameObject)));
                AudioController.Instance.RestartAudio(AudioType.ItemSFX_Destroy);
            }
            this.gameObject.SetActive(false);
            Destroy(this.transform.gameObject);   
        }
    }

    private void OnDestroy()
    {
        this._selfItemTypes.Clear();
        this.requireItemTypes.Clear();
    }

    private void Satisfied()
    {
        //Add Recovered Task
        this.m_canBeInteracted = false;
        EventKit.Broadcast<int, Vector3>("Add Score", 100 + 50 * this.transform.childCount, this.transform.position);
        ImpulseManager.Instance.GenerateImpulse(2);
        AudioController.Instance.RestartAudio(AudioType.ItemSFX_Finish);
        CompletedTaskAllNum.Value++;
        CompltedTaskNum.Value++;
        Timer.Register(1.0f, DestroySelf);
        this.transform.DOScale(Vector3.zero, 0.6f).OnComplete((() =>
        {
            this.ClearBlock();
            var obj = Instantiate(GameManager.Instance.ItemCompleteParticle, this.transform.position,
                Quaternion.identity);
            Timer.Register(5.0f, (() =>
            {
                Destroy(obj.gameObject);
            }));
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
            if (outlineRenderer!= null)
            {
                outlineRenderer.enabled = true;
                foreach (var material in outlineRenderer.materials)
                {
                    material.SetFloat("_Outline", .1f);
                    material.SetColor("_OutlineColor", Color.yellow);
                }
            }
            else
            {
                _material.SetFloat("_Outline", .6f);
                _material.SetColor("_OutlineColor", Color.yellow);
            }
        }
        else
        {
            if (outlineRenderer != null)
            {
                outlineRenderer.enabled = false;
            }
            else
            {
                _material.SetFloat("_Outline", .0f);
            }
        }
    }

    private void DoUiHint(bool yes)
    {
        EventKit.Broadcast<bool>("Can Item Interact", yes);
    }

    #endregion
    
}
