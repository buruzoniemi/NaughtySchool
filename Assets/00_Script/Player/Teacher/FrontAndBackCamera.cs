using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrontAndBackCamera : CameraManager
{
    [SerializeField]private LessonProgress lessonProgress; //LessonProgressを格納しておくもの
    [SerializeField] private FrontCameraMove FrontCameraMove;
    private bool isFrontCameraChange;   //カメラが切り換えられたかどうか
    private bool isCameraChange;   //カメラが切り換えられたかどうか
    private bool isTeachingCamera; //授業中かどうかチェック

    /// <summary>
    /// ・Manager側で設定したカメラを
    ///   変数に代入する
    /// ・ゲーム開始時は黒板側のカメラを
    /// 　Active状態にする
    /// </summary>
    private void Start()
    {
        frontCamera = SendFrontCamera();
        backCamera = SendBackCamera();
        isFrontCameraChange = false;
        FrontCameraChange();
        isTeachingCamera = FrontCameraMove.SendTeacherToward();
    }

    private void Update()
    {
        isTeachingCamera = FrontCameraMove.SendTeacherToward();
    }

    /// <summary>
    /// InputSystem側の処理(後にCallbackを撤廃する予定)
    /// カメラの切り換え処理をする
    /// </summary>
    /// <param name="context"></param>
    public void OnCameraChange(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isTeachingCamera) return;

        isCameraChange = lessonProgress.SendCameraChange();
        isFrontCameraChange = !isFrontCameraChange;

        if (isCameraChange == false) return;

        //現在のカメラが切り替えられている状態かどうか
        if(!isFrontCameraChange)
        {
            FrontCameraChange();
        }
        else
        {
            BackCameraChange();
        }
    }

    /// <summary>
    /// 黒板側のカメラに切り替える処理
    /// </summary>
    public void FrontCameraChange()
    {
        frontCamera.gameObject.SetActive(true);
        backCamera.gameObject.SetActive(false);

    }

    /// <summary>
    /// ロッカー側のカメラに切り替える処理
    /// </summary>
    public void BackCameraChange()
    {
        frontCamera.gameObject.SetActive(false);
        backCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// 現在のカメラの状態を返す処理
    /// </summary>
    /// <returns></returns>
    public bool SendIsCameraChange()
    {
        return isFrontCameraChange;
    }

}
