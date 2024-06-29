//Playerの数を選択するUIを管理するスクリプト

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Join_Counter_UI : MonoBehaviour
{
    // 変数宣言-----------------------------------
    private static readonly int maxPlayerCount = 3; // Playerの最大値
    private static readonly int minPlayerCount = 1; // Playerの最小値
    private int playerCount; // Playerの数を入れておく変数
    [SerializeField]
    private Text uiTextComponent; // UIのTextを引っ張ってくる変数
    //-------------------------------------------

    /// <summary>
    /// スタート時の初期化値
    /// </summary>
    private void Start()
    {
        // 初期化
        playerCount = 1;

        //uiTextComponentが入っていたら処理する
        if (uiTextComponent != null)
        {
            // 文字列に変換
            uiTextComponent.text = playerCount.ToString();
        }
        //空だった場合
        else
        {
            //error処理
            UiTextComponentNullError();
        }
    }

    /// <summary>
    /// 設定したボタンを押されたときPlayerの数を増やす処理
    /// </summary>
    /// <param name="context"></param>
    public void OnPlayerCountUp(InputAction.CallbackContext context)
    {
        //押された瞬間だけ取得する
        if (!context.performed) return;

        // 最大値を超えないようにする
        if (playerCount < maxPlayerCount)
        {
            // インクリメント
            playerCount++;
            //uiTextComponentが入っていたら処理する
            if (uiTextComponent != null)
            {
                // 文字列に変換
                uiTextComponent.text = playerCount.ToString();
            }
            //空だった場合
            else 
            {
                //error処理
                UiTextComponentNullError();
            }
        }
    }

    /// <summary>
    /// 設定したボタンを押されたときPlayerの数を減らす処理
    /// </summary>
    /// <param name="context"></param>
    public void OnPlayerCountDown(InputAction.CallbackContext context)
    {
        //押された瞬間だけ取得する
        if (!context.performed) return;

        // 最小値を超えないようにする
        if (playerCount > minPlayerCount)
        {
            // デクリメント
            playerCount--;
            //uiTextComponentが入っていたら処理する
            if (uiTextComponent != null)
            {
                // 文字列に変換
                uiTextComponent.text = playerCount.ToString();
            }
            else 
            {
                //error処理
                UiTextComponentNullError();
            }
        }
    }

    /// <summary>
    /// ゲームに参加するプレイヤー数を送る処理
    /// </summary>
    /// <returns></returns>
    public int SendPlayerCount()
    {
        //プレイヤー数を送る
        return playerCount;
    }

    /// <summary>
    /// uiTextComponentのnullチェック時のerror処理
    /// </summary>
    private void UiTextComponentNullError()
    {
        //エラーメッセージを表示する
        Debug.LogError("uiTextComponent がnullです。Player_Countスクリプトに「Text」コンポーネントを入れてください");
        return; //処理を中断する
    }
}
