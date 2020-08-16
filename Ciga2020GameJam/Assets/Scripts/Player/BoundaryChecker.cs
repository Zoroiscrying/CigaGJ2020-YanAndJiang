using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using NaughtyCharacter;
using UniRx.Triggers;
using UnityCore.AudioSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoundaryChecker : Singleton<BoundaryChecker>
{
    public Vector3 GetRandomPosition()
    {
        float randX = 0;
        float randY = 0;
        float randZ = 0;
        if (!redirectOnAxis)
        {
            randX = Random.Range(-Mathf.Abs(RegerateBox.x/2), Mathf.Abs(RegerateBox.x/2));
            randY = Random.Range(-Mathf.Abs(RegerateBox.y/2), Mathf.Abs(RegerateBox.y/2));
            randZ = Random.Range(-Mathf.Abs(RegerateBox.z/2), Mathf.Abs(RegerateBox.z/2));
        }
        var pos = RegerateCenter.transform.position + new Vector3(randX, randY, randZ);
        return pos;
    }
    
    public Vector3 RegerateBox = Vector3.one;
    public GameObject RegerateCenter;
    public bool redirectOnAxis = false;
    public Vector3 DirectedIndicator = new Vector3(1,1,0);
    private void OnTriggerEnter(Collider other)
    {
        float randX = 0;
        float randY = 0;
        float randZ = 0;
        if (!redirectOnAxis)
        {
            randX = Random.Range(-Mathf.Abs(RegerateBox.x/2), Mathf.Abs(RegerateBox.x/2));
            randY = Random.Range(-Mathf.Abs(RegerateBox.y/2), Mathf.Abs(RegerateBox.y/2));
            randZ = Random.Range(-Mathf.Abs(RegerateBox.z/2), Mathf.Abs(RegerateBox.z/2));
        }
        var pos = RegerateCenter.transform.position + new Vector3(randX, randY, randZ);  

        if (other.CompareTag("Player"))
        {
            AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.SceneSFX_FallDown);   
            var characterController = other.GetComponent<CharacterController>();
            var character = other.GetComponent<Character>();
            // characterController.SimpleMove(Vector3.zero);
            characterController.SimpleMove(Vector3.zero);
            characterController.enabled = false;
            if (redirectOnAxis)
            {
                other.transform.position = new Vector3(0,5,other.transform.position.z);
            }
            else
            {
                other.transform.position = pos;   
            }
            characterController.enabled = true;
            characterController.SimpleMove(Vector3.zero);
            // characterController.velocity.Set(0,0,0);
        }
        else
        {
            if (other.attachedRigidbody)
            {
                other.attachedRigidbody.velocity = Vector3.zero;
            }
            other.transform.position = pos;  
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(RegerateCenter.transform.position, RegerateBox);
    }
}
