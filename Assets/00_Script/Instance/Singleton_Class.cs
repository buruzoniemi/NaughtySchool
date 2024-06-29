using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// シングルトンにさせるクラス
/// シングルトンにさせたいクラスはこのクラスに継承させて、
/// 仮装メソッドで処理させたいメソッドを呼び出す
/// </summary>
public abstract class Singleton_Class<T> : MonoBehaviour where T : Singleton_Class<T>
{
    //シングルトンの宣言
    public static T instance;

    // Startより先に呼び出される関数
    protected virtual void Awake()
    {
        if (instance == null)
        {
            // インスタンス化されていない場合はインスタンス化する。
            instance = (T)this;
            // マップを移動してもオブジェクトを取っておく。
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            // すでにインスタンス化されている場合はそのオブジェクトを破壊する。
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// インスタンスへの処理
    /// </summary>
    public void InstanceAssign()
    {

    }

    /// <summary>
    /// インスタンス破棄時の処理
    /// </summary>
    public void InstanceReset()
    {
        instance = null;
    }

    /// <summary>
    /// 現在の時間を返すための仮想メソッド
    /// </summary>
    /// <returns></returns>
    public virtual float SendNowTime() { return 0; }

    /// <summary>
    /// 制限時間を返す仮装メソッド
    /// </summary>
    /// <returns></returns>
    public virtual float SendLimitTime() { return 0; }

    public virtual Mischief BeginMischiefTask(int mischiefID) { return default; }

    public virtual void EndMischiefTask(Mischief mischief) { return; }

    public virtual bool CheckTaskIsPlay(int mischiefID) { return default; }
                   
    public virtual bool CheckTaskComplete(int mischiefID) { return default; }
}

