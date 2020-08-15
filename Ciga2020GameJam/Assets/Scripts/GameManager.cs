using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityAtoms.BaseAtoms;
using UnityCore.AudioSystem;
using UnityCore.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = UnityCore.AudioSystem.AudioType;

public class GameManager : Singleton<GameManager>
{

    [Header("Item Type")] public ItemType HolderType;
    public ItemType DecoratorType;
    
    public FloatReference GameSecondsThisRound;
    public IntReference GameHardRate;
    public BoolReference GamePaused;

    [Header("UI CONTROL")] 
    public PageTypeAtom MenuPageType;
    public PageTypeAtom LoadingPageType;
    public PageTypeAtom GamePageType;

    public void OnSwitchToSceneMenu()
    {
        AudioController.Instance.RestartAudio(AudioType.BgMusic01, true, 1.0f);
    }

    public void OnSwitchToSceneIntro()
    {
        AudioController.Instance.RestartAudio(AudioType.BgMusic02, true, 1.0f);
    }
    
    public void OnSwitchToSceneGame()
    {
        AudioController.Instance.RestartAudio(AudioType.BgMusic03, true, 1.0f);
    }

    public void OnSceneDifficultyRaised(int hardRate)
    {
        if (hardRate == 1)
        {
            AudioController.Instance.RestartAudio(AudioType.BgMusic04, true, 1.0f);
        }
        else if (hardRate == 2)
        {
            AudioController.Instance.RestartAudio(AudioType.BgMusic05, true, 1.0f);
        }
    }

    public void EnterIntroGame()
    {
        //没玩过intro
        if (PlayerPrefs.GetInt("Intro") == 0)
        {
            SceneController.Instance.Load(SceneType.Intro, loadingPage: LoadingPageType);
            OnSwitchToSceneIntro();
        }
        else
        {
            EnterFormalGame();
        }
    }

    public void EnterFormalGame()
    {
        PlayerPrefs.SetInt("Intro", 1);
        OnSwitchToSceneGame();
        SceneController.Instance.Load(SceneType.Game, loadingPage: LoadingPageType);
    }

    // Start is called before the first frame update
    void Start()
    {
        OnSwitchToSceneGame();
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
            package.DestroyAttachedItem();
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
