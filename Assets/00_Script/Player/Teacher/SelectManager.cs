using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
	[SerializeField] public GameObject[,] studentObjects; // プレハブを格納
	[SerializeField] private GameObject targetIndication; // ターゲット指示
	[SerializeField] private GameObject specialTargetIndication;// 特殊ターゲット指示
	[SerializeField] private GameObject[] specialTarget; //特殊モードの時のターゲット
	[SerializeField] private Text stopTimeText; //選択ミスした後のStopTime中に出すText
	[SerializeField] private Text specialTimeText; //SpecialTime中に出すText
	[SerializeField] private Text selectSuccess; //摘発成功text
	[SerializeField] private Image darkPanel; //選択ミスしたときの暗い演出用のキャンバス
	[SerializeField] private InGame_Spawner inGameSpawner; //InGameの生徒を生成する処理が入ってるスクリプト
	[SerializeField] private LessonProgress lessonProgress; //授業進捗を管理するスクリプト
	[SerializeField] private FrontAndBackCamera frontAndBackCamera; //カメラを前後に切り替える用のスクリプト
	[SerializeField] private TeacherAnim_Controller teacherAnimController; //アニメーション管理用のスクリプト
	[SerializeField] private FrontCameraMove FrontCameraMove; //カメラを回転する用のスクリプト

	private PlayMischief playMischief;

	public static int currentTargetRows; // 選択中の目標（行）
	public static int currentTargetClos; // 選択中の目標（列）
	public static bool[,] studentInAction; // 学生を摘発されて特殊行動に入る

	private static readonly int rows = 3; // 行の数
	private static readonly int cols = 5; // 列の数

	private bool isChangeCamera; // 生徒に向くカメラに切り替えるかどうか
	private bool isInputEnabled; //入力を受け付ける状態かどうか
	private int exposurePlayerCount; //摘発したPlayerの数を数える変数
	private float targetHeight;  // ターゲット指示の高さ
	private float inputStopTime; // 入力を止める時間
	private float startInputStopTime; // 一定時間操作できないようにする時間を格納するもの
	private float normalTargetYPos;   //ノーマルターゲット座標Yを格納する

	private bool[] isSpecialTimesUp; //特殊行動の回数を増加したか
	private bool[] canSpecialTimesUp; //特殊行動の回数を増加できるか
	private bool isInSpecialActionTime;//特殊行動に入るか
	private bool specialActionChange;  //特殊タイム行動対応
	private int specialActionTimes;    //特殊行動できる回数
	private float specialInActionTime; //一回特殊行動に入る時間

	private float selectSuccessTime; //文字を表示する時間
	private bool isSelectSuccess;    //文字表示用

	public AudioSource[] teacherSelectSound;//摘発する音

	private bool isTeachingCamera; //授業中をチェックする
	private bool isDollyCheck;      //ドリーカートのチェックを始めるか確認する	


	void Start()
	{
		exposurePlayerCount = 0;
		targetHeight = 1.2f;
		startInputStopTime = 0.0f;
		inputStopTime = 3.0f;
		currentTargetRows = 0;
		currentTargetClos = 0;
		studentInAction = new bool[3, 5];
		isInputEnabled = true;
		specialActionChange = false;
		specialActionTimes = 0;
		isSelectSuccess = false;
		isTeachingCamera = false;
		isInSpecialActionTime = false;
		isSpecialTimesUp = new bool[3] { true, true, true };
		canSpecialTimesUp = new bool[3] { false, false, false };
		specialInActionTime = 10.0f;
		specialTimeText.text = "";
		teacherAnimController = this.GetComponent<TeacherAnim_Controller>();
		isDollyCheck = false;


		//生徒オブジェクトのActionのフラグをfalseに初期化する
		for (int i = 0; i < studentInAction.GetLength(0); i++)
		{
			for (int j = 0; j < studentInAction.GetLength(1); j++)
			{
				studentInAction[i, j] = false;
			}
		}

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

		specialTarget = new GameObject[cols];

		//Textの表示を消す
		stopTimeText.gameObject.SetActive(false);
		darkPanel.gameObject.SetActive(false);

		//選択するオブジェクトを生成する
		targetIndicationSpawn();
	}

	void Update()
	{
		UpdateTargetIndication();
		// Debug.Log("isInputEnabled：" + $"{isInputEnabled}");
		StopDeltaTime(inputStopTime);
		SelectSuccess();
		//ここにドリーカート呼ぶ関数をさす
		if(isDollyCheck)
		{
			playMischief = studentObjects[currentTargetRows, currentTargetClos].GetComponent<PlayMischief>();
			playMischief.isDollyCartSetPath();
		}
		//授業中をチェックする
		isTeachingCamera = FrontCameraMove.SendTeacherToward();
	}

	// 以下入力処理------------------------------------------------------
	/// <summary>
	/// 入力があった時目線から左の目標に切り替える処理
	/// </summary>
	/// <param name="context"></param>
	public void OnObjectChangeLeft(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (isTeachingCamera) return;
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す
		NowCameraChack();

		if (!specialActionChange)
		{
			if (isChangeCamera)
			{
				//Debug.Log("左のターゲットに切り替える");
				SwitchObject(0, -1); // 目線から左の目標に切り替え
			}
			else
			{
				//Debug.Log("左のターゲットに切り替える");
				SwitchObject(0, 1);  // 目線から左の目標に切り替え
			}
		}
		else
		{
			if (isChangeCamera)
			{
				//Debug.Log("右のターゲットに切り替える");
				SpecialSwitchObject(-1); // 視線から右の目標に切り替え
			}
			else
			{
				//Debug.Log("右のターゲットに切り替える");
				SpecialSwitchObject(1);  // 目線から右の目標に切り替え
			}
		}
	}

	/// <summary>
	/// 入力があった時目線から右の目標に切り替える処理
	/// </summary>
	/// <param name="context"></param>
	public void OnObjectChangeRight(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (isTeachingCamera) return;
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す
		NowCameraChack();

		if (!specialActionChange)
		{
			if (isChangeCamera)
			{
				//Debug.Log("右のターゲットに切り替える");
				SwitchObject(0, 1); // 視線から右の目標に切り替え
			}
			else
			{
				//Debug.Log("右のターゲットに切り替える");
				SwitchObject(0, -1);  // 目線から右の目標に切り替え
			}
		}
		else
		{

			if (isChangeCamera)
			{
				//Debug.Log("右のターゲットに切り替える");
				SpecialSwitchObject(1); // 視線から右の目標に切り替え
			}
			else
			{
				//Debug.Log("右のターゲットに切り替える");
				SpecialSwitchObject(-1);  // 目線から右の目標に切り替え
			}
		}

	}

	/// <summary>
	/// 入力があった時目線から前の目標に切り替える
	/// </summary>
	/// <param name="context"></param>
	public void OnObjectChangeFront(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (isTeachingCamera) return;
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す
		NowCameraChack();

		if (!specialActionChange)
		{
			if (isChangeCamera)
			{
				//Debug.Log("前のターゲットに切り替える");
				SwitchObject(1, 0);// 視線から前の目標に切り替える
			}
			else
			{
				//Debug.Log("前のターゲットに切り替える");
				SwitchObject(-1, 0);// 視線から前の目標に切り替える
			}
		}

	}

	/// <summary>
	/// 入力があった時目線から後ろの目標に切り替える処理
	/// </summary>
	/// <param name="context"></param>
	public void OnObjectChangeBack(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (isTeachingCamera) return;
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す
		NowCameraChack();

		if (!specialActionChange)
		{
			if (isChangeCamera)
			{
				//Debug.Log("後ろのターゲットに切り替える");
				SwitchObject(-1, 0);// 視線から前の目標に切り替える
			}
			else
			{
				//Debug.Log("後ろのターゲットに切り替える");
				SwitchObject(1, 0);// 視線から前の目標に切り替える
			}
		}

	}

	/// <summary>
	/// ボタンが押されたら、選択中の目標の行動を実行
	/// </summary>
	/// <param name="context"></param>
	public void OnObjectSelect(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (isTeachingCamera) return;
		// Debug.Log("摘発されるボタンが押されてるよ");
		//アニメーションの呼び出し
		teacherAnimController.AnimUsePoint();

		if (specialActionChange)
		{
			//特殊行動の回数があればと今生徒側が特殊行動に入ってないで特殊行動できる
			if (specialActionTimes > 0 && !isInSpecialActionTime)
			{
				teacherAnimController.AnimReadAloud();
				SelectSoundCreate();// 音声生成
				SpecialTimeObjectSelect();//特殊行動の処理
				specialActionTimes--;//特殊行動の回数を減らす
				isInSpecialActionTime = true;  //特殊行動に入ってる
				if (specialActionTimes <= 0)
				{
					specialActionTimes = 0;
				}
			}
		}
		else
		{
			SelectSoundCreate();// 音声生成
			ObjectSelect();  //普通の摘発する処理
		}
	}

	public void OnEmperorTimeCheck(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (isTeachingCamera) return;

		if (specialActionTimes > 0)
		{
			specialActionChange = !specialActionChange; //特殊行動と普通の選択を切り替える
		}
	}


	// 以下ターゲット選択処理------------------------------------------------------------------------------------------------------

	/// <summary>
	/// 選択中の目標の行動
	/// </summary>
	private void ObjectSelect()
	{
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す
		if (studentObjects == null) return; //生徒オブジェクトが格納されてなかったら処理を返す
		Debug.Log("選択する処理");
		Debug.Log("スペシャルタイムじゃない");

		//選択したものがNPCなら停止処理を送る
		if (studentObjects[currentTargetRows, currentTargetClos].tag == "Student_NPC")
		{
			//間違えたというFlagを立てる処理
			DisableMoveFlagCheck();
			//アニメーションを呼び出す
			teacherAnimController.AnimHit(isSelectSuccess);
			return;
		}

		// 通常タイムであるか、特殊タイムで黒板や生徒がいない場合、選択中の目標をログに表示
		Debug.Log("現在の選択オブジェクト：" + studentObjects[currentTargetRows, currentTargetClos].name);
		playMischief = studentObjects[currentTargetRows, currentTargetClos].GetComponent<PlayMischief>();
		//もし生徒がいたずらQTE中だったら
		if (playMischief.ExposedMischief() == true)
		{
			//摘発された後の処理を呼び出す。
			playMischief.ExposedMischief();
			exposurePlayerCount++;
			isSelectSuccess = true;

			//この下後参加している見つけたPlayerを数えて参加人数と同じ数になったら
			//先生の勝利を送る
			if (StageSelectManager.studentPlayerQuantites == exposurePlayerCount)
			{
				LessonManager.instance.WinnerDecided(1);
			}

		}
		else
		{
			Debug.Log("5秒間動けない");
			//間違えたというFlagを立てる処理
			DisableMoveFlagCheck();
		}
		isDollyCheck = true;
		//アニメーションを呼び出す
		teacherAnimController.AnimHit(isSelectSuccess);
	}

		/// <summary>
		/// specialTime中の選択中の目標の行動
		/// </summary>
		private void SpecialTimeObjectSelect()
	{
		Debug.Log("選択する処理");
		Debug.Log("SpecialTime中だぞ");
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す

		Debug.Log("スペシャルタイムなう");
		// 特殊タイムで生徒がいる場合、生徒の目標を選択
		// 選択されている生徒の目標
		for (int j = 0; j < studentInAction.GetLength(1); j++)
		{
			studentInAction[currentTargetRows, j] = true;

			//後で立つアニメーションの処理を追加
			studentObjects[currentTargetRows, j].GetComponent<StudentAnimContoroller>().AnimStartReadAloud();

		}

		for (int i = 0; i < studentObjects.GetLength(1); i++)
		{
			if (studentObjects[currentTargetRows, i].tag == "Student")
			{
				Debug.Log("特殊処理中に以下の処理を通す");
				playMischief = studentObjects[currentTargetRows, i].GetComponent<PlayMischief>();
				//摘発の処理が入るように作っておいてね♡
				if (playMischief.ExposedMischief() == true)
				{
					//摘発された後の処理を呼び出す。
					playMischief.ExposedMischief();
					exposurePlayerCount++;
					isSelectSuccess = true;

					//この下後参加している見つけたPlayerを数えて参加人数と同じ数になったら
					//先生の勝利を送る
					if (StageSelectManager.studentPlayerQuantites == exposurePlayerCount)
					{
						LessonManager.instance.WinnerDecided(1);
					}
				}
				Debug.Log("InTime!");
			}
		}
	}

	/// <summary>
	/// ターゲット指示の更新
	/// </summary>
	private void UpdateTargetIndication()
	{
		if (specialActionTimes <= 0) //特殊行動回数がなくなる
		{
			specialActionChange = false;  //特殊行動に入れない
		}
		if (!specialActionChange)
		{
			NormalTargetIndication();
		}
		else
		{
			SpecialTargetIndication();
		}

		SpecialTimeLimit();
		SpecialTimesUp();

	}

	/// <summary>
	/// 指示用のモデルの上下移動
	/// </summary>

	/// <summary>
	/// 特殊ターゲットの指示
	/// </summary>
	private void SpecialTargetIndication()
	{
		Debug.Log("SpecialYes");
		//単体選択するオブジェクトを消す
		if (targetIndication != null)
		{
			targetIndication.SetActive(false);
		}

		for (int i = 0; i < cols; i++)
		{
			//if (studentObjects[currentTargetRows, i] == null) return;
			if (studentObjects[currentTargetRows, i] != null)
			{
				Vector3 targetPos = studentObjects[currentTargetRows, i].transform.position + new Vector3(0, targetHeight, 0);

				if (specialTarget[i] == null)
				{
					specialTarget[i] = Instantiate(specialTargetIndication, targetPos, Quaternion.identity);
				}
				else
				{
					specialTarget[i].transform.position = targetPos;
					specialTarget[i].SetActive(true);
					float moveUpDown = Mathf.Sin(Time.time * 5f) * 0.05f;  //上下移動距離
					Vector3 tempPos = specialTarget[i].transform.position; //今のターゲット座標を入力
					tempPos.y = normalTargetYPos + moveUpDown + targetHeight; //ターゲット座標Yを更新
					specialTarget[i].transform.position = tempPos; //上下移動処理
				}
			}
			else
			{
				if (specialTarget[i] != null)
				{
					specialTarget[i].SetActive(false);
				}

			}
			//specialTarget[i].SetActive(true);

		}
	}

	/// <summary>
	/// 通常ターゲットの指示
	/// </summary>
	private void NormalTargetIndication()
	{
		//Debug.Log($"Indicationは{currentTargetRows},{currentTargetRows}のターゲットの上にいます。");
		// 特殊ターゲットの表示を非アクティブにする
		for (int j = 0; j < cols; ++j)
		{
			if (specialTarget[j] != null)
			{
				specialTarget[j].SetActive(false);
			}
		}

		for (int i = 0; i < studentObjects.GetLength(0); ++i)
		{
			for (int j = 0; j < studentObjects.GetLength(1); ++j)
			{
				if (studentObjects[i, j] == null) return;
				// ターゲット指示が可能な場合、ターゲット指示の位置を上下に動かす
				targetIndication.SetActive(true); //
				float moveUpDown = Mathf.Sin(Time.time * 5f) * 0.05f;  //上下移動距離
				Vector3 tempPos = targetIndication.transform.position; //今のターゲット座標を入力
				tempPos.y = normalTargetYPos + moveUpDown + targetHeight; //ターゲット座標Yを更新
				targetIndication.transform.position = tempPos; //上下移動処理
															   //Debug.Log("NormalYes");
			}
		}
	}

	// 以下ヘルパーメソッド------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// 現在のカメラ状況をチェックする処理
	/// </summary>
	private void NowCameraChack()
	{
		isChangeCamera = frontAndBackCamera.SendIsCameraChange();
	}
	/// <summary>
	//  目標を切り替える処理
	/// </summary>
	/// <param name="changeX"> X方向の値 </param>
	/// <param name="changeY"> Y方向の値 </param>
	private void SwitchObject(int changeX, int changeY)
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

	private void SpecialSwitchObject(int changeY)
	{
		// 現在のターゲットの行と列を計算します
		int nowRow = currentTargetRows;

		// 列を変更します
		nowRow += changeY;

		// 列が負の場合、最後の列にループします
		if (nowRow < 0) nowRow = rows - 1;
		// 列が列の数を超える場合、最初の列にループします
		else if (nowRow >= rows) nowRow = 0;

		// 新しいターゲットを計算します
		currentTargetRows = nowRow;

		UpdateSpecialTargetPosition();
	}
	//特殊ターゲット生成
	private void UpdateSpecialTargetPosition()
	{
		for (int i = 0; i < specialTarget.Length; i++)
		{
			if (studentObjects[currentTargetRows, i] != null)
			{
				specialTarget[i].transform.position = new Vector3(
					studentObjects[currentTargetRows, i].transform.position.x,
					studentObjects[currentTargetRows, i].transform.position.y + targetHeight,
					studentObjects[currentTargetRows, i].transform.position.z
				); //特殊ターゲットの座標を更新

				specialTarget[i].SetActive(true);
			}
			else
			{
				specialTarget[i].SetActive(false);
			}
		}
	}



	/// <summary>
	/// 引数秒間動けなくする処理を起動する用のもの
	/// </summary>
	/// <param name="seconds"> </param>
	/// <returns></returns>
	private void DisableMoveFlagCheck()
	{
		// 入力を受け付けないようにフラグを設定する
		isInputEnabled = false;
		//Debug.Log($"{inputStopTime}" + "秒間待ち始める");
	}

	/// <summary>
	/// 選択中の生徒を分かりやすくするためのオブジェクトの生成
	/// </summary>
	private void targetIndicationSpawn()
	{
		//Debug.Log("targetIndicationを生成");
		//生成処理
		if (studentObjects != null && studentObjects.GetLength(0) > 0 && studentObjects[0, 0] != null)
		{
			Vector3 studentPos = studentObjects[0, 0].transform.position; //デフォルト生徒の座標を記録
			normalTargetYPos = studentPos.y; //ターゲット座標Yを入力
			Vector3 targetPos = new Vector3(studentPos.x, studentPos.y + targetHeight, studentPos.z); //ターゲット座標更新
			targetIndication = Instantiate(targetIndication, targetPos, Quaternion.identity);//ターゲット生成
		}
	}
	/// <summary>
	/// 引数秒間止めておく処理
	/// </summary>
	/// <param name="StopTime">止める時間</param>
	private void StopDeltaTime(float StopTime)
	{
		if (isInputEnabled == true) return;
		//Textとキャンバスを表示する
		stopTimeText.gameObject.SetActive(true);
		darkPanel.gameObject.SetActive(true);

		startInputStopTime += Time.deltaTime;
		stopTimeText.text = "ミスしちゃった！" + "\n" + $"{(int)StopTime - (int)startInputStopTime}" + "秒間待機！";
		//Debug.Log("startInputStopTime：" + $"{startInputStopTime}");
		if (startInputStopTime > StopTime)
		{
			// 待機が終了したら、入力を受け付けるようにフラグを再設定する
			isInputEnabled = true;
			//Debug.Log("3秒経ったよ！");
			startInputStopTime = 0.0f;
			//Textのとキャンバスの表示を消す
			stopTimeText.gameObject.SetActive(false);
			darkPanel.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// 摘発成功の処理
	/// </summary>
	private void SelectSuccess()
	{
		if (isSelectSuccess)
		{
			selectSuccessTime += Time.deltaTime; //時間計算
			if (selectSuccessTime <= 2.0f) //２秒表示
			{
				selectSuccess.text = "摘発成功";
			}
			else
			{
				selectSuccessTime = 0.0f;
				selectSuccess.text = "";
				isSelectSuccess = false;
			}
		}

	}

	//摘発する音を生成
	private void SelectSoundCreate()
	{
		int randomSoundNumber = Random.Range(0, 3);  //3種音声からランダムで生成
		for (int i = 0; i < teacherSelectSound.Length; i++)
		{
			if (i == randomSoundNumber)
			{
				teacherSelectSound[i].Play();
			}
		}
	}

	//特殊行動の時間制限
	private void SpecialTimeLimit()
	{
		if (isInSpecialActionTime)
		{
			specialInActionTime -= Time.deltaTime;//時間計算
			if (specialActionTimes > 0)
			{
				specialTimeText.text = specialInActionTime.ToString("F1");
			}
			//時間が0になったらリセット
			if (specialInActionTime <= 0.0f)
			{
				isInSpecialActionTime = false; //特殊行動外になる
				specialInActionTime = 10.0f; //時間リセット
				specialTimeText.text = "";
			}
		}
	}

	//特殊行動回数を増やす
	private void SpecialTimesUp()
	{
		canSpecialTimesUp[0] = lessonProgress.SendCanSpecialTimes1();
		canSpecialTimesUp[1] = lessonProgress.SendCanSpecialTimes2();
		canSpecialTimesUp[2] = lessonProgress.SendCanSpecialTimes3();
		//条件を当たったら特殊行動の回数を１増やす
		for (int i = 0; i < canSpecialTimesUp.Length; ++i)
		{
			if (canSpecialTimesUp[i] && isSpecialTimesUp[i])
			{
				specialActionTimes++;
				isSpecialTimesUp[i] = false;
			}
		}

	}

	public bool SendCanSpecialAction()
	{
		//特殊行動の回数があればと特殊行動に入ってない、アイコン表示用
		if (specialActionTimes > 0 && !isInSpecialActionTime)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
