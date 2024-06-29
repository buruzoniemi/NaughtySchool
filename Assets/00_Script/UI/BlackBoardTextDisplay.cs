
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoardTextDisplay : MonoBehaviour
{
    [SerializeField] private Text targetUI; //自身を格納する
    [SerializeField] private Camera frontCameraMove; //カメラの値を取得するためのコンポーネント

	private static readonly float frontCameraRotateAngleMax = 91.0f; //前方カメラの回転できる最大値

    /// <summary>
    /// UIの位置を毎フレーム更新
    /// </summary>
    private void Update()
    {
        OnUpdatePosition();
    }

    /// <summary>
    /// 黒板を向いたら表示する
    /// </summary>
    private void OnUpdatePosition()
    {
        if(frontCameraMove.transform.eulerAngles.y <= frontCameraRotateAngleMax)
        {
            //アルファ値を変更(表示)
            targetUI.color = new Color(1.0f,1.0f,1.0f,1.0f);
        }
        else
        {
            //アルファ値を変更(非表示)
            targetUI.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }
}
