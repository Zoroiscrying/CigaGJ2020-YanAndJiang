using System.Collections;
using System.Collections.Generic;
using NaughtyCharacter;
using UnityCore.AudioSystem;
using UnityEngine;
using AudioType = UnityCore.AudioSystem.AudioType;

public class PlayerAudio : MonoBehaviour
{
    private Character _character;
    private bool _characterLastFrameGrounded = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_character.IsGrounded && _characterLastFrameGrounded)
        {
            Debug.Log("Jumped!");
            AudioController.Instance.RestartAudio(AudioType.PlayerSFX_Jump);  
        }

        _characterLastFrameGrounded = _character.IsGrounded;
        
        
    }
}
