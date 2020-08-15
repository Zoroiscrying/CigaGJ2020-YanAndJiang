using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomBreakController : MonoBehaviour
{
    public IntReference MaxRoomMass;
    public IntReference CurrentRoomMass;
    public IntReference RecoverRequiredTaskNum;
    public IntReference CompletedTask;
    public IntReference DestroyedBlockNum;

    public void RecoverOneBlock()
    {
        RoomBlockManager.Instance.RecoverOneBlock();
        DestroyedBlockNum.Value++;
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
    }

    // Update is called once per frame
    void Update()
    {
        CheckRoomMass();
        CheckRecoverCount();
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

    private void CheckRoomMass()
    {
        if (CurrentRoomMass > MaxRoomMass)
        {
            //Camera shaking
            
            //destroy some packages
            FindAndDestroySomePackage(Random.Range(4, 6));
            //destroy some floors and relevant objects on it
            if (RoomBlockManager.Instance.DestroyOneBlock())
            {
                DestroyedBlockNum.Value--;
            }
        }
    }

    private void FindAndDestroySomePackage(int num)
    {
        List<BasicPackage> packages = FindObjectsOfType<BasicPackage>().ToList();
        for (int i = 0; i < num && packages.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, packages.Count);
            var package = packages[randomIndex];
            package.DestroySelf();
            packages.RemoveAt(randomIndex);
        }
        
    }
}
