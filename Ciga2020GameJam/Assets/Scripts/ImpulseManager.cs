using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class ImpulseManager : Singleton<ImpulseManager>
{
    public CinemachineImpulseSource MinSource;
    public CinemachineImpulseSource MidSource;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void GenerateImpulse(int level)
    {
        if (level == 1)
        {
            MinSource.GenerateImpulse();
        }else if (level == 2)
        {
            MidSource.GenerateImpulse();
        }
        else
        {
            MinSource.GenerateImpulse();
        }
    }
}
