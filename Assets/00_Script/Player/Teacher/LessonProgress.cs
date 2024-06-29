using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LessonProgress : MonoBehaviour
{
	private bool isKeyPressed;  //キーを押す状態を切り替える
	private bool isBlackboardWriting;//板書するかどうかをチェック
	private bool isCameraChange; //カメラの前後変更を
	private float keyPressTime;    //キーを押す状態を記録する
	private float pressDuration;  //完成までの時間

	//public static bool isSpecialTime = false; //特殊タイムに入るかどうかを確認
	public static bool isTeachingOver = false; //授業進度が終わるかどうかを確認

	[SerializeField] private Image progressBar;        //UIのプレハブ（進度イメージ）
	[SerializeField] private Text progressPercentText;  //UIのプレハブ（進度文字）
	[SerializeField] private FrontCameraMove frontCameraMove; //FrontCameraMoveを格納するもの
	[SerializeField] private AudioSource blackboardWriting; //板書の音
	[SerializeField] private TeacherAnim_Controller teacherAnimController;

	//田代くん追加変数--------------------
	private bool isBlackBordDirection; //黒板の方に向いてるかどうか
	private bool isLecternDirection; //教科書の方に向いてるかどうか
	private float barMagnification; //Barの増加倍率
	private static readonly float barThreeMagnification = 3.0f; //Barの増加率3倍
	private static readonly float barNormalMagnification = 1.0f; //Barの増加率1倍
	private static readonly float barCheckPointQuarter = 0.25f; //Barの進捗度の4分の1チェックポイント
	private static readonly float barCheckPointHalf = 0.5f; //Barの進捗度の2分の1チェックポイント
	private static readonly float barCheckPointThreeQuarters = 0.75f; //arの進捗度の4分の3チェックポイント
	private static readonly float barCheckPointFull = 1.0f; //Barの進捗度の1分の1チェックポイント
	private static readonly float percentageConvert = 100.0f; //百分率に直す

	private bool[] canSpecialTimesUp; //特殊行動回数を増やす

	void Start()
	{
		progressBar.fillAmount = 0.0f;
		progressPercentText.text = "0.0%";     //最初の進度は0
		barMagnification = 1;
		keyPressTime = 0f;
		pressDuration = 120f;

		//isSpecialTime = false;
		isTeachingOver = false;
		isBlackBordDirection = false;
		isLecternDirection = false;
		isBlackboardWriting = false;
		isCameraChange = false;
		isKeyPressed = false;

		canSpecialTimesUp = new bool[3] { false, false, false };

	}

	void Update()
	{
		//キーを押す状態中の処理
		ProgressUpdate();
		if (!isBlackboardWriting)
		{
			blackboardWriting.Stop();
		}
	}

	/// <summary>
	/// InputSystemの処理
	/// </summary>
	/// <param name="context"></param>
	public void OnKeyCheck(InputAction.CallbackContext context)
	{
		if (context.started) return;
		isBlackBordDirection = frontCameraMove.SendLeftPressed();
		isLecternDirection = frontCameraMove.SendFrontPressed();
		//Debug.Log($"isBlackBordDirection ： {isBlackBordDirection}");
		//Debug.Log($"islecternDirection : {isLecternDirection}");
		if (isBlackBordDirection == false && isLecternDirection == false) return;

		// キーを押すかどうかをチェック
		if (context.performed)
		{
			isKeyPressed = true;
			if (!isBlackboardWriting)
			{
				blackboardWriting.loop = true;
				blackboardWriting.Play();
				isBlackboardWriting = true;
				teacherAnimController.AnimWrite();
			}
		}
		if (context.canceled)
		{
			isKeyPressed = false;
			isBlackBordDirection = false;
			isLecternDirection = false;
			isBlackboardWriting = false;
			teacherAnimController.AnimStopWrite();
		}
	}

	/// <summary>
	/// どっちの向きで呼び出すかを決めている
	/// </summary>
	void ProgressUpdate()
	{
		//押されてなかったら処理しない
		if (!isKeyPressed)
		{
			isBlackboardWriting = false;
			return;
		}

		if (isBlackBordDirection)
		{
			barMagnification = barThreeMagnification;
			BarUpdate(isBlackBordDirection, barMagnification);
		}
		if (isLecternDirection)
		{
			barMagnification = barNormalMagnification;
			BarUpdate(isLecternDirection, barMagnification);
		}
	}

	/// <summary>
	/// 先生の進捗ゲージBarの処理
	/// </summary>
	/// <param name="isDirection">今向いてる方向が授業する向きかどうか</param>
	/// <param name="barMagnification">Barの増加倍率</param>
	void BarUpdate(bool isDirection, float barMagnification)
	{
		if (!isDirection) return;

		keyPressTime += Time.deltaTime * barMagnification;    //押す時間を加算
		float progress = Mathf.Clamp(keyPressTime / pressDuration, 0f, 1f);   //プログレスバー増加の変数,値は0～1の間
		if (progress >= barCheckPointQuarter)
		{
			isCameraChange = true;
			canSpecialTimesUp[0] = true;
		}
		if (progress >= barCheckPointHalf)
		{
			//isSpecialTime = true;
			canSpecialTimesUp[1] = true;
		}
		if (progress >= barCheckPointThreeQuarters)
		{
			canSpecialTimesUp[2] = true;
		}
		if (progress >= barCheckPointFull)
		{
			isTeachingOver = true;
			LessonManager.instance.WinnerDecided(1);
		}
		if (isTeachingOver) Debug.Log("Over");
		if (progressBar != null)
		{
			progressBar.fillAmount = progress;   //プログレスバー増加
		}
		progressPercentText.text = (progress * percentageConvert).ToString("F1") + "%";   //進度を表示する

		//Debug.Log("Barが増加してるなう");
		//Debug.Log($"{progressPercentText.text}"); 
	}

	public float SendProgressBar()
	{
		return progressBar != null ? progressBar.fillAmount : 0f;
	}

	public bool SendCameraChange()
	{
		return isCameraChange;
	}

	//public bool SendSpecialTime()
	//{
	//    return isSpecialTime;
	//}

	public void StopKeyPressed()
	{
		isKeyPressed = false;
	}

	public bool SendCanSpecialTimes1()
	{
		return canSpecialTimesUp[0];
	}

	public bool SendCanSpecialTimes2()
	{
		return canSpecialTimesUp[1];
	}

	public bool SendCanSpecialTimes3()
	{
		return canSpecialTimesUp[2];
	}
}
