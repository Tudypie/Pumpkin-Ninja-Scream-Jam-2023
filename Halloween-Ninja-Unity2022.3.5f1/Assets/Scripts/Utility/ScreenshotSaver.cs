using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotSaver : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] [Range(1, 5)] private int size = 1;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            path += "screenshot ";
            path += System.Guid.NewGuid().ToString() + ".png";
            ScreenCapture.CaptureScreenshot(path, size);
            path = "";
        }
    }
}
