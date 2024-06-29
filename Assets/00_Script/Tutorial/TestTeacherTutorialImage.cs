using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTeacherTutorialImage : MonoBehaviour
{
    [SerializeField] private Image teacherTutorialImage;

	void Start()
    {
		SetActiveTeacherTutorialImage(true);
	}

	/// <summary>
	/// 先生用の操作説明図を出すメソッド
	/// </summary>
	public void SetActiveTeacherTutorialImage(bool isActive)
	{
		teacherTutorialImage.gameObject.SetActive(isActive);
	}

}
