using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExit : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		InputGameExit();
	}

	//ゲームウィンドウを閉じる
	private void InputGameExit()
	{
		//escキーを押されてゲームを終了
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            #if UNITY_EDITOR
            			UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
		}
	}
}
