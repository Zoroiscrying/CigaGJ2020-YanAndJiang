using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityAtoms.BaseAtoms;
using UnityCore.AudioSystem;
using UnityCore.MenuSystem;
using UnityCore.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioType = UnityCore.AudioSystem.AudioType;

public class GameManager : Singleton<GameManager>
{

    [Header("Item Type")] public ItemType HolderType;
    public ItemType DecoratorType;

    public IntReference GameScoreThisRound;
    public FloatReference GameSecondsThisRound;
    public IntReference GameHardRate;
    public BoolReference GamePaused;

    [Header("UI CONTROL")] 
    public PageTypeAtom MenuPageType;
    public PageTypeAtom LoadingPageType;
    public PageTypeAtom GamePageType;

    [Header("VFX")] public ParticleSystem ItemDestroyedParticle;
    public ParticleSystem ItemCompleteParticle;

    private void OnEnable()
    {
        EventKit.Subscribe<int, Vector3>("Add Score", AddScore);
    }

    private void OnDisable()
    {
        EventKit.Unsubscribe<int, Vector3>("Add Score", AddScore);
    }

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
        SceneController.Instance.Load(SceneType.Game, (type =>
        {
            StartGame();
            UIController.Instance.TurnMenuPage(false);
            UIController.Instance.TurnGamePage(true);
        }),loadingPage: LoadingPageType);
    }

    public void AddScore(int score, Vector3 pos)
    {
        this.GameScoreThisRound.Value += score;
        UIController.Instance.GenerateScoreTextFromPosition(score, pos);
        int highestScore = PlayerPrefs.GetInt("Highest Score");
        if (highestScore < this.GameScoreThisRound.Value)
        {
            PlayerPrefs.SetInt("Highest Score", Mathf.RoundToInt(this.GameScoreThisRound.Value));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Intro", 0);
        
        OnSwitchToSceneMenu();
        DontDestroyOnLoad(this.gameObject);
        GamePaused.Value = true;
        GameScoreThisRound.Value = 0;
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
        // RestartRound();
    }

    public void RestartRound()
    {
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
        GameScoreThisRound.Value = 0;
        GamePaused.Value = false;
        SceneController.Instance.Load(SceneType.Game, reload: true, loadingPage: LoadingPageType);
        // RoomBlockManager.Instance.RecoverAllBlocks();
        // List<BasicPackage> packages = FindObjectsOfType<BasicPackage>().ToList();
        // List<BasicItem> items = FindObjectsOfType<BasicItem>().ToList();
        // foreach (var package in packages)
        // {
        //     package.DestroyAttachedItem();
        //     package.DestroySelf();
        // }
        //
        // foreach (var item in items)
        // {
        //     Destroy(item.gameObject);
        // }
    }

    public void BackToMenu()
    {
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
        GameScoreThisRound.Value = 0;
        GamePaused.Value = true;
        SceneController.Instance.Load(SceneType.Menu, loadingPage: LoadingPageType);
        UIController.Instance.TurnMenuPage(true);
    }

    public void StartGame()
    {
        GameSecondsThisRound.Value = 0f;
        GameHardRate.Value = 0;
        GameScoreThisRound.Value = 0;
        GamePaused.Value = false;
        UIController.Instance.TurnMenuPage(false);
    }

    public void PauseGame(bool gameEnd = false)
    {
        GamePaused.Value = true;
        UIController.Instance.TurnPausePage(true);
        if (gameEnd)
        {
            UIController.Instance.ResumeBtn.SetActive(false);
        }
        else
        {
            UIController.Instance.ResumeBtn.SetActive(true);
        }
    }
    
    public void ResumeGame()
    {
        GamePaused.Value = false;
        UIController.Instance.TurnPausePage(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GamePaused)
        {
            PauseGame();
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
