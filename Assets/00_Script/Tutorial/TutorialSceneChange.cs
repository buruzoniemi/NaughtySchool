using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// チュートリアルシーン中にボタンを長押ししたら
/// シーンの切り替えをする
/// </summary>
public class TutorialSceneChange : MonoBehaviour
{
	//private static readonly float pressMaxTime = 1.5f; //長押ししててほしい時間 
	public TutorialTeacherUI tutorialTeacherUI;
	public TutorialStudentUI tutorialStudentUI;
	private static readonly float sceneChanegeMaxTimer = 2.0f; //flagが立ってから次のシーンに移行するまでの時間
	private static readonly string nextSceneName = "InGameScene";

	private bool allPlayerReady = false; //すべてのプレイヤーの準備状態
	private float sceneChangeTime;
	//private bool isPressCurrentTime;
	//private float pressCurrentTimer; //長押ししている時間


	// Start is called before the first frame update
	void Start()
	{
		//isPressCurrentTime = false;
		//pressCurrentTimer = 0.0f;
		allPlayerReady = false;
		sceneChangeTime = 0;
	}

	// Update is called once per frame
	void Update()
	{
		//PressButtonProceedCurrentTimer();
		//PressMaxSceneChange();
		//シーンを切り替えるチェック
		SceneChangeManager();
	}

	//public void OnPressSceneChange(InputAction.CallbackContext context)
	//{
	//	// キーを押すかどうかをチェック
	//	if (context.performed)
	//	{
	//		isPressCurrentTime = true;
	//	}
	//	if (context.canceled)
	//	{
	//		isPressCurrentTime = false;
	//	}
	//}

	///// <summary>
	///// 長押しした時間が規定値を超えたら
	///// シーンを変える
	///// </summary>
	//private void PressMaxSceneChange()
	//{
	//	if(pressCurrentTimer > pressMaxTime)
	//	{
	//		SceneManager.LoadScene(nextSceneName);
	//	}
	//}

	//private void PressButtonProceedCurrentTimer()
	//{
	//	// "Jump" は Unity における A ボタンのデフォルトの名前です
	//	if (isPressCurrentTime == true)
	//	{
	//		pressCurrentTimer += Time.deltaTime;
	//	}
	//	else
	//	{
	//		// Aボタンが離された場合、pressCurrentTime をリセットします
	//		pressCurrentTimer = 0.0f;
	//	}
	//}

	//すべてのプレイヤーが準備完了すればインゲームに入る
	private void SceneChangeManager()
	{
		if(tutorialTeacherUI.isTeacherPlayerReady() && tutorialStudentUI.isStudentPlayerReady())
		{
			allPlayerReady = true;
		}
		//すべてのプレイヤーが準備完了してインゲームに入る
		if(allPlayerReady)
		{
			sceneChangeTime += Time.deltaTime;
			if(sceneChangeTime >= sceneChanegeMaxTimer)
			{
				SceneManager.LoadScene(nextSceneName);
			}
		}
	}
}
