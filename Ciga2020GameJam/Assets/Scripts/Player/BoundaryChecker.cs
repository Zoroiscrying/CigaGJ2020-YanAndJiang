using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using NaughtyCharacter;
using UnityCore.AudioSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoundaryChecker : MonoBehaviour
{
    public Vector3 RegerateBox = Vector3.one;
    public GameObject RegerateCenter;

    private void OnTriggerEnter(Collider other)
    {
        var randX = Random.Range(-Mathf.Abs(RegerateBox.x/2), Mathf.Abs(RegerateBox.x/2));
        var randY = Random.Range(-Mathf.Abs(RegerateBox.y/2), Mathf.Abs(RegerateBox.y/2));
        var randZ = Random.Range(-Mathf.Abs(RegerateBox.z/2), Mathf.Abs(RegerateBox.z/2));
        var pos = RegerateCenter.transform.position + new Vector3(randX, randY, randZ);
        
        if (other.CompareTag("Player"))
        {
            AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.SceneSFX_FallDown);   
            var characterController = other.GetComponent<CharacterController>();
            var character = other.GetComponent<Character>();
            // characterController.SimpleMove(Vector3.zero);
            characterController.SimpleMove(Vector3.zero);
            characterController.enabled = false;
            other.transform.position = pos;
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
