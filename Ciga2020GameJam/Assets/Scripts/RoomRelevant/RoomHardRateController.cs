using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class RoomHardRateController : MonoBehaviour
{
    [Tooltip("每一轮难度持续的时间，当游戏时间超过某一个时间时游戏难度会增加一点。\n 比如这个数组被设置为60，60；就意味着前60秒是难度0，到达60s时难度变为1，再过60s难度变为2")]
    public List<int> GameHardRateLastTimeEveryRound;
    public FloatReference GameSecondsThisRound;
    //0 1 2 3
    //0-60s
    //60-120s
    //120-180s
    //180-240s
    public IntReference GameHardRate;
    private int indexChecker = 0;
    private float accumulatedTime = 0f;
    private bool doneHardIncrease = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameHardRateLastTimeEveryRound.Count > 0)
        {
            accumulatedTime += GameHardRateLastTimeEveryRound[0];   
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameHardRate();
    }

    public void CheckGameHardRate()
    {
        if (!doneHardIncrease && GameSecondsThisRound.Value > accumulatedTime)
        {
            indexChecker++;
            if (indexChecker == GameHardRateLastTimeEveryRound.Count)
            {
                GameHardRate.Value++;
            }
            else if(indexChecker < GameHardRateLastTimeEveryRound.Count)
            {
                accumulatedTime += GameHardRateLastTimeEveryRound[indexChecker];
                GameHardRate.Value++;
            }
            else
            {
                doneHardIncrease = true;
            }
        }
    }
}
