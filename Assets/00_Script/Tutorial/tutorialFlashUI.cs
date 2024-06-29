using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialFlashUI : MonoBehaviour
{
	[SerializeField] private Behaviour flashTargetImage; // 点滅させる対象
	[SerializeField] private Behaviour flashTargetText; // 点滅させる対象
	[SerializeField] private Behaviour flashTargetButton; // 点滅させる対象

	[Range(0, 1)] private static readonly float dutyRate = 0.5f; //点滅のデューティ比(1で完全にON、0で完全にOFF)
	private static readonly float maxDutyRate = 1.0f;
	private static readonly float flashCycle = 0.5f; // 点滅周期[s]

	private float elapsedTime; //経過時間を保持しておく変数

	/// <summary>
	/// 初期化
	/// </summary>
	private void Start()
	{
		elapsedTime = 0.0f;
	}

	/// <summary>
	/// 常に処理するメソッドを呼び出す
	/// </summary>
	private void Update()
	{
		ObjectFlash();
	}

	/// <summary>
	/// 点滅処理をする
	/// </summary>
	private void ObjectFlash()
	{
		// 内部時刻を経過させる
		elapsedTime += Time.unscaledDeltaTime;

		// 周期cycleで繰り返す値の取得
		// 0～cycleの範囲の値が得られる
		var repeatValue = Mathf.Repeat(elapsedTime, flashCycle);

		// 内部時刻timeにおける明滅状態を反映
		// デューティ比でON/OFFの割合を変更している
		flashTargetImage.enabled = repeatValue >= flashCycle * (maxDutyRate - dutyRate);
		flashTargetText.enabled = repeatValue >= flashCycle * (maxDutyRate - dutyRate);
		flashTargetButton.enabled = repeatValue >= flashCycle * (maxDutyRate - dutyRate);
	}
}
