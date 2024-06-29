using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Scene_Change : MonoBehaviour
{
    // �ϐ��錾------------------------------------------
    //�ύX���Scene�̖��O
    [SerializeField]
    private string changeSceneName;
    private float sceneChangeDelayTime = 0.0f;
    // --------------------------------------------------


    public void OnSceneChange(InputAction.CallbackContext context)
    {
        //�������u�Ԃ�������������
        if (!context.performed) return;

        
        StartCoroutine(ReceiveFragSceneChange.SceneChange(sceneChangeDelayTime, changeSceneName));
    }
}
