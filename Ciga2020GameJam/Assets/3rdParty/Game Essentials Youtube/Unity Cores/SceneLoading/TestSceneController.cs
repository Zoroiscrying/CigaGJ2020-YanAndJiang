using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore.SceneManagement
{
    public class TestSceneController : MonoBehaviour
    {
        public SceneController SceneController;
        public PageTypeAtom PageTypeAtom;
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneController.Load(SceneType.Menu, loadingPage: PageTypeAtom);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SceneController.Load(SceneType.Game, loadingPage: PageTypeAtom);
            }
        }
#endif
    }
}

