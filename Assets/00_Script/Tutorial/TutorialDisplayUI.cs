using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplayUI : MonoBehaviour
{
	[SerializeField] private Behaviour teacherTutorialUITarget; //tutorialで表示される対象
	[SerializeField] private Behaviour studentTutorialUITarget; //tutorialで表示される対象

	private static readonly float MaxDisplayTimer = 5.0f;

	private float currentDisplayTimer; //UIを表示するまでの時間
	private bool isDisplayTutorialUi; //この時間を動かすかどうか

	// Start is called before the first frame update
	void Start()
	{
		isDisplayTutorialUi = false;
		DisplayTutorialUI();
	}

	private void Update()
	{
		AddDeltaTimeDisplayTimer();
	}

	/// <summary>
	/// UIを表示するまでの時間を計測する
	/// </summary>
	private void AddDeltaTimeDisplayTimer()
	{
		if (currentDisplayTimer < MaxDisplayTimer)
		{
			currentDisplayTimer += Time.deltaTime;
		}
		else
		{
			isDisplayTutorialUi = true;
			DisplayTutorialUI();
		}
	}

	/// <summary>
	/// UIの表示、非表示を切り替える処理
	/// </summary>
	private void DisplayTutorialUI()
	{
		teacherTutorialUITarget.gameObject.SetActive(isDisplayTutorialUi);
		studentTutorialUITarget.gameObject.SetActive(isDisplayTutorialUi);
	}
}
