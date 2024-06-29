using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Teacher_Command_UI : MonoBehaviour
{
    //変数宣言----------------------------------
    [SerializeField] private Image lessonUI; //授業ボタンUI
    [SerializeField] private Image exposureUI; //摘発ボタンUI
    [SerializeField] private Image emperorTimeUI; //エンペラータイムUI
    [SerializeField] private Image cameraChangeUI; //カメラ切り替えUI

    private static readonly float displayAlpha = 1.0f; //表示
    private static readonly float hiddenAlpha = 0.2f; //非表示  
    //------------------------------------------

    /// <summary>
    /// 授業ボタンのUIだけ光らせる
    /// </summary>
    /// <param name="context"></param>
    public void OnLessonUIFlash(InputAction.CallbackContext context)
    {
        //押された瞬間だけ
        if (!context.performed) return;
        Debug.Log("授業ボタンが押された。");
        //授業ボタンを光らせ、それ以外を暗くするメソッドの呼び出し
        SwitchButtonVisibility(lessonUI, new Image[] { exposureUI, emperorTimeUI, cameraChangeUI });
    }

    /// <summary>
    /// 摘発ボタンのUIだけを光らせる
    /// </summary>
    /// <param name="context"></param>
    public void OnExposureUIFlash(InputAction.CallbackContext context)
    {
        //押された瞬間だけ
        if (!context.performed) return;
        Debug.Log("摘発ボタンが押された。");
        //授業ボタンを光らせ、それ以外を暗くするメソッドの呼び出し
        SwitchButtonVisibility(exposureUI, new Image[] { lessonUI, emperorTimeUI, cameraChangeUI });
    }

    /// <summary>
    /// 特殊時間ボタンを光らせる
    /// </summary>
    /// <param name="context"></param>
    public void OnEmperorTimeUIFlash(InputAction.CallbackContext context)
    {
        //押された瞬間だけ
        if (!context.performed) return;
        Debug.Log("特殊時間ボタンが押された。");
        //授業ボタンを光らせ、それ以外を暗くするメソッドの呼び出し
        SwitchButtonVisibility(emperorTimeUI, new Image[] { exposureUI, lessonUI, cameraChangeUI });
    }

    /// <summary>
    /// カメラの切り替えボタンを光らせる
    /// </summary>
    /// <param name="context"></param>
    public void OnCameraChangeFlash(InputAction.CallbackContext context)
    {
        //押された瞬間だけ
        if (!context.performed) return;
        Debug.Log("カメラ切り替えボタンが押された。");
        //授業ボタンを光らせ、それ以外を暗くするメソッドの呼び出し
        SwitchButtonVisibility(cameraChangeUI, new Image[] { exposureUI, emperorTimeUI, lessonUI });
    }

    /// <summary>
    /// 押したボタンを光らせ、そのほかを暗くする処理
    /// </summary>
    /// <param name="targetButton">光らせるUI</param>
    /// <param name="otherButtons">暗くするUI</param>
    private void SwitchButtonVisibility(Image targetButton, Image[] otherButtons)
    {
        Debug.Log("対応したボタンが光る");
        // 対象のボタンを光らせる
        targetButton.color = new Color(1.0f, 1.0f, 1.0f, displayAlpha);

        // 他のボタンを暗くする
        foreach (Image button in otherButtons)
        {
            button.color = new Color(1.0f, 1.0f, 1.0f, hiddenAlpha);
        }
    }
}
