using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityCore.AudioSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomPackageGenerator : MonoBehaviour
{
    public GameObject PackagePrefab;
    public BoolReference gamePaused;
    public List<int> PackageComingTime;
    public FloatReference GameSecondsThisRound;
    //0 1 2 3
    //0-60s
    //60-120s
    //120-180s
    //180-240s
    public IntReference GameHardRate;
    public FloatReference NextPackageComeTime;

    [SerializeField] private GameObject GenerateCenter;
    [SerializeField] private Vector3 GenerateBoxXYZ;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(GenerateCenter.transform.position, GenerateBoxXYZ);
        Gizmos.color = Color.white;
    }

    public float PackageComingtimeThisHardRate
    {
        get
        {
            if (GameHardRate.Value < PackageComingTime.Count)
            {
                return PackageComingTime[GameHardRate.Value];
            }
            else
            {
                return PackageComingTime[PackageComingTime.Count - 1];
            }
        }
    }

    private Vector3 RandomPosInRegion()
    {
        var randX = Random.Range(-Mathf.Abs(GenerateBoxXYZ.x/2), Mathf.Abs(GenerateBoxXYZ.x/2));
        var randY = Random.Range(-Mathf.Abs(GenerateBoxXYZ.y/2), Mathf.Abs(GenerateBoxXYZ.y/2));
        var randZ = Random.Range(-Mathf.Abs(GenerateBoxXYZ.z/2), Mathf.Abs(GenerateBoxXYZ.z/2));
        return  GenerateCenter.transform.position + new Vector3(randX, randY, randZ);
    }
    
    private void GenerateOnePackage()
    {
        AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.PackageSFX_Generate);   
        Instantiate(PackagePrefab, RandomPosInRegion(), Quaternion.identity);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        NextPackageComeTime.Value += PackageComingtimeThisHardRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused)
        {
            if (NextPackageComeTime.Value <= 0)
            {
                GenerateOnePackage();
                NextPackageComeTime.Value += PackageComingtimeThisHardRate;
            }
            else
            {
                NextPackageComeTime.Value -= Time.deltaTime;       
            }   
        }
    }

   
    
}
