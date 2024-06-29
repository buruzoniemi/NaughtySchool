using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Time_Manager : Singleton_Class<Time_Manager>
{
    //変数宣言--------------------------------------------------
    [SerializeField, Header("ストップウォッチの制限時間を入れてください")] private const float limitTime = 120.0f;  //制限時間の値
    private float currentTimer = 0.0f; //制限時間の現在の値
   //----------------------------------------------------------

    protected new void Awake()
    {
        base.Awake(); // 親クラスのAwakeメソッドを呼び出す
    }


    void Start()
    {
        //制限時間を設定する
        currentTimer = limitTime;
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime();
    }

    private void ElapsedTime()
    {
        //Maxのタイマーから経過時間を削る
        currentTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 現在の時間を渡す
    /// </summary>
    /// <returns></returns>
    public override float SendNowTime()
    {
        return currentTimer;
    }

    /// <summary>
    ///制限時間を渡す
    /// </summary>
    /// <returns></returns>
    public override float SendLimitTime()
    {
        return limitTime;
    }
}