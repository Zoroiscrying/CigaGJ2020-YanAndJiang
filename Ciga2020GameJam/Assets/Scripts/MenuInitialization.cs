using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInitialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnSwitchToSceneMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
