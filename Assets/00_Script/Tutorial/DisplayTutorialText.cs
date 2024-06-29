using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTutorialText : MonoBehaviour
{
	[SerializeField] private Text explanationInGameTimeText; //インゲーム時間について説明するText
	[SerializeField] private Text explanationProgressBarText; //授業進捗ゲージについて説明するText
	[SerializeField] private Text explanationLeftRotateText; //左回転について説明するText
	[SerializeField] private Text explanationFrontRotateText; //前回転について説明するText
	[SerializeField] private Text explanationLessonTeacherUIText; //授業進捗をするボタンについて説明するText
	[SerializeField] private Text explanationExposureTeacherUIText; //生徒を指摘するボタンについて説明するText 
	[SerializeField] private Text explanationEmperorTimeTeacherUIText; //特殊行動をするボタンについて説明するText 
	[SerializeField] private Text explanationCameraChangeTeacherUIText; //前後のカメラを変更するボタンについて説明するText


	private bool isDisplayTutorialText;
	

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
