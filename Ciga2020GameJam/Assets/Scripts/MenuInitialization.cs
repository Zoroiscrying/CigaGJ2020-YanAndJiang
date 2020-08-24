using System.Collections;
using System.Collections.Generic;
using UnityCore.SceneManagement;
using UnityEngine;

public class MenuInitialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneController.Instance.Load(SceneType.Menu);
    }
    
}
