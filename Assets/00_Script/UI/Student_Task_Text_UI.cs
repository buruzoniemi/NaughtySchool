using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Task_Text_UI : MonoBehaviour
{
    // 変数宣言------------------------------------------
    private List<string> studentTaskList = new List<string>(); //生徒のタスクを管理するリスト
    private string loadText; //Resourcesフォルダから直接テキストを読み込む					 			
    private string[] splitText; //改行で分割して配列に入れる						 

    [SerializeField, Header("リストを表示するTextのUIを入れてください")] Text textComponent;
    //---------------------------------------------------

    /// <summary>
    /// 最初に処理する
    /// </summary>
    void Start()
    {
        //初期化
        loadText = (Resources.Load("TaskRistText", typeof(TextAsset)) as TextAsset).text;

        //Textファイルからロードしたときちゃんとロードできたかどうか
        if (loadText != null)
        {
            splitText = loadText.Split(char.Parse("\n"));
        }
        //Textファイルをロードできなかった
        else
        {
            Debug.LogError("Textファイルをロードできませんでした。");
            return;//処理を中断
        }


        //リストの初期化
        ResetList();
        //リストのUI表示
        TaskListAddUIText();
    }

    /// <summary>
    /// リストの中にTextファイルのものを代入する
    /// </summary>
    private void ResetList()
    {
        studentTaskList.Clear(); // Listをクリアして初期化
        //配列の中身の数だけ繰り返して処理する
        foreach(string strArray in splitText)
        {
            //最初に取得したタスクの配列をリストで管理する
            studentTaskList.AddRange(splitText);
        }

    }

    /// <summary>
    /// Listの要素を表示し、1要素につき改行する
    /// </summary>
    private void TaskListAddUIText()
    {
        textComponent.text = string.Join("\n", studentTaskList.ToArray());
    }


}
