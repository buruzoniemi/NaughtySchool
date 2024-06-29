using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student_Task_Check : MonoBehaviour
{
    // 変数宣言------------------------------
    [SerializeField] private TextAsset textAsset; //読む込むテキストが書き込まれている.txtファイル		
					 
    private string loadText; //Resourcesフォルダから直接テキストを読み込む					 			
    private string[] splitText; //改行で分割して配列に入れる						 
    private int textNum; //現在表示中テキスト番号
    private List<string> studentTaskList = new List<string>(); // task管理をListに変更 インスペクター上でtaskを増やすことが可能
    private Dictionary<string, bool> studentTask = new Dictionary<string, bool>(); // Listの中身をbool管理できるようする変数iD
    //--------------------------------------

    /// <summary>
    /// 最初に処理するメソッド
    /// </summary>
    void Start()
    {
        //初期化
        loadText = (Resources.Load("TaskRistText", typeof(TextAsset)) as TextAsset).text;
        splitText = loadText.Split(char.Parse("\n"));
        textNum = 0;

        //辞書を初期化する
        ResetTasks();
        //リストを初期化する
        ResetList();
    }

    /// <summary>
    /// 毎フレーム処理するメソッド
    /// </summary>
    void Update()
    {
        //Debug_Manager.instance.DebugLog($"{studentTask.Keys}");
    }

    /// <summary>
    /// リストの中にTextファイルのものを代入する
    /// </summary>
    private void ResetList()
    {
        studentTaskList.Clear(); // Listをクリアして初期化
            if (splitText[textNum] != "")
            {
                studentTaskList.AddRange(splitText);
                textNum++;
            }
    }

    /// <summary>
    /// 辞書を初期化する処理
    /// </summary>
    private void ResetTasks()
    {
        studentTask.Clear(); // 辞書をクリアして初期化

        // studentTaskList内の各タスクを辞書に追加し、初期状態では未完了(false)に設定する
        foreach (string task in studentTaskList)
        {
            studentTask.Add(task, false);
        }
    }

    /// <summary>
    ///  特定のタスクを完了済みにする関数
    /// </summary>
    /// <param name="taskName">Listのタスクの中身</param>
    public void MarkTaskCompleted(string taskName)
    {
        if (studentTask.ContainsKey(taskName))
        {
            studentTask[taskName] = true; // 指定されたタスクを完了済み(true)に設定する
        }
        else
        {
            Debug_Manager.instance.DebugLog("このタスクが見つかりません: " + taskName);
        }
    }

    /// <summary>
    /// 特定のタスクが完了済みかどうかを取得する関数
    /// </summary>
    /// <param name="taskName">Listのタスクの中身</param>
    /// <returns></returns>
    public bool IsTaskCheck(string taskName)
    {
        if (studentTask.ContainsKey(taskName))
        {
            return studentTask[taskName]; // 指定されたタスクの完了状態を返す
        }
        else
        {
            Debug_Manager.instance.DebugLog("このタスクが見つかりません: " + taskName);
            return false; // タスクが見つからない場合、未完了(false)を返す
        }
    }
}
