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
    [SerializeField, Header("Maxの時間を入れる時間")] private Text maxTimeLimitText; //制限時間のMax時間のテキストを入れる変数
	[SerializeField] private GameObject stopWatchObject; //StopWatchのオブジェクトを保管する
	[SerializeField] private Image Image; // Rendererコンポーネント

    private bool isRunning; //ストップウォッチの実行状態
    private bool isTimeStop; //タイムになる状態をチェック
    private float startInputStopTime; // 一定時間後に別シーンに移行するための変数
    private float nowSecondsTime; // 現在の時間
	private float quarterSecondsTime; //残り時間が四分の一かどうかを区別する変数
	private Vector3 initialScale; // 初期サイズ

	//private readonly static float nextSceneStopTime = 3.0f; //次のシーンに移行するまでの待機時間
	private readonly string nextResultScene = "ResultScene"; //リザルトシーンに移行する用のstring
	private readonly float maxSize = 1.5f; // 最大サイズ
	private readonly float minSize = 0.7f; // 最小サイズ
	private readonly float speed = 2.5f; // 変化スピード

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
        Time.timeScale = 5.0f;
        isRunning = false; 
        isTimeStop = false; 
        startInputStopTime = 0.0f; 
        nowSecondsTime = 0.0f; 
		quarterSecondsTime = 15.0f; 
		initialScale = stopWatchObject.transform.localScale; //初期の大きさを保存する
		Image.material.color = Color.white;

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
		StopWatchUISizeChange();
		Debug.Log("現在のisRunning：" + $"{isRunning}");
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
        float secondsAngle = seconds * 6.0f / minites; //1秒あたりの角度 (360度 / 60秒)
        nowSecondsTime = seconds;
        //回転処理の呼び出し
        RotateHand(secondHand, secondsAngle);
        //経過時間が0秒を下回ったらフラグを折る
        if(seconds <= 0.0f)
        {
			StopInGameTimer();
			Image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			stopWatchObject.transform.localScale = initialScale;
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
        maxTimeLimitText.text = $"{ Time_Manager.instance.SendLimitTime()}";
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

	/// <summary>
	/// 制限時間が四分の一を切った時、
	/// 大きさを変更し続け色を変更する
	/// </summary>
	private void StopWatchUISizeChange()
	{
		if (nowSecondsTime > quarterSecondsTime) return;
		if (isRunning == false) return;

		float newSize = Mathf.PingPong(Time.time * speed, maxSize - minSize) + minSize; // PingPong関数で変化させるサイズを計算
		stopWatchObject.transform.localScale = initialScale * newSize; //サイズの変更

		// 大きくなった時赤くする
		if (newSize > initialScale.magnitude)
		{
			Image.color = new Color(1.0f, 0.5f, 0.5f , 1.0f);
		}
		else // 小さくなった時元の色に戻す
		{
			Image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
	}
}
