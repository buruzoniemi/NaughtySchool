using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]protected Camera frontCamera; //正面のカメラ
    [SerializeField]protected Camera backCamera;  //後ろのカメラ
    [SerializeField]protected TeacherAnim_Controller teacherAnimController; //先生用のアニメーションを管理するスクリプト
    public float callBackBesideValue; //InputSystemでのCallBackのValueを格納しておく変数
    public float callBackVerticalValue; //InputSystemでのCallBackのValueを格納しておく変数

    public struct rotateValue
    {
        private float startX; //開始位置の回転位置を取得
        private float startY; //開始位置の回転位置を取得
        private float startZ; //開始位置の回転位置を取得
        private float current; //現在の回転量を保持させる

        public static readonly float X = 1.0f;  //カメラの回転X軸
        public static readonly float Y = 1.0f;  //カメラの回転Y軸
        public static readonly float Z = 1.0f;  //カメラの回転Z軸
        public static readonly float Speed = 180.0f; //カメラの回転スピード
        public static readonly float XMin = 15.0f;   //カメラの回転角度の最小値
        public static readonly float YMin = -90.0f;   //カメラの回転角度の最小値
        public static readonly float XMax = 60.0f;   //カメラのX軸回転角度の最大値
        public static readonly float YMax = -270.0f;   //カメラのY軸回転角度の最大値
    }

    /// <summary>
    /// フロントのカメラ情報を送るためのメソッド
    /// </summary>
    /// <returns>黒板側カメラの変数</returns>
    public Camera SendFrontCamera()
    {
        //Debug.Log("正面のカメラを送る");
        return frontCamera;
    }

    /// <summary>
    /// 後ろのカメラ情報を送るためのメソッド
    /// </summary>
    /// <returns>ロッカー側のカメラの変数</returns>
    public Camera SendBackCamera()
    {
       // Debug.Log("後方のカメラを送る");
        return backCamera;
    }
}
