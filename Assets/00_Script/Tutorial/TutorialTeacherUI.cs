using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialTeacherUI : MonoBehaviour
{
	[SerializeField] private Behaviour TargetImage; // イメージ図
	[SerializeField] private Text TargetText; // テキスト
	[SerializeField] private Behaviour TargetButton; // Aボタン
	[SerializeField] private Image FillImage; //円形ゲージ対象

	private bool isPressing = false; //ボタンを長押しするかどうかを確認
	private bool isTeacherReady = false; //準備完了するかどうかを確認
	private float fillSpeed = 0.5f; //円形ゲージを増加するスピード

	/// <summary>
	/// 初期化
	/// </summary>
	private void Start()
	{
		//elapsedTime = 0.0f;
	}

	/// <summary>
	/// 常に処理するメソッドを呼び出す
	/// </summary>
	private void Update()
	{
		ObjectFillAmount();
	}

	public void OnPressing(InputAction.CallbackContext context)
	{
		// キーを押すかどうかをチェック
		if (context.performed)
		{
			isPressing = true;
		}
		if (context.canceled)
		{
			isPressing = false;
		}
	}

	/// <summary>
	/// 円形ゲージを表示する
	/// </summary>
	private void ObjectFillAmount()
	{
		//円形ゲージの表示
		if (isPressing && FillImage.fillAmount < 1.0f && !isTeacherReady)
		{
			//ボタンを長押ししたら増加
			FillImage.fillAmount += fillSpeed * Time.deltaTime;
		}
		else if (!isPressing && FillImage.fillAmount > 0.0f && !isTeacherReady)
		{
			//ボタンを離したら減少
			FillImage.fillAmount -= fillSpeed * Time.deltaTime;
		}
		//円形ゲージを完全に表示すれば準備完了
		if (FillImage.fillAmount >= 1.0f)
		{
			isTeacherReady = true;
			TargetText.text = "準備完了";
		}
		//準備完了の場合に円形ゲージを完全に表示
		if (isTeacherReady)
		{
			FillImage.fillAmount = 1.0f;
		}
	}

	//準備完了の値を渡す
	public bool isTeacherPlayerReady()
	{
		return isTeacherReady;
	}
}
