using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSrudentTutorialImage : MonoBehaviour
{
	[SerializeField] private Image studentTutorialImage;

	void Start()
	{
		SetActiveStudentTutorialImage(true);
	}


	/// <summary>
	/// 生徒用の操作説明図を出すメソッド
	/// </summary>
	public void SetActiveStudentTutorialImage(bool isActive)
	{
		studentTutorialImage.gameObject.SetActive(isActive);
	}
}
