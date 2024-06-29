using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSelect : TargetIndication
{
	[SerializeField] private LessonProgress lessonProgress; //授業進捗を管理するスクリプト
	[SerializeField] private FrontAndBackCamera frontAndBackCamera; //カメラを前後に切り替える用のスクリプト
	[SerializeField] private TeacherAnim_Controller teacherAnimController; //アニメーション管理用のスクリプト
	[SerializeField] private FrontCameraMove FrontCameraMove; //カメラを回転する用のスクリプト

	private bool isChangeCamera; // 生徒に向くカメラに切り替えるかどうか
	private bool isSpecialActionChange; //スペシャルタイム中かどうか
	private int exposurePlayerCount; //摘発したPlayerの数を数える変数
	private float targetHeight;  // ターゲット指示の高さ
	private float normalTargetYPos;   //ノーマルターゲット座標Yを格納する

	void Start()
	{
		exposurePlayerCount = 0;
		targetHeight = 1.2f;
		
		teacherAnimController = this.GetComponent<TeacherAnim_Controller>();

		//選択するオブジェクトを生成する
		targetIndicationSpawn();
	}

	/// <summary>
	/// 生徒選択のオブジェクトを移動させる
	/// </summary>
	private void StudentSelectTargetIndicationChange()
	{
		if (isSpecialActionChange) return;
		NowCameraChack();

		if (isChangeCamera)
		{
			if (InputManager.GetKey(PadButton.Left))
			{
				SwitchObject(0, -1); // 目線から左の目標に切り替え
			}
			if(InputManager.GetKey(PadButton.Right))
			{
				SwitchObject(0, 1); // 目線から右の目標に切り替え
			}
			if (InputManager.GetKey(PadButton.Up))
			{
				SwitchObject(1, 0); // 目線から左の目標に切り替え
			}
			if (InputManager.GetKey(PadButton.Down))
			{
				SwitchObject(-1, 0); // 目線から右の目標に切り替え
			}
		}
		else
		{
			if (InputManager.GetKey(PadButton.Left))
			{
				SwitchObject(0, 1); // 目線から左の目標に切り替え
			}
			if (InputManager.GetKey(PadButton.Right))
			{
				SwitchObject(0, -1); // 目線から右の目標に切り替え
			}
			if (InputManager.GetKey(PadButton.Up))
			{
				SwitchObject(-1, 0); // 目線から左の目標に切り替え
			}
			if (InputManager.GetKey(PadButton.Down))
			{
				SwitchObject(1, 0); // 目線から右の目標に切り替え
			}
		}
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
	/// 選択中の目標の行動
	/// </summary>
	private void StudentSelect()
	{
		if (isInputEnabled == false) return; //入力状態じゃなきゃ処理を返す
		if (studentObjects == null) return; //生徒オブジェクトが格納されてなかったら処理を返す
		
		// 通常タイムであるか、特殊タイムで黒板や生徒がいない場合、選択中の目標をログに表示
		PlayMischief playMischief = studentObjects[currentTargetRows, currentTargetClos].GetComponent<PlayMischief>();

		if (InputManager.GetKey(PadButton.East))
		{ 
			//選択したものがNPCなら停止処理を送る
			if (studentObjects[currentTargetRows, currentTargetClos].tag == "Student_NPC")
			{
				//間違えたというFlagを立てる処理
				isInputEnabled = false;
				return;
			}
			StudentCheckMischief(playMischief);
			//ここにドリーカート呼ぶ関数をさしてみる
			playMischief.isDollyCartSetPath();
		}

	}

	/// <summary>
	/// 生徒を選択したときQTE中かどうか
	/// </summary>
	private void StudentCheckMischief(PlayMischief playMischief)
	{
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
			//間違えたというFlagを立てる処理
			isInputEnabled = false;
		}
	}

	/// <summary>
	/// 現在のカメラ状況をチェックする処理
	/// </summary>
	private void NowCameraChack()
	{
		isChangeCamera = frontAndBackCamera.SendIsCameraChange();
	}

	/// <summary>
	/// ターゲット座標更新
	/// </summary>
	private void UpdateTargetPosition()
	{
		if (studentObjects[currentTargetRows, currentTargetClos] == null) return;
		
		//今の生徒の座標を更新
		Vector3 studentPos = studentObjects[currentTargetRows, currentTargetClos].transform.position;
		//ターゲット座標Yを更新
		normalTargetYPos = studentPos.y;
		//ターゲット座標更新
		targetIndication.transform.position = new Vector3(studentPos.x, normalTargetYPos + targetHeight, studentPos.z);
	}

}
