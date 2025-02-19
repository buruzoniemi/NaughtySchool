using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Scene_Change : MonoBehaviour
{
    // 変数宣言------------------------------------------
    //変更先のSceneの名前
    [SerializeField]
    private string changeSceneName;
    private float sceneChangeDelayTime = 0.0f;
    // --------------------------------------------------


    public void OnSceneChange(InputAction.CallbackContext context)
    {
        //押した瞬間だけ処理させる
        if (!context.performed) return;

        
        StartCoroutine(ReceiveFragSceneChange.SceneChange(sceneChangeDelayTime, changeSceneName));
    }
}
