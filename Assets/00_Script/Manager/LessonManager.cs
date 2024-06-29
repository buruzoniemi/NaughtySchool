/*
 * 勝敗処理、時間管理などを行うわよ～～～～～～
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LessonManager : Singleton_Class<LessonManager>
{
	[SerializeField] private GameObject StudentWIN;
	[SerializeField] private GameObject TeacherWIN;
	[SerializeField] private Stopwatch_UI stopwatchUI; //ストップウォッチを管理する
	[SerializeField] private AudioSource chimeSound; //チャイム
	[SerializeField] private AudioSource backgroundMusic; //BGM
	[SerializeField] private Text[] gameOverText; //ゲーム終了になるUI
	[SerializeField] private PlayerSpawnInGameStop PlayerSpawnInGameStop;
	private float gameOverTime;
	public static bool teacherWin;
	public static bool studentWin;

	private readonly static string nextResultScene = "ResultScene"; //リザルトシーンに移行する用のstring

	private bool isBGMPlay; //BGMプレイかどうか
	private bool isChimePlay;//チャイムプレイかどうか
	private bool isTimeUp; //タイムになるかどうか
	private bool isPaused; //ゲームスタートの停止状態
	private bool isGameStart;//ゲームスタートをチェックする


	// Start is called before the first frame update
	void Start()
	{
		StudentWIN.SetActive(false);
		TeacherWIN.SetActive(false);
		gameOverTime = 0;
		teacherWin = false;
		studentWin = false;
		isBGMPlay = false;
		isChimePlay = false;
		for (int i = 0; i < gameOverText.Length; i++)
		{
			gameOverText[i].text = "";
		}
		isGameStart = false;
		isPaused = PlayerSpawnInGameStop.SendPausedEnd();
	}

	// Update is called once per frame
	void Update()
	{
		resultAdress();
		SoundControl();
	}

	public void WinnerDecided(int playerSide)
	{
		if (playerSide == 0)
		{
			//StudentWIN.SetActive(true);
			studentWin = true;
		}
		else
		{
			//TeacherWIN.SetActive(true);
			teacherWin = true;
		}
	}

	private void resultAdress()
	{
		//先生の勝ちでシーンを切り替える処理
		if (teacherWin)
		{
			gameOverTime += Time.deltaTime;
			if (gameOverTime >= 2 && isChimePlay && !chimeSound.isPlaying)
			{
				SceneManager.LoadScene(nextResultScene);
			}
		}
		//生徒の勝ちでシーンを切り替える処理
		if (studentWin)
		{
			gameOverTime += Time.deltaTime;
			if (gameOverTime >= 2 && isChimePlay && !chimeSound.isPlaying)
			{
				SceneManager.LoadScene(nextResultScene);
			}
		}
		//タイムになってシーンを切り替える処理
		if (isTimeUp)
		{
			gameOverTime += Time.deltaTime;
			if (gameOverTime >= 2 && isChimePlay && !chimeSound.isPlaying)
			{
				SceneManager.LoadScene(nextResultScene);
			}
		}
	}
	//音声管理
	private void SoundControl()
	{
		isPaused = PlayerSpawnInGameStop.SendPausedEnd(); //状態更新
		//ゲーム開始時の音声
		if (chimeSound != null && !isGameStart && !isPaused)
		{
			chimeSound.Play();
			isGameStart = true; //一回放送制限
		}
		isTimeUp = stopwatchUI.SendTimeStop(); //stopwatchからタイムになるかチェック
		if (!chimeSound.isPlaying && !isBGMPlay && isGameStart)
		{
			if (backgroundMusic != null)
			{
				backgroundMusic.loop = true; //繰り返すプレイ
				backgroundMusic.Play(); //音声プレイ
				isBGMPlay = true;
			}
		}
		if (studentWin || teacherWin || isTimeUp)
		{
			//inputsystem使用停止
			InputSystem.DisableAllEnabledActions();
			//ゲーム終了文字表示
			for (int i = 0; i < gameOverText.Length; i++)
			{
				if (studentWin || teacherWin)
				{
					gameOverText[i].text = "ゲーム終了";
				}
				if (isTimeUp)
				{
					gameOverText[i].text = "時間切れ";
				}
			}
			//BGM音声の大きさが減らす
			backgroundMusic.volume -= 0.7f * Time.deltaTime;
			if (!isChimePlay && backgroundMusic.volume <= 0.0f)
			{
				backgroundMusic.Stop();    //BGM停止
				chimeSound.Play();         //チャイムが出る
				isChimePlay = true;
			}
		}
	}
}
