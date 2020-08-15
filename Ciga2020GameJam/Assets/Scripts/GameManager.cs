using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<BasicPackage> packages = FindObjectsOfType<BasicPackage>().ToList();
        List<BasicItem> items = FindObjectsOfType<BasicItem>().ToList();
        foreach (var package in packages)
        {
            package.DestroySelf();
        }

        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
    }

    public void BackToMenu()
    {
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
        GamePaused.Value = true;
    }

    public void StartGame()
    {
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
        GamePaused.Value = false;
    }

    public void PauseGame()
    {
        GamePaused.Value = true;
    }
    
    public void ResumeGame()
    {
        GamePaused.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            UIController.Instance.TurnPausePage(true);
        }
        
        if (GamePaused)
        {
            
        }
        else
        {
            GameSecondsThisRound.Value += Time.deltaTime;
        }
    }
}
