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
            // Debug.Log("Jumped!");
            AudioController.Instance.RestartAudio(AudioType.PlayerSFX_Jump);  
        }else if (_character.IsGrounded && !_characterLastFrameGrounded)
        {
            ImpulseManager.Instance.GenerateImpulse(1);
        }

        _characterLastFrameGrounded = _character.IsGrounded;
        //
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     EventKit.Broadcast<int, Vector3>("Add Score", Random.Range(0, 500), this.transform.position);
        // }
        
        
    }
}
