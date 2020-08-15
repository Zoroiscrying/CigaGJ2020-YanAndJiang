using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIMethods 
{
    public static Vector2 GetScreenPosViaWorldPos(RectTransform rectCanvas, Vector3 pos)
    {
        if (Camera.main)
        {
            Vector2 canvasSize = rectCanvas.sizeDelta;
            Vector3 viewPortPos3d = Camera.main.WorldToViewportPoint(pos);
            Vector2 viewPortRelative = new Vector2(viewPortPos3d.x - 0.5f, viewPortPos3d.y - 0.5f);
            Vector2 cubeScreenPos = new Vector2(viewPortRelative.x * canvasSize.x, viewPortRelative.y * canvasSize.y);
            // Debug.Log(cubeScreenPos);
            return cubeScreenPos;
        }
        return Vector2.zero;
    }
}
