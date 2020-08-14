using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityCore.MenuSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityCore.SceneManagement
{
    public class SceneController : Singleton<SceneController>
    {
        public bool debug;
        
        private SceneType m_TargetScene;
        private PageTypeAtom m_LoadingPage;
        private Action<SceneType> m_SceneLoadDelegate;
        private bool m_SceneIsLoading;

        private string currentSceneName => SceneManager.GetActiveScene().name;

        #region Public Functions

        public void Load(SceneType sceneType,
            Action<SceneType> sceneLoadDelegate = null,
            bool reload = false,
            PageTypeAtom loadingPage = null)
        {
            if (m_LoadingPage != null && !PageController.Instance)
            {
                return;
            }

            if (!SceneCanBeLoaded(sceneType, reload))
            {
                return;
            }

            m_SceneIsLoading = true;
            m_TargetScene = sceneType;
            m_LoadingPage = loadingPage;
            m_SceneLoadDelegate = sceneLoadDelegate;
            StartCoroutine(LoadScene());
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (SceneController.Instance == this)
            {
                //initialized the controller
                Configure();
            }
            else
            {
                Destroy(this.gameObject);
            }  
        }

        private void OnDisable()
        {
            Dispose();
        }

        #endregion

        #region Private Functions

        private void Configure()
        {
            if (SceneController.Instance == this)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;   
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private async void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            //the scene is loaded by Other Scripts rather than this Controller
            if (m_TargetScene == SceneType.None)
            {
                LogWarning("Scene is loaded without using the Scene Controller, please do not do this.");
                return;
            }
            
            //the scene is loaded by Other Scripts rather than this Controller
            SceneType sceneType = StringToSceneType(scene.name);
            if (m_TargetScene != sceneType)
            {
                LogWarning("Scene is loaded without using the Scene Controller, please do not do this.");
                return;
            }
            
            if (m_SceneLoadDelegate != null)
            {
                try
                {
                    m_SceneLoadDelegate(sceneType);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    LogWarning("Unable to respond with SceneLoadDelegate after scene [" + sceneType + "] loaded.");
                    throw;
                }
            }

            if (m_LoadingPage != null)
            {
                //wait for one second for the loading page to come up and then close the page
                await Task.Delay(1000);
                PageController.Instance.TurnPageOff(m_LoadingPage);
            }

            m_SceneIsLoading = false;
        }

        private IEnumerator LoadScene()
        {
            if (m_LoadingPage != null)
            {
                PageController.Instance.TurnPageOn(m_LoadingPage);
                while (!PageController.Instance.IsPageOn(m_LoadingPage))
                {
                    yield return null;
                }
            }

            string targetSceneName = SceneTypeToString(m_TargetScene);
            SceneManager.LoadScene(targetSceneName);
        }

        private bool SceneCanBeLoaded(SceneType sceneType, bool reload)
        {
            string targetSceneName = SceneTypeToString(sceneType);
            if (currentSceneName == targetSceneName && !reload)
            {
                LogWarning("You are trying to load a scene [" + sceneType + "] which is already loaded.");
                return false;
            }else if (targetSceneName == string.Empty)
            {
                LogWarning("The scene you are trying to load is not valid. Type["+ sceneType + "]." );
            }
            else if (m_SceneIsLoading)
            {
                LogWarning("Unable to load scene [" + sceneType + "]. Another scene [" + m_TargetScene +
                           "] is already loading");
                return false;
            }
            return true;
        }

        private string SceneTypeToString(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.Menu:
                    return "Scene_Menu";
                case SceneType.Game:
                    return "Scene_Game";
                default:
                    LogWarning("Scene [" + sceneType + "] does not contain a string for a valid scene." );
                    return string.Empty;
            }
        }

        private SceneType StringToSceneType(string sceneString)
        {
            switch (sceneString)
            {
                case "Scene_Menu":
                    return SceneType.Menu;
                case "Scene_Game":
                    return SceneType.Game;
                default:
                    LogWarning("Scene [" + sceneString + "] does not match a scene type for a valid scene." );
                    return SceneType.None;
            }
        }

        private void Log(string msg)
        {
            if (debug)
            {
                Debug.Log("[Scene Controller]:" + msg);
            }
        }

        private void LogWarning(string msg)
        {
            if (debug)
            {
                Debug.LogWarning("[Scene Controller]:" + msg);
            }
        }

        #endregion

    }
}

