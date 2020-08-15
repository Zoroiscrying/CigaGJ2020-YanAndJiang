using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlock : InteractableObject,IPositionable
{
    [SerializeField] private Vector2 blockPosition;
    [SerializeField] private GameObject anchorPosition;
    private BasicItem holdingItem;
    private Material _material;
    private bool hasItemUpon = false;
    
    #region Unity Functions

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
    }

    #endregion

    #region Public Functions

    public void AddItemUpon(BasicItem item)
    {
        this.holdingItem = item;
        hasItemUpon = true;
    }

    public void DropItem()
    {
        this.holdingItem = null;
        hasItemUpon = false;
    }
    
    public override bool CanInteract { get => !hasItemUpon; }
    public override void OnInteract()
    {
        base.OnInteract();
        EventKit.Broadcast<RoomBlock>("Interact Room Block", this);
    }

    public override void OnCanInteract()
    {
        base.OnCanInteract();
        _material.SetColor("_BaseColor", Color.yellow);
        _material.SetColor("_Tint", Color.yellow);
        EventKit.Broadcast<RoomBlock, bool>("Can Interact Room Block", this, true);
    }

    public override void OnQuitInteract()
    {
        base.OnQuitInteract();
        _material.SetColor("_BaseColor", Color.white);
        _material.SetColor("_Tint", Color.white);
        EventKit.Broadcast<RoomBlock, bool>("Can Interact Room Block", this, false);
    }

    public void OnBlockCanInteractVisual(bool beginInteract)
    {
        if (beginInteract)
        {
            if (hasItemUpon)
            {
                _material.SetColor("_BaseColor", Color.cyan);
            }
            else
            {
                _material.SetColor("_BaseColor", Color.red);
            }
        }
        else
        {
            _material.SetColor("_BaseColor", Color.white);
        }
    }

    public Vector3 AnchoredPosition3D { get => anchorPosition.transform.position; }
    public Vector3 LocalPosition3D { get => Vector3.zero; }
    
    public bool HasNewPosition()
    {
        return !hasItemUpon;
    }

    public Vector3 CalculatePosition(IPositionable installingObject)
    {
        return this.AnchoredPosition3D + installingObject.LocalPosition3D;
    }

    public Vector2 BlockPosition { get => blockPosition; }

    public void DestroyBlock()
    {
        //贴图？ 材质？
        if (this.hasItemUpon)
        {
            holdingItem.ClearBlock();
        }
        this.gameObject.SetActive(false);
    }

    public void RecoverBlock()
    {
        //贴图？ 材质？
        this.gameObject.SetActive(true);
    }

    #endregion
    
    
    #region Private Functions

    

    #endregion
    
}
