using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FloatReference GameSecondsThisRound;
    public IntReference GameHardRate;
    public BoolReference GamePaused;

    [Header("UI CONTROL")] 
    public PageTypeAtom MenuPageType;
    public PageTypeAtom LoadingPageType;
    public PageTypeAtom GamePageType;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GamePaused.Value = false;
        RestartRound();
    }

    public void RestartRound()
    {
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePaused)
        {
            
        }
        else
        {
            GameSecondsThisRound.Value += Time.deltaTime;
        }
    }
}
