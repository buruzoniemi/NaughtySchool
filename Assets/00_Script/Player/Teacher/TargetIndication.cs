using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndication : MonoBehaviour
{
	[SerializeField] protected GameObject[,] studentObjects; // プレハブを格納
	[SerializeField] protected GameObject targetIndication; // ターゲット指示
	[SerializeField] private InGame_Spawner inGameSpawner; //InGameの生徒を生成する処理が入ってるスクリプト

	private static readonly int rows = 3; // 行の数
	private static readonly int cols = 5; // 列の数

	private bool isTeachingCamera; //授業中をチェックする
	private float inputStopTime; // 入力を止める時間
	private float startInputStopTime; // 一定時間操作できないようにする時間を格納するもの
	private float normalTargetYPos;   //ノーマルターゲット座標Yを格納する
	private float targetHeight;  // ターゲット指示の高さ

	protected int currentTargetRows; // 選択中の目標（行）
	protected int currentTargetClos; // 選択中の目標（列）
	protected bool isInputEnabled; //入力を受け付ける状態かどうか
	protected bool isSelectSuccess; //摘発に成功したか否か

	// Start is called before the first frame update
	void Start()
    {
		isInputEnabled = true;
		isTeachingCamera = false;
		currentTargetRows = 0;
		currentTargetClos = 0;
		inputStopTime = 3.0f;
		startInputStopTime = 0.0f;
		targetHeight = 1.2f;

		//選択対象の生徒Objectを代入
		studentObjects = new GameObject[rows, cols];
		for (int i = 0; i < studentObjects.GetLength(0); ++i)
		{
			for (int j = 0; j < studentObjects.GetLength(1); ++j)
			{
				studentObjects[i, j] = inGameSpawner.SendStudentArray(i, j);
				Debug.Log($"{studentObjects[i, j]}");
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		StopDeltaTime(inputStopTime);
	}

	/// <summary>
	/// 引数秒間止めておく処理
	/// </summary>
	/// <param name="StopTime">止める時間</param>
	private void StopDeltaTime(float StopTime)
	{
		if (isInputEnabled == true) return;

		startInputStopTime += Time.deltaTime;
		//Debug.Log("startInputStopTime：" + $"{startInputStopTime}");
		if (startInputStopTime > StopTime)
		{
			// 待機が終了したら、入力を受け付けるようにフラグを再設定する
			isInputEnabled = true;
			//Debug.Log("3秒経ったよ！");
			startInputStopTime = 0.0f;
		}
	}

	/// <summary>
	/// 摘発成功の処理
	/// </summary>
	public bool SelectSuccess()
	{
		return isSelectSuccess;
	}

	/// <summary>
	///  目標を切り替える処理
	/// </summary>
	/// <param name="changeX"> X方向の値 </param>
	/// <param name="changeY"> Y方向の値 </param>
	protected void SwitchObject(int changeX, int changeY)
	{
		// 現在のターゲットの行と列を計算します
		int nowRow = currentTargetRows;
		int nowCol = currentTargetClos;

		// 行と列を変更します
		nowCol += changeX;
		nowRow += changeY;

		// 行が負の場合、最後の列にループします
		if (nowCol < 0) nowCol = cols - 1;
		// 行が行の数を超える場合、最初の列にループします
		else if (nowCol >= cols) nowCol = 0;
		// 列が負の場合、最後の列にループします
		if (nowRow < 0) nowRow = rows - 1;
		// 列が列の数を超える場合、最初の列にループします
		else if (nowRow >= rows) nowRow = 0;

		// 新しいターゲットを計算します
		currentTargetRows = nowRow;
		currentTargetClos = nowCol;

		UpdateTargetPosition();
	}


	/// <summary>
	/// ターゲット座標更新
	/// </summary>
	private void UpdateTargetPosition()
	{
		if (studentObjects[currentTargetRows, currentTargetClos] != null)
		{
			Vector3 studentPos = studentObjects[currentTargetRows, currentTargetClos].transform.position; //今の生徒の座標を更新
			normalTargetYPos = studentPos.y;//ターゲット座標Yを更新
			targetIndication.transform.position = new Vector3(studentPos.x, normalTargetYPos + targetHeight, studentPos.z);//ターゲット座標更新
		}
	}
}
