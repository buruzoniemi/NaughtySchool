using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherUI : MonoBehaviour
{
	[SerializeField] private Text stopTimeText; //選択ミスした後のStopTime中に出すText
	[SerializeField] private Image darkPanel; //選択ミスしたときの暗い演出用のキャンバス
	[SerializeField] private Text specialTimeText; //SpecialTime中に出すText
	[SerializeField] private Text selectSuccess; //摘発成功text
	[SerializeField] private TeacherAnim_Controller teacherAnimController; //アニメーション管理用のスクリプト

	private float selectSuccessTime; //文字を表示する時間
	private bool isSelectSuccess;    //文字表示用

	// Start is called before the first frame update
	void Start()
    {
		specialTimeText.text = "";

		//Textの表示を消す
		MissTimeFadeUI();
	}

	/// <summary>
	/// 選択ミスした時のUIを表示する
	/// </summary>
   protected void MissTimeActiveUI()
	{
		stopTimeText.gameObject.SetActive(true);
		darkPanel.gameObject.SetActive(true);
	}

	/// <summary>
	/// 選択ミスした時のUIを非表示にする
	/// </summary>
	protected void MissTimeFadeUI()
	{
		stopTimeText.gameObject.SetActive(false);
		darkPanel.gameObject.SetActive(false);
	}

	/// <summary>
	/// 摘発成功の処理
	/// </summary>
	protected void SelectCheckUI()
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
			//アニメーションを呼び出す
			teacherAnimController.AnimHit(isSelectSuccess);
		}
	}

	/// <summary>
	/// そのUIを一瞬光らせる
	/// </summary>
	public void FlashUI(Image image)
	{
		image.color = new Color(1.0f, 1.0f, 1.0f, 3.0f);
	}
}
