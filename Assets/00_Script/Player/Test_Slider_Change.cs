using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Slider_Change : MonoBehaviour
{
    // 変数宣言----------------------------
    [SerializeField] private float sendSliderValue; // スライダーに値を送る変変数
    private Teacher_Bar_UI uiTeacherBar; //UI_Teacher_Barを格納する変数
    //------------------------------------

    /// <summary>
    /// 最初にいろいろ処理するメソッド
    /// </summary>
    private void Start()
    {
        uiTeacherBar = GameObject.FindWithTag("TeacherUI").GetComponent<Teacher_Bar_UI>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            uiTeacherBar.TeacherGageUp(sendSliderValue);
            Debug_Manager.instance.DebugLog("押されている");
        }
    }
}
