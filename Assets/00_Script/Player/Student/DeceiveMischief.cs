using UnityEngine;
using UnityEngine.InputSystem;

/*
 * 生徒側のプレイヤーがいたずらをごまかすスクリプト
 * 
 * 指定されたボタンの入力を受け取りカウントする
 * 回数を記録した変数が設定した必要数を超えたときコールバックを返す
 * 
 * ごまかすたびに必要連打数を増やす
 */
public class DeceiveMischief : MonoBehaviour
{
    [Header("連打回数")]
    [SerializeField] private int mashButtonNum;         // 連打させる回数
    [Header("初期値")]                                     
    [SerializeField] private int mashInitialNum;        // 連打数初期値
    [Header("1回辺りの増加量")]                            
    [SerializeField] private int mashIncreaseNum;       // 基礎増加数
    [Header("上昇倍率")]                                   
    [SerializeField] private int mashIncreaseRate;      // 連打数上昇倍率
                                                           
    private int DeceiveActionNum;                       // ごまかし行動回数
                                                           
    public bool enableDeceive;                          // ごまかしが完了したかのフラグ
                                                           
    private int pushedNum;                              // 入力された回数を計測する変数

    // デリゲート
    public delegate void CompDeceive();
    private CompDeceive compDeceiveCallBack;

    /// <summary>
    ///  開始時に呼ばれる処理
    /// </summary>
    void Start()
    {
        enableDeceive = false;
        pushedNum = 0;
        mashButtonNum = mashInitialNum;
    }

    /// <summary>
    /// 初期化
    /// デリゲートを設定
    /// </summary>
    /// <param name="onCompDeceive"></param>
    public void init(CompDeceive onCompDeceive) 
    {
        // デリゲートの設定
        compDeceiveCallBack = onCompDeceive;
        // ごまかしの有効化
        enableDeceive = true;
    }

    // フレーム毎の更新処理
    void Update()
    {
        //連打数が必要数を越えたときデリゲートを呼ぶ
        if (pushedNum >= mashButtonNum)
        {
            // 連打数を増やす
            PlusmashNum();
            // 連打回数のリセット
            pushedNum = 0;
            // 無効化
            enableDeceive = false;
            // デリゲートを呼ぶ
            OnCompDeceive();
        }
    }

    /// <summary>
    /// ごまかしボタンが押された時に呼ばれる関数
    /// </summary>
    /// <param name="context"></param>
    public void OnPushedButton(InputAction.CallbackContext context)
    {
        //ボタンが押し込まれたとき以外は関数を終わらす
        if (!context.started) return;
        // 有効化されてなければ返す
        if (!enableDeceive) return;
        // 回数を増やす
        pushedNum++;
    }

    /// <summary>
    /// 必要連打数を調整する関数
    /// </summary>
    private void PlusmashNum()
    {
        //必要連打数を増やす処理
        //必要数 = 初期値 + 増加量 * 上昇倍率^行動回数
        mashButtonNum = mashInitialNum + mashIncreaseNum * mashIncreaseRate ^ DeceiveActionNum;
        //行動回数を増やす
        ++DeceiveActionNum;
    }

    /// <summary>
    /// ごまかしの成功を送る関数
    /// </summary>
    public void OnCompDeceive()
    {
		this.GetComponent<StudentAnimContoroller>().AnimDeceive();
		// QTEを無効化(コールバック)
		compDeceiveCallBack();
        Debug.Log("ごまかし完了");    
    }
}