using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BackCameraMove : CameraManager
{
    
    /// <summary>
    /// いろんな変数を初期化
    /// </summary>
    void Start()
    {
        backCamera = SendBackCamera();
    }

    private void Update()
    {

    }

}
