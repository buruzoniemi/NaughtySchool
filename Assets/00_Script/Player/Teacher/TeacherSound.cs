using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherSound : MonoBehaviour
{
	[SerializeField] private AudioSource[] teacherSelectSound;//摘発する音

	//摘発する音を生成
	private void SelectSoundCreate()
	{
		int randomSoundNumber = Random.Range(0, 3);  //3種音声からランダムで生成
		for (int i = 0; i < teacherSelectSound.Length; i++)
		{
			if (i == randomSoundNumber)
			{
				teacherSelectSound[i].Play();
			}
		}
	}
}
