using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使い方------------------------------------------------------------
//単純に文字を出したい場合
//DebugManager.instance.DebugLog("{確認したい文字}");
//or
//変数の中身の確認をしたい場合
//DebugManager.instance.DebugLog($"{中身を確認したい変数}");

//これでできるから使ってね❤
//どこで使われてるかは、下の関数の参照から確認できるよ
//-----------------------------------------------------------------

public class Debug_Manager : MonoBehaviour
{ 
    //シングルトンの宣言
    public static Debug_Manager instance;

    //Statrより先に呼び出される関数
    private void Awake()
    {
        if (instance == null)
        {
            //インスタンス化されていない場合はインスタンス化する。
            instance = this;
            //マップを移動してもオブジェクトを取っておく。
            //DontDestroyOnLoad(gameObject);
            //→ManagerObjectをおくなら入れてもいい
        }
        else
        {
            //すでにインスタンス化されている場合はそのオブジェクトを破壊する。
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// デバックをどこでやってるかわかりやすくしたやつ
    /// </summary>
    /// <param name="s"></param>
    public void DebugLog(string s)
    {
        Debug.Log(s);
    }

}
