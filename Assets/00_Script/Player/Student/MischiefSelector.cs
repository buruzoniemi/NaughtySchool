/*
 * Script to select mischief
 * Created by Misora Tanaka
 * 
 * date: 24/02/16
 * --- Log ---
 * 02/16 Write a comment
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MischiefSelector : MonoBehaviour
{
    [Header("--- 初期設定 ---")]
    [Header("QTEスクリプト")]
    [SerializeField] private PlayMischief playMischief;
    [Header("PiMenuスクリプト")]
    [SerializeField] private PiMenuController piMenu;

    // Angle of the previous stick
    private float previousDagree;
    private int selectAllowID;
    // DeviceName
    private string myDevices;
    // Misbehave
    private bool nowActionMischief;
    // Operate it
    private bool isPlay;

    // Start is called before the first frame update
    void Start()
    {
        // initialization
        nowActionMischief = false;
        myDevices = null;
        isPlay = true;
    }

	/// <summary>
	/// Open pieMenu
	/// Function called when input is received from InputSystem
	/// </summary>
	/// <param name="context">inputValue</param>
	public void DragMenuOpen(InputAction.CallbackContext context)
	{
		if (!isPlay) return;
		if (nowActionMischief) return;
		if (context.phase == InputActionPhase.Started) piMenu.EnableCanvas();
	}


	/// <summary>
	/// Choose mischief in the pie menu
	/// Function called when input is received from InputSystem
	/// </summary>
	/// <param name="context">inputValue</param>
	public void DragSelect(InputAction.CallbackContext context)
    {
        // Terminates processing when other operations are in progress
        if (!isPlay) return;
        if (nowActionMischief) return;
        // ボタンを押している間
        if (!context.canceled)
        {
            if (myDevices == null) sendDevices(context); // 初回入力時QTE判断用に値を取る
            // 値を取得
            Vector2 dragValue = context.ReadValue<Vector2>();
            float dagree = Mathf.Atan2(dragValue.x, dragValue.y) * Mathf.Rad2Deg;

            // 0～360度の中に収める
            if (dagree < 0) { dagree += 360; }
            // 前フレームの角度を更新
            previousDagree = dagree;
            // パイメニューを更新
            selectAllowID = piMenu.GetSelectedSlot(previousDagree);
        }
        else
        {
            // 非表示にする
            piMenu.DisableCanvas();
            // 離したら処理する
            DecideMischief();
        }
    }

	/// <summary>
	/// いたずら決定後の関数
	/// </summary>
	private void DecideMischief()
    {
        // Find out if anyone is playing
        if (MischiefManager.instance.CheckTaskIsPlay(selectAllowID))
        {
            Debug.Log("このタスクは誰かがプレイ中です");
            return;
        }
        // Find out if you've already achieved
        if (MischiefManager.instance.CheckTaskComplete(selectAllowID)) 
        {
            Debug.Log("このタスクは達成済です");
            return;
        }

        // 無効化
        nowActionMischief = true;
        // コマンドの有効化
        playMischief.OnEnableQTE(MischiefManager.instance.BeginMischiefTask(selectAllowID));

        // デリゲートの設定　　
        playMischief.initDisableQTE(OnDisableQTE);
        playMischief.initExposedQTE(ExposedStudent);
        Debug.Log("いたずらのコマンドを有効化します");
    }


    /// <summary>
    /// QTEが終了した時に呼ぶ関数
    /// </summary>
    private void OnDisableQTE(Mischief mischief)
    {
        nowActionMischief = false;
        // MischiefManagerに送る
        MischiefManager.instance.EndMischiefTask(mischief);
    }

    private void ExposedStudent(Mischief mischief)
    {
        OnDisableQTE(mischief);
        isPlay = false;
    }

    /// <summary>
    /// 初回入力時にQuickTimeEventにデバイス名を送る
    /// </summary>
    /// <param name="context">入力されたコントローラー</param>
    private void sendDevices(InputAction.CallbackContext context)
    {
        playMischief.SetDeviceName(context.control.device.name);
    }
}