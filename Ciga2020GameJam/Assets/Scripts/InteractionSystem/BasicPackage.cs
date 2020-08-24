using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityCore.AudioSystem;
using UnityEngine;
using AudioType = UnityEngine.AudioType;

public class BasicPackage : InteractableObject, IMass
{
    public bool instantiated = false;
    private Material _material;
    private BasicItem storedItem;
    public override bool CanInteract { get => _canBeInteracted; }
    private bool _canBeInteracted = true;
    [SerializeField]
    private int _mass;

    #region Unity Functions
    void Start()
    {
        EventKit.Broadcast<int>("Add Mass", this._mass);
        GetItemFromManager();
        _material = this.GetComponent<Renderer>().material;
        instantiated = true;
    }

    #endregion


    #region Public Functions

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
        AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.PackageSFX_Open);   
        base.OnInteract();
        //UI消失
        DoUiHint(false);
        //生成物体（给个初力？）
        GenerateItem();
        //自身消失
        //粒子特效
        var obj = Instantiate(GameManager.Instance.ItemDestroyedParticle, this.transform.position,
            Quaternion.identity);
        Timer.Register(5.0f, (() => Destroy(obj.gameObject)));
        DestroySelf();
    }

    public override void OnQuitInteract()
    {
        base.OnQuitInteract();
        //描边消失pacasd
        DoMaterialOutline(false);
        //UI消失
        DoUiHint(false);
    }

    public void DestroyAttachedItem()
    {
        if (storedItem)
        {
            Destroy(storedItem.gameObject);   
        }
        Debug.Log(storedItem.gameObject.name);
    }
    
    public void DestroySelf()
    {
        
        ImpulseManager.Instance.GenerateImpulse(2);
        EventKit.Broadcast<int>("Add Mass", -_mass);
        if (this.gameObject.activeSelf)
        {
            AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.PackageSFX_Destroy);   
        }
        _canBeInteracted = false;
        //自身消失
        Timer.Register(1.0f, (() => Destroy(this.gameObject)));
        //粒子特效
        this.gameObject.SetActive(false);
    }

    public int Mass { get => _mass; }

    #endregion

    #region Private Functions

    private void GetItemFromManager()
    {
        this.storedItem = ItemGenerator.Instance.GenerateRandomItem();
        if (storedItem == null)
        {
            Debug.LogWarning("What is going on?" + this.gameObject.name);
        }
    }

    private void DoMaterialOutline(bool yes)
    {
        if (_material)
        {
            if (yes)
            {
                this._material.SetColor("_BaseColor", Color.blue);
                this._material.SetFloat("_Outline", .6f);
            }
            else
            {
                this._material.SetColor("_BaseColor", Color.white);
                this._material.SetFloat("_Outline", .0f);
            }   
        }
    }

    private void DoUiHint(bool yes)
    {
        EventKit.Broadcast<bool>("Can Package Interact", yes);
    }

    private void GenerateItem()
    {
        var obj = storedItem.gameObject;
        obj.transform.position = this.transform.position;
        obj.SetActive(true);
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();
        objRigidbody.AddForce(Vector3.one * 2, ForceMode.Impulse);
        // BasicItem item = obj.GetComponent<BasicItem>();
        
        //item initialization or randomization
        
        //randomize visuals and mass and other things
        
        //initialize mass and other things
        
    }

    #endregion

}
