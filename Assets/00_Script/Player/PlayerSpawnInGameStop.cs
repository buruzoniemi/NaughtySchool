using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnInGameStop : MonoBehaviour
{
	private int targetPlayerCount = 0;
	private int currentPlayerCount = 0;

	// InGameの処理が一時停止されているかどうかを示すフラグ
	private bool isPaused = true;

	void Start()
	{
		Debug.Log("targetPlayerCount：" + $"{targetPlayerCount}");
		targetPlayerCount = StageSelectManager.studentPlayerQuantites;
		// InGameの処理を一時停止する
		PauseGame();
	}

	// プレイヤーが参加したときに呼び出されるメソッド
	public void PlayerJoined()
	{
		currentPlayerCount++;

		// 参加人数が目標人数に達したら、InGameの処理を再開する
		if (currentPlayerCount == targetPlayerCount)
		{
			ResumeGame();
		}
	}

	// InGameの処理を一時停止するメソッド
	private void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0f; // 時間の経過を停止する
							 //Time.timeScale 0がInGameを止める 1はInGameを進む
	}

	// InGameの処理を再開するメソッド
	private void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1f; // 時間の経過を再開する
	}

	public int SendCurrentPlayerCount()
	{
		return currentPlayerCount;
	}

	public int SendTargetPlayerCount()
	{
		return targetPlayerCount;
	}

	public bool SendPausedEnd()
	{
		return isPaused;
	}
}
