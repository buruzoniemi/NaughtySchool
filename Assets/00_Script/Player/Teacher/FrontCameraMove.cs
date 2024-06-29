using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrontCameraMove : CameraManager
{
    [SerializeField] private FrontAndBackCamera FrontAndBackCamera;
    [SerializeField] private NeckRotation neckObj;
    [SerializeField] private LessonProgress lessonProgress;

    private Quaternion addQuaternion; //加える回転量
    private Quaternion thisQuaternion;  //自分自身のQuaternionを取得

    private bool isLeftPressed; //長押ししているかどうか
    private bool isFrontPressed; //長押ししているかどうか
    private bool isTeachingCamera; //先生の授業カメラが使用するかどうか
    private bool isBackCamera; //後ろのカメラが使用しているかどうか

    /// <summary>
    /// カメラをManagerから取得し、
    /// もろもろを初期化している
    /// </summary>
    private void Start()
    {
        isLeftPressed = false;
        isFrontPressed = false;
        isTeachingCamera = false;
        addQuaternion = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        thisQuaternion = this.transform.localRotation;
        frontCamera = SendFrontCamera();
        isBackCamera = FrontAndBackCamera.SendIsCameraChange();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        LeftRotation();
        ResetRotate();
        FrontRotation();
        //後ろのカメラを使用しているチェック
        isBackCamera = FrontAndBackCamera.SendIsCameraChange();
    }

    /// <summary>
    /// InputSystemを使用(後にCallbackではないやり方を実装予定)
    /// 長押ししているかどうかを判断する
    /// 黒板の向きから生徒たちに向く処理に使う
    /// </summary>
    /// <param name="context"></param>
    public void OnleftRotation(InputAction.CallbackContext context)
    {
        if (isFrontPressed) return;
        if (isBackCamera) return;

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                isLeftPressed = true;
                isTeachingCamera = true;//授業中
                //アニメーションの呼び出し
                teacherAnimController.AnimTowardBlackBoard();
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                isLeftPressed = false;
                isTeachingCamera = false;//授業してない
                lessonProgress.StopKeyPressed();
                //アニメーションの呼び出し
                teacherAnimController.AnimTowardStudent();
                break;
        }

    }

    /// <summary>
    /// 長押ししてるかの判断
    /// 教卓の教科書から生徒側に向く処理に使う
    /// </summary>
    /// <param name="context"></param>
    public void OnFrontRotation(InputAction.CallbackContext context)
    {
        if (isLeftPressed) return;
        if (isBackCamera) return;

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // ボタンが押された時の処理
                isFrontPressed = true;
                isTeachingCamera = true;//授業中
                //アニメーションの切り替え
                teacherAnimController.AnimLookDown();
                break;

            case InputActionPhase.Canceled:
                // ボタンが離された時の処理
                isFrontPressed = false;
                isTeachingCamera = false;//授業してない
                lessonProgress.StopKeyPressed();
                //アニメーションの切り替え
                teacherAnimController.AnimLookUp();
                break;
        }

    }

    /// <summary>
    /// 回転させてから元の位置に戻す処理
    /// 正面に向き直すのに使う
    /// </summary>
    private void ResetRotate()
    {
        if (isLeftPressed) return;
        if (isFrontPressed) return;

        //元の位置に回転させる処理
        frontCamera.transform.rotation = Quaternion.RotateTowards(frontCamera.transform.rotation, //回転させる対象
                                                                  Quaternion.Euler(rotateValue.XMin, rotateValue.YMin, 0), //どこまで回転させるか
                                                                   rotateValue.Speed * Time.deltaTime); //どれくらいのスピードで回転させるか
    }

    /// <summary>
    /// 左に回転させる処理
    /// 生徒たちの向きから黒板の方向を向くのに使う
    /// </summary>
    private void LeftRotation()
    {
        //Lトリガーが長押し状態じゃないとき、もしくはRトリガーが押されたら処理を中断させる
        if (!isLeftPressed) return;
        if (isFrontPressed) return;

        frontCamera.transform.rotation = Quaternion.RotateTowards(frontCamera.transform.rotation, //回転させる対象
                                                                  Quaternion.Euler(rotateValue.XMin, rotateValue.YMax, 0.0f), //どこまで回転させるか 
                                                                  rotateValue.Speed * Time.deltaTime); //どれくらいのスピードで回転させるか
        
        //Debug.Log($"{frontCamera.transform.rotation.eulerAngles.y}");
        //neckObj.DisableRotation();
    }

    /// <summary>
    /// 前方向に回転させる処理
    /// 生徒たちの方向から教卓の教科書に向くのに使う
    /// </summary>
    private void FrontRotation()
    {
        //Lトリガーが長押し状態のとき、もしくはRトリガーが押されてなかったら処理を中断させる
        if (isLeftPressed) return;
        if (!isFrontPressed) return;

        frontCamera.transform.rotation = Quaternion.RotateTowards(frontCamera.transform.rotation, //回転させる対象
                                                                  Quaternion.Euler(rotateValue.XMax, rotateValue.YMin, 0.0f), //どこまで回転させるか
                                                                  rotateValue.Speed * Time.deltaTime); //どれくらいのスピードで回転させるか

        //neckObj.EnableRotation();
    }

    /// <summary>
    /// 現在左に振り向いている状態かどうか
    /// </summary>
    public bool SendLeftPressed()
    {
        return isLeftPressed;
    }

    /// <summary>
    /// 現在前のめりになっている状態かどうか
    /// </summary>
    public bool SendFrontPressed()
    {
        return isFrontPressed;
    }

    ///
    /// 先生の向き方向を確認
    ///
    public bool SendTeacherToward()
    {
        return isTeachingCamera;
    }

}
