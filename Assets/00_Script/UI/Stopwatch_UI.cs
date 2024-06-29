using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//これをUIキャンバスに適切に配置し、
//ボタンやテキストなどのUI要素を用意してアタッチすれば、
//アナログスタイルのストップウォッチを実装できるはず

public class Stopwatch_UI : MonoBehaviour
{
    // 変数宣言------------------------------------------------------------------------
    [SerializeField,Header("針のPrafubを入れてください")] private RectTransform secondHand; //針のPrafub(UIの)
    [SerializeField, Header("半分の時間を入れる時間")] private Text halfTimeLimitText; //制限時間の半分時間のテキストを入れる変数
    [SerializeField, Header("Maxの時間を入れる時間")] private Text MaxTimeLimitText; //制限時間のMax時間のテキストを入れる変数

    private bool isRunning; //ストップウォッチの実行状態
    private bool isTimeStop; //タイムになる状態をチェック
    private float startInputStopTime; // 一定時間後に別シーンに移行する処理
    private float nowSecondsTime; 

    //private readonly static float nextSceneStopTime = 3.0f; //次のシーンに移行するまでの待機時間
    private readonly static string nextResultScene = "ResultScene"; //リザルトシーンに移行する用のstring
	private static readonly float secondHandRotationAngleValue = 6.0f;

	private bool teacherWin;
    private bool studentWin;
    //---------------------------------------------------------------------------------

    /// <summary>
    /// スタート時に処理される内容
    /// </summary>
    private void Start()
    {
        //ゲームのスピードを倍化している
        //後で消して
        //Time.timeScale = 5.0f;
        isRunning = false;
        startInputStopTime = 0.0f;
        nowSecondsTime = 0.0f;
        isTimeStop = false;

        //ストップウォッチをスタート
        StartInGameTimer();
        //ストップウォッチの半分の時間を表示
        HalfTimeDisplay();
        MaxTimeDisplay();

        //初期化
        teacherWin = false;
        studentWin = false;
    }

    /// <summary>
    /// 実行状態なら
    /// 経過時間に応じて秒針を回転 
    /// </summary>
    void Update()
    {
        //勝者雅出る前に時間計算
        if(!teacherWin && !studentWin) UpdateInGameTimerUI();
        StopDeltaTime();
    }

    /// <summary>
    /// ストップウォッチを開始
    /// </summary>
    public void StartInGameTimer()
    {
        isRunning = true;
    }

    /// <summary>
    /// ストップウォッチを停止
    /// </summary>
    public void StopInGameTimer()
    {
        isRunning = false;
    }

    /// <summary>
    /// 経過時間に応じて秒針を回転
    /// </summary>
    /// <param name="time">ストップウォッチの経過時間</param>
    void UpdateInGameTimerUI()
    {
        teacherWin = LessonManager.teacherWin;
        studentWin = LessonManager.studentWin;
        if (isRunning == false) return;

        //Debug.Log("針の回転処理");
        //Debug.Log($"現在時間：{Time_Manager.instance.SendNowTime()}");
        float seconds = Time_Manager.instance.SendNowTime(); //ストップウォッチの経過時間から算出する
        float minites = Time_Manager.instance.SendLimitTime() / 60.0f;
        float secondsAngle = seconds * secondHandRotationAngleValue / minites; //1秒あたりの角度 (360度 / 60秒)
        nowSecondsTime = seconds;
        //回転処理の呼び出し
        RotateHand(secondHand, secondsAngle);
        //経過時間が0秒を下回ったらフラグを折る
        if(seconds <= 0.0f)
        {
            isRunning = false;
        }
    }

    /// <summary>
    /// 秒針を回転させる処理
    /// </summary>
    /// <param name="hand">針の位置</param>
    /// <param name="angle">針の回転座標？</param>
    void RotateHand(RectTransform hand, float angle)
    {
        //Debug.Log($"回転角度：{angle}");
        //回転処理
        hand.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    /// <summary>
    /// 半分の時間を表示する
    /// </summary>
    private void HalfTimeDisplay()
    {
        //テキストに表示
        halfTimeLimitText.text = $"{ Time_Manager.instance.SendLimitTime() / 2}";
    }

    /// <summary>
    /// 最大の時間を表示する
    /// </summary>
    private void MaxTimeDisplay()
    {
        //テキストに表示
        MaxTimeLimitText.text = $"{ Time_Manager.instance.SendLimitTime()}";
    }

    /// <summary>
    /// 引数秒間止めておく処理
    /// </summary>
    /// <param name="StopTime">止める時間</param>
    private void StopDeltaTime()
    {
        if (nowSecondsTime >= 0.0f) return;
        isTimeStop = true;
    }

	public bool SendTimeStop()
	{
		return isTimeStop;
	}
}
