using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlinking : MonoBehaviour
{
    private Image _image;
    
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        var l = Mathf.Sin(Time.time) * 50 + 50;
        this._image.color = Color.HSVToRGB(0f, 0f, l/100f);
    }
}
