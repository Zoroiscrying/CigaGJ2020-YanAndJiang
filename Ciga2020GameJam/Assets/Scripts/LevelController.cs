using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    //平台的升降
    [SerializeField] private GameObject Platform;
    private bool ShouldPlatformTurnOn = false;
    private bool PlatformOn = false;
    //挡板的消失
    [SerializeField] private GameObject Wall;
    private bool ShouldWallTurnOn = false;
    private bool WallOn = true;
    //随机性奖励物的刷新
    [SerializeField] private List<GameObject> items;
    private bool ShouldFreshItem = false;

    [SerializeField] private float eventFreshTime = 60f;
    private float NothingTimer = 0.0f;

    private void CheckWhichEventToUse()
    {
        int ind = Random.Range(0, 3);
        switch (ind)
        {
            case 0:
                ShouldPlatformTurnOn = true;
                ShouldWallTurnOn = false;
                ShouldFreshItem = false;
                break;
            case 1:
                ShouldWallTurnOn = true;
                ShouldPlatformTurnOn = false;
                ShouldFreshItem = false;
                break;
            case 2:
                ShouldFreshItem = true;
                ShouldWallTurnOn = false;
                ShouldPlatformTurnOn = false;
                break;
            default:
                break;
        }
    }
    
    private void DoWall()
    {
        float UpMovingDistance = 20f;
        if (WallOn)
        {
            //turn off
            Wall.transform.DOLocalMoveY(UpMovingDistance, 10f);
        }
        else
        {
            //turn on
            Wall.transform.DOLocalMoveY(-UpMovingDistance, 10f);
        }
    }

    private void DoPlatform()
    {
        float UpMovingDistance = 5f;
        if (WallOn)
        {
            //turn off
            Platform.transform.DOLocalMoveY(-UpMovingDistance, 10f);
        }
        else
        {
            //turn on
            Platform.transform.DOLocalMoveY(UpMovingDistance, 10f);
        }
    }

    private void DoItem()
    {
        int generateItemNum = Random.Range(4, 8);
        float itemInterval = 0.5f;
        for (int i = 0; i < generateItemNum; i++)
        {
            int randomInd = Random.Range(0, items.Count);
            var item = items[randomInd];
            Timer.Register(itemInterval * (i + 1), () =>
            {
                Instantiate(item, BoundaryChecker.Instance.GetRandomPosition(), Random.rotation);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        NothingTimer += Time.deltaTime;
        if (NothingTimer > eventFreshTime)
        {
            NothingTimer = 0.0f;
            if (ShouldFreshItem)
            {
                DoItem();
            }else if (ShouldPlatformTurnOn)
            {
                DoPlatform();
            }else if (ShouldWallTurnOn)
            {
                DoWall();
            }
            CheckWhichEventToUse();
        }
    }
}
