using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Scene_Change : MonoBehaviour
{
    // •Ï”éŒ¾------------------------------------------
    //•ÏXæ‚ÌScene‚Ì–¼‘O
    [SerializeField]
    private string changeSceneName;
    private float sceneChangeDelayTime = 0.0f;
    // --------------------------------------------------


    public void OnSceneChange(InputAction.CallbackContext context)
    {
        //‰Ÿ‚µ‚½uŠÔ‚¾‚¯ˆ—‚³‚¹‚é
        if (!context.performed) return;

        
        StartCoroutine(ReceiveFragSceneChange.SceneChange(sceneChangeDelayTime, changeSceneName));
    }
}
