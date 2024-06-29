using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class ControllerCheck : MonoBehaviour
{
    //変数宣言--------------------------------------------
    [InputControl, SerializeField] private string _controlPath; // 対象のControl（Control Path）
    private string sendDevicePath;
    //----------------------------------------------------


    // 初期化
    private void Awake()
    {
        DevicePathCheck();
    }

    /// <summary>
    /// Playerに接続されているデバイスのパスを取得してログに出す。
    /// </summary>
    private void DevicePathCheck()
    {
        // Control PathからControlを取得
        var control = InputSystem.FindControl(_controlPath);
        if (control == null)
        {
            Debug.LogError($"指定されたControl Path「{_controlPath}」のControlが見つかりませんでした。");
            return;
        }
        //デバイスの名前を表示
        Debug.Log($"Device: {control.device}");

        sendDevicePath = control.device.ToString();
    }


    /// <summary>
    /// デバイスのパスを送る
    /// </summary>
    /// <returns></returns>
    public string SendDevicePath()
    {
        return sendDevicePath;
    }
}
