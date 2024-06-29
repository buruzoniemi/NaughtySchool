using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
	//カメラ（二つウィンドウ対応）
	public Camera teacherCamera;
	public Camera studentCamera;

	public Text teacherReady;

	public Text studentPlayer;               //プレイヤー文字表示
	public Text studentReady;                //プレイヤー文字表示
	public GameObject studentPlayerPanel;    //背景表示
	public GameObject studentReadyPanel;     //背景表示

	public Image black;           //グラデーション用

	public AudioSource backgroundMusic;   //BGM

	public static int studentPlayerQuantites = 0;     //人数選択

	private float sceneOverTime;          //シーンを切り替える計算
	private float sceneChangeTime = 2.0f; //シーンを切り替えるまでの時間
	private bool isSceneChange;           //シーンを切り替える事ができるか
	private bool isStudentReady = false;  //生徒の準備状態
	private bool selectNumber = true;     //人数選択をする時
	private bool isTeacherReady = false;  //先生の準備状態

	private string teacherNotReady;       //先生側準備してない文字
	private string teacherInReady;        //先生側準備している
	private string studentNotReady;       //生徒側準備してない文字
	private string studentInReady;        //生徒側準備している 

	private static readonly string nextSceneName = "TutorialScene";		  //次のシーン先

	void Start()
	{
		//モニターチェック
		if (Display.displays.Length > 1)
		{
			Display.displays[1].Activate();
			//先生視角はモニター１、生徒は２
			teacherCamera.targetDisplay = 0;
			studentCamera.targetDisplay = 1;
		}
		else
		{
			teacherCamera.targetDisplay = 0;
		}

		teacherNotReady = "授業準備中";
		teacherInReady = "授業準備完了";
		studentNotReady = "休み時間中";
		studentInReady = "着席";


		teacherReady.text = teacherNotReady;
		studentPlayer.text = "0";
		studentReady.text = studentNotReady;
		isTeacherReady = false;
		isStudentReady = false;
		selectNumber = true;
		studentPlayerQuantites = 0;
		sceneOverTime = 0.0f;
		isSceneChange = false;

		//BGMを設定
		if (backgroundMusic != null)
		{
			backgroundMusic.loop = true; //繰り返すプレイ
			backgroundMusic.Play(); //音声プレイ
		}
	}

	private void Update()
	{
		UpdateUIHighlight();
		IngameChange();
	}

	//先生の準備をチェック
	public void OnTeacherReadyCheck(InputAction.CallbackContext context)
	{
		//押された瞬間だけ取得する
		if (!context.performed) return;
		//準備状態切り替える
		isTeacherReady = !isTeacherReady;
		teacherReady.text = isTeacherReady ? teacherInReady : teacherNotReady;
	}
	//生徒の人数選択を減らす
	public void OnStudentSelectLeft(InputAction.CallbackContext context)
	{
		//押された瞬間だけ取得する
		if (!context.performed) return;

		if (selectNumber)
		{
			studentPlayerQuantites--;
			if (studentPlayerQuantites < 0)
			{
				studentPlayerQuantites = 0;
			}
			studentPlayer.text = studentPlayerQuantites.ToString();
		}
		else
		{

		}

	}
	//生徒の人数選択を増やす
	public void OnStudentSelectRight(InputAction.CallbackContext context)
	{
		//押された瞬間だけ取得する
		if (!context.performed) return;
		if (selectNumber)
		{
			studentPlayerQuantites++;
			if (studentPlayerQuantites > 3)
			{
				studentPlayerQuantites = 3;
			}
			studentPlayer.text = studentPlayerQuantites.ToString();
		}
		else
		{

		}

	}
	//生徒の準備チェック
	public void OnStudentReadyCheck(InputAction.CallbackContext context)
	{
		//押された瞬間だけ取得する
		if (!context.performed) return;
		if (!selectNumber)
		{
			//準備状態切り替える
			if (studentPlayerQuantites > 0)
			{
				isStudentReady = !isStudentReady;
				studentReady.text = isStudentReady ? studentInReady : studentNotReady;
			}
			else
			{
				isStudentReady = false;
				studentReady.text = studentNotReady;
			}
		}
	}
	//生徒プレイヤーの人数選択と準備状態を変更
	public void OnStudentSelectCheck(InputAction.CallbackContext context)
	{
		//押された瞬間だけ取得する
		if (!context.performed) return;
		selectNumber = !selectNumber;
	}

	//文字表示の背景
	private void UpdateUIHighlight()
	{
		studentPlayerPanel.SetActive(selectNumber);
		studentReadyPanel.SetActive(!selectNumber);
	}
	//シーンを切り替える処理
	private void IngameChange()
	{
		if (isStudentReady && isTeacherReady)
		{
			isSceneChange = true;
			InputSystem.DisableAllEnabledActions();
		}
		if (isSceneChange)
		{
			sceneOverTime += Time.deltaTime;
			//不透明になる処理
			if (sceneOverTime < sceneChangeTime)
			{
				float alphaChnage = Time.deltaTime / sceneChangeTime;
				if (black != null)
					black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + alphaChnage * 1.1f);
			}
			//音声の大きさを減らす
			backgroundMusic.volume -= 0.7f * Time.deltaTime;
			if (sceneOverTime >= 2.0f)
			{
				backgroundMusic.Stop();
				SceneManager.LoadScene(nextSceneName);
			}

		}
	}
}
