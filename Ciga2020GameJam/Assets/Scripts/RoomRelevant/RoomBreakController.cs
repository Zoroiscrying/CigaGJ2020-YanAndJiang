using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityAtoms.BaseAtoms;
using UnityCore.AudioSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomBreakController : MonoBehaviour
{
    public IntReference MaxRoomMass;
    public IntReference CurrentRoomMass;
    public IntReference RecoverRequiredTaskNum;
    public IntReference CompletedTask;
    public IntReference DestroyedBlockNum;
    public IntReference MaxDestroyedBlockNum;
    public CinemachineImpulseSource CinemachineImpulseSource;

    public void RecoverOneBlock()
    {
        AudioController.Instance.PlayAudio(UnityCore.AudioSystem.AudioType.SceneSFX_Cheer);   
        RoomBlockManager.Instance.RecoverOneBlock();
        DestroyedBlockNum.Value--;
    }

    private void OnEnable()
    {
        EventKit.Subscribe<int>("Add Mass", AddRoomMass);
    }

    private void OnDisable()
    {
        EventKit.Unsubscribe<int>("Add Mass", AddRoomMass);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentRoomMass.Value = 0;
        DestroyedBlockNum.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRecoverCount();
        if (DestroyedBlockNum.Value > MaxDestroyedBlockNum.Value && !GameManager.Instance.GamePaused)
        {
            RoomBlockManager.Instance.DestroyAllBlocks();
            GameManager.Instance.PauseGame(true);
        }
        
    }

    public void AddRoomMass(int mass)
    {
        this.CurrentRoomMass.Value += mass;
        CheckRoomMass();
    }

    private void CheckRecoverCount()
    {
        if (DestroyedBlockNum.Value > 0)
        {
            if (CompletedTask.Value >= RecoverRequiredTaskNum.Value)
            {
                CompletedTask.Value = 0;
                RecoverOneBlock();
            }
        }
        else
        {
            CompletedTask.Value = 0;
        }
    }
    public void CheckRoomMass()
    {
        if (CurrentRoomMass > MaxRoomMass)
        {
            //Camera shaking
            CinemachineImpulseSource.GenerateImpulse();
            //destroy some floors and relevant objects on it
            if (RoomBlockManager.Instance.DestroyOneBlock())
            {
                CinemachineImpulseSource.GenerateImpulse();
                AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.SceneSFX_Boom);   
                DestroyedBlockNum.Value++;
            }
            FindAndDestroySomePackage(Random.Range(4, 6));
        }
    }

    private void FindAndDestroySomePackage(int num)
    {
        List<BasicPackage> packages =
            FindObjectsOfType<BasicPackage>().ToList().FindAll(package => package.instantiated == true);
        List<BasicItem> items = FindObjectsOfType<BasicItem>().ToList()
            .FindAll((item => item.gameObject.activeSelf == true));
        
        for (int i = 0; i < num; i++)
        {
            if (packages.Count == 0 && items.Count == 0)
            {
                break;
            }
            //no package found, destroy some items
            if (packages.Count <= 0)
            {
                Debug.Log("No package found, destroy one item.");
                int randomIndex = Random.Range(0, items.Count);
                var item = items[randomIndex];
                items.RemoveAt(randomIndex);
                if (item != null)
                {
                    item.DestroySelf();
                }
            }
            else
            {
                int randomIndex = Random.Range(0, packages.Count);
                var package = packages[randomIndex];
                package.DestroyAttachedItem();
                package.DestroySelf();
                packages.RemoveAt(randomIndex);   
            }
        }
        
    }
}
