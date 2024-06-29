using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class TitleManager : MonoBehaviour
{
	public Camera mainCamera;
	public Camera subCamera;
	// ゲーム中に表示される文字列
	public TMP_Text[] AnyKeyDownText;
	//グラデーションしてシーンを切り替える
	public Image[] black;
	private bool isAnyKeyDown;
	private float imageChangeTime = 1.0f;  //不透明になる時間
	private float imageChangeTimeNow = 0f;
	private int keyDownTImes = 0;

	// 文字の上下動の距離と速度
	public float UpDownDistance = 5f; // 跳ねる距離
	public float UpDownSpeed = 5f; // 跳ねる速度

	//BGM
	public AudioSource backgroundMusic;

	// 最初のフレーム更新前に呼び出される
	void Start()
	{
		//モニターチェック
		if (Display.displays.Length > 1)
		{
			Display.displays[1].Activate();
			//先生視角はモニター１、生徒は２
			mainCamera.targetDisplay = 0;
			subCamera.targetDisplay = 1;
		}
		else
		{
			mainCamera.targetDisplay = 0;
		}
		// ゲームのフレームレートを60FPSに設定
		Application.targetFrameRate = 60;
		for (int i = 0; i < AnyKeyDownText.Length; i++)
		{
			AnyKeyDownText[i].text = "Please Take Any Key"; // 表示テキストの設定
															// TextMeshProコンポーネントが未指定の場合、自動的に取得
			if (AnyKeyDownText[i] == null) AnyKeyDownText[i] = GetComponent<TMP_Text>();
		}
		// 初期化処理
		isAnyKeyDown = false;
		for (int i = 0; i < black.Length; i++)
		{
			black[i].color = new Color(black[i].color.r, black[i].color.g, black[i].color.b, 0);
		}
		//BGMを設定
		if (backgroundMusic != null)
		{
			backgroundMusic.loop = true; //繰り返すプレイ
			backgroundMusic.Play(); //音声プレイ
		}
	}

	// フレームごとに呼び出される
	void Update()
	{
		ButtonDown(); // キー入力の確認
		textManager(); // 文字のアニメーション処理
		sceneChange(); //シーン切り替える処理
	}

	// キー入力によるシーンの切り替えを処理
	private void ButtonDown()
	{
		// 任意のキーが押されたら「Stage Select」シーンに切り替える
		if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
		{
			keyDownTImes++;
		}
		if (keyDownTImes == 1)
		{
			isAnyKeyDown = true;
		}
	}

	// 文字の上下動アニメーションを管理
	private void textManager()
	{
		for (int j = 0; j < AnyKeyDownText.Length; j++)
		{
			string text = AnyKeyDownText[j].text;
			TMP_TextInfo textInfo = AnyKeyDownText[j].textInfo;
			for (int i = 0; i < textInfo.characterCount; i++)
			{
				TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
				if (!charInfo.isVisible)
					continue; // 見えない文字はスキップ

				int materialIndex = charInfo.materialReferenceIndex;
				int vertexIndex = charInfo.vertexIndex;

				Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

				// Mathf.Sin関数を利用して波形動作を生成し、文字を上下に動かす
				float wave = Mathf.Sin(Time.time * UpDownSpeed + i * 0.1f) * UpDownDistance;

				for (int z = 0; z < 4; z++)
				{
					Vector3 original = vertices[vertexIndex + z];
					vertices[vertexIndex + z] = original + new Vector3(0, wave, 0); // 文字の頂点位置を更新
				}
			}

			// メッシュを更新して変更を適用
			AnyKeyDownText[j].UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
		}

	}

	private void sceneChange()
	{
		if (isAnyKeyDown)
		{
			//時間計測
			imageChangeTimeNow += Time.deltaTime;
			for (int i = 0; i < black.Length; i++)
			{
				//不透明になる処理
				if (imageChangeTimeNow < imageChangeTime)
				{
					float alphaChnage = Time.deltaTime / imageChangeTime;

					if (black[i] != null)
						black[i].color = new Color(black[i].color.r, black[i].color.g, black[i].color.b, black[i].color.a + alphaChnage * 1.1f);
				}
			}
			//音声の大きさを減らす
			backgroundMusic.volume -= 0.7f * Time.deltaTime;
			//シーンを切り替える
			if (imageChangeTimeNow > imageChangeTime + 1.0f)
			{
				backgroundMusic.Stop();
				SceneManager.LoadScene("Stage Select");
			}
		}
	}
}
