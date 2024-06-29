using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayMischief : MonoBehaviour
{
    // 見にくいので後で直す(スクリプト分割も視野)
    [Header("--- 調整用パラメータ ---")]
    [Header("コマンドの入力時間")]
    [SerializeField] float commandInputTime;                        // コマンド入力時間
    [Header("ペナルティ")]
    [SerializeField] float mistakeDelayTime;                        // ミスした時の待ち時間
	[Header("QTE時のカメラ角度")]
	[SerializeField] float mischiefCameraAngle;
	[SerializeField] float cameraSpeed;

	[Header("--- 初期設定 ---")]
    [Header("ごまかしコマンドのスクリプト")]
    [SerializeField] private DeceiveMischief deceiveMischief;       // ごまかし
    [Header("パス上を歩くドリーカートのスクリプト")]
    [SerializeField] private DollyCartController dollyCart;
    [Header("OperationUIのスクリプトを入れる")]
    [SerializeField] private OperationController operation;         // UI表示
    [Header("正誤判断を表示するUIのスクリプト")]
    [SerializeField] private JudgeUIController judge;
    [Header("プログレスサークルUIのスクリプト")]
    [SerializeField] private ProgressCircleController gauge;
	// Reiwa's Shit Code Season 3
	[SerializeField] private GameObject ownedCamera;

	[Header("--- デバッグ用情報 ---")]
    [Header("実行中のいたずら")]
    [SerializeField] private Mischief inProgressCommand;            // コマンドリスト
    [Header("デバイス名")]
    [SerializeField] private string myDevice;                       // デバイス名
    private float inputTime;                                        // 入力時間のカウンター
    private float delayTime;                                        // 待ち時間のカウンター
    private int stepCounter;                                        // コマンドの進捗カウンター
    private bool isQTEActive;                                       // StudentMischiefから受け取る
    private bool canInput;                                          // 入力できるか
	private bool isLookup;

    public delegate void DisableQTE(Mischief mischief);
    public delegate void ExposedQTE(Mischief mischief);
    private DisableQTE disableQTECallBack;
    private ExposedQTE exposedQTECallBack;

	public Text gameOverText; //ゲームオーバーの時表示する文字

    // Start is called before the first frame update
    void Start()
    {
        inProgressCommand.None();
        stepCounter = 0;
        canInput = false;
        isQTEActive = false;
		isLookup = false;
		gameOverText.text = "";

	}

    // フレーム毎の更新処理
    void Update()
    {
        // QTEが有効化されるまで処理しない
        if (!isQTEActive) return;
		Rotate();
        // タイムオーバーした時の処理
        if (!canInput)
        {
            MistakeDelay();
            return;
        }

        // タイマーの加算
        inputTime += Time.deltaTime;
        // 受付時間内の処理
        if (inputTime > commandInputTime)
        {
            ResetCommand();    // コマンドのリセット
        }
    }

    /// <summary>
    /// デリゲートの初期化
    /// </summary>
    /// <param name="onDisableQTE">コールバックに設定する関数</param>
    public void initDisableQTE(DisableQTE onDisableQTE)
    {
        disableQTECallBack = onDisableQTE;
    }

    public void initExposedQTE(ExposedQTE onExposedQTE)
    {
        exposedQTECallBack = onExposedQTE;
	}


    // QTEコマンドを有効化(MischiefSelectorが有効化)
    public void OnEnableQTE(Mischief mischief)
    {
        // QTEが有効だったら処理しない
        if (isQTEActive) return;
        // QTEを有効
        isQTEActive = true;
        canInput = true;
        // 受け取ったMischiefを入れる
        inProgressCommand = mischief;
        // ごまかしコマンドのデリゲート設定
        deceiveMischief.init(OnDisableQTE);
		// カメラの角度変える
		ownedCamera.transform.rotation = Quaternion.Euler(mischiefCameraAngle,
			transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		// UI表示
		operation.EnableCanvas();
        judge.EnableCanvas();
        gauge.EnableCanvas();
        gauge.ActiveGauge(commandInputTime);
        ChangeCommandOperation();

		//アニメーションを呼び出す
		this.GetComponent<StudentAnimContoroller>().AnimNaughtyNum(inProgressCommand.name);
		this.GetComponent<StudentAnimContoroller>().AnimDoNaughty();
    }

    // QTE中かチェック
    public bool GetIsQTEActive()
    {
        return isQTEActive;
    }

    /// <summary>
    /// QTEの無効化
    /// </summary>
    public void OnDisableQTE()
    {
        // フラグを切る
        isQTEActive = false;
        // 各種変数の初期化 
        stepCounter = 0;
        inputTime = 0;
		ownedCamera.transform.rotation = Quaternion.Euler(0.0f,
			transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		// 実行中フラグを切る
		inProgressCommand.isPlay = false;
        // UI消す
        operation.DisableCanvas();
        judge.DisableCanvas();
        gauge.DisableCanvas();
        //neck.DisableRotation();
        // コールバック返す
        disableQTECallBack(inProgressCommand);
    }

    // 入力チェック
    // ※バグあり(いたずら選択時に同時に処理されて失敗判定が出る)
    public void CheckCommand(InputAction.CallbackContext context)
    {
        // 押された瞬間だけ呼ぶ
        if (!context.started) return;
        // QTEが有効化されてなければ呼ばない
        if (!isQTEActive) return;
        // インプット
        if (!canInput) return;

        //入力されたボタンのパス
        string controlPath = context.control.path;
        Debug.Log($"{controlPath}");

        if (stepCounter < 0 || stepCounter >= inProgressCommand.commandNum)
        {
            Debug.LogError($"currentInputIndex が配列の要素数に合ってないよ！！ {stepCounter}"); // エラーメッセージを出力
            return; // 処理を中断する
        }

        //現在の入力されるべきボタンをログで確認
        string answer = "/" + myDevice + "/" + inProgressCommand.commandList[stepCounter];
        Debug.Log($"Input: {"/" + myDevice + "/" + inProgressCommand.commandList[stepCounter] }");
        //入力されたボタンのパスが配列に用意したものと合致しているかどうか
        if (controlPath == answer)
        {
            Debug.Log($"QTE：{stepCounter + 1}回目成功");
            // 成功を表示
            judge.EnableUI(true);
            judge.SetIsFadeOut(0);
            //配列の要素数を確認するプロパティをインクリメントする
            stepCounter++;
            //もし配列の要素数を確認するプロパティが配列の要素数を超えたら以下の処理
            if (stepCounter >= inProgressCommand.commandNum)
            {
                //QTEが完了処理
                OnCompQTE();
				//アニメーションを呼び出す
				this.GetComponent<StudentAnimContoroller>().AnimEndNaughty();
            }
            else
            {
                //次に入力されるべきボタンをログで確認
                Debug.Log($"Input: { "/" + myDevice + "/" + inProgressCommand.commandList[stepCounter] }");
                // UIの変更
                ChangeCommandOperation();
            }
        }
        else
        {
            judge.EnableUI(false);
            Debug.Log("QTE失敗");
            //QTEをリセットする処理の呼び出し
            ResetCommand();
        }
    }

	// カメラ
	public void CameraRotate(InputAction.CallbackContext context)
	{
		switch (context.phase)
		{
			case InputActionPhase.Performed:
				isLookup = true;
				break;

			case InputActionPhase.Canceled:
				isLookup = false;
				break;
		}
	}

	private void Rotate()
	{
		if (isLookup)
		{
			ownedCamera.transform.rotation = Quaternion.RotateTowards(ownedCamera.transform.rotation,
																	Quaternion.Euler(0, 90.0f, 0),
																	cameraSpeed * Time.deltaTime);
		}
		else
		{
			ownedCamera.transform.rotation = Quaternion.RotateTowards(ownedCamera.transform.rotation,
																	Quaternion.Euler(mischiefCameraAngle, 90.0f, 0),
																	cameraSpeed * Time.deltaTime);
		}
	}


	// 次のコマンドの設定
	private void ChangeCommandOperation()
    {
        // dpad(十字キー)の為の文字列分割処理
        string[] vs = inProgressCommand.commandList[stepCounter].Split("/");
        int dpadNum = 0;
        // 十字キー入力の場合区切りの後ろを読む 
        if (vs[0] == "dpad") dpadNum = 1;
        //
        for (KeyValue value = 0; value < KeyValue.Length; value++)
        {
            if (value.ToString() == vs[dpadNum])
            {
                operation.EnableUI(value);
                break;
            }
        }
    }

    // ミスした時の待ち時間
    private void MistakeDelay()
    {
        delayTime += Time.deltaTime;
        if (delayTime > mistakeDelayTime)
        {
            delayTime = 0.0f;
            canInput = true;
            Debug.Log("Input OK");
            gauge.EnableCanvas();
            gauge.ActiveGauge(commandInputTime);
            operation.EnableCanvas();
            // UIの変更
            ChangeCommandOperation();
            judge.SetIsFadeOut(1);

        }
    }
    // コマンドのリセット
    private void ResetCommand()
    {
        Debug.Log("Reset");
        // 現在の入力を切る

        // stepを戻す
        stepCounter = 0;
        // UIオフ
        gauge.DisableCanvas();
        operation.DisableCanvas();
        // タイマーのリセット
        inputTime = 0;
        // 入力待機時間を作る
        canInput = false;
    }

    // QTE成功時の処理
    private void OnCompQTE()
    {
        // フラグの設定
        inProgressCommand.isComp = true;
        judge.CompUI();
        // QTEの無効化
        judge.initCompUI(OnDisableQTE);

    }

    /// <summary>
    /// いたずらが摘発された時に呼ばれる処理
    /// (先生側でカウントする時のためにbool返すとかでもいいかも？)
    /// // いつか直そうね
    /// </summary>
    public bool ExposedMischief()
    {
        // いつか直そうね

        // いたずら中じゃなければ返す
        if (!isQTEActive) return false;
        deceiveMischief.enableDeceive = false;

		this.GetComponent<StudentAnimContoroller>().AnimPickUp();

		// フラグを切る
		isQTEActive = false;
        // 各種変数の初期化 
        stepCounter = 0;
        inputTime = 0;
        // 実行中フラグを切る
        inProgressCommand.isPlay = false;
        // UI消す
        operation.DisableCanvas();
        judge.DisableCanvas();
        gauge.DisableCanvas();
        // Exit ClassRoom
        dollyCart.SetPath();

        // コールバック返す
        exposedQTECallBack(inProgressCommand);

		//ゲームオーバー文字を表示
		gameOverText.text = "ばれました";

		return true;
    }

    /// <summary>
    /// デバイス名を記録
    /// 呼び出し MishicefSelector.cs
    /// </summary>
    /// <param name="name">デバイス名</param>
    public void SetDeviceName(string name) { myDevice = name; }
}