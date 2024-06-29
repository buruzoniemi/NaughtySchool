using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human_UIControl : MonoBehaviour
{
	[SerializeField] private LessonProgress LessonProgress;
	[SerializeField] private SelectManager SelectManager;
	[SerializeField] private Image CameraChange;       //CameraChangeのUI
	[SerializeField] private Image SpecialTime;        //SpecialTimeのUI

	private bool isCameraChange;     //カメラを切り替えるかどうかをチェックする
	private bool isSpecialTime;      //特殊タイムに入るかどうかをチェックする

	// Start is called before the first frame update
	void Start()
	{
		isCameraChange = LessonProgress.SendCameraChange();
		isSpecialTime = SelectManager.SendCanSpecialAction();
		CameraChange.color = new Color(CameraChange.color.r, CameraChange.color.g, CameraChange.color.b, 0.25f);//CameraChangeのUIを半透明化
		SpecialTime.color = new Color(SpecialTime.color.r, SpecialTime.color.g, SpecialTime.color.b, 0.25f);//SpecialTimeのUIを半透明化

	}

	// Update is called once per frame
	void Update()
	{
		UIAlphaChange();
	}

	private void UIAlphaChange()
	{
		isCameraChange = LessonProgress.SendCameraChange(); //変数の状態がLessonProgressから入力
		isSpecialTime = SelectManager.SendCanSpecialAction();   //変数の状態がSelectManagerから入力
		if (isCameraChange)
		{
			CameraChange.color = new Color(CameraChange.color.r, CameraChange.color.g, CameraChange.color.b, 1.0f);//CameraChangeのUIを不透明化
		}
		if (isSpecialTime)
		{
			SpecialTime.color = new Color(SpecialTime.color.r, SpecialTime.color.g, SpecialTime.color.b, 1.0f);//SpecialTimeのUIを不透明化
		}
		else
		{
			SpecialTime.color = new Color(SpecialTime.color.r, SpecialTime.color.g, SpecialTime.color.b, 0.25f);//SpecialTimeのUIを透明化
		}
	}
}
