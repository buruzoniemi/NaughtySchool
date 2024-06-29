using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// いたずらの構造体
[System.Serializable]
public struct Mischief
{
    [SerializeField, Header("名前")] 
    public string name;                             // いたずらの名称
    [SerializeField, Header("コマンド数")] 
    public int commandNum;                          // コマンド数
    [SerializeField, Header("コマンド詳細")]
    public List<string> commandList;                // コマンドリスト
    [SerializeField, Header("テーブル内での番号")] 
    public int ID;                                  // いたずらのIndex 
    [SerializeField, Header("実行中")] 
    public bool isPlay;                             // いたずら中か
    [SerializeField, Header("完了済")] 
    public bool isComp;                             // 完了したか

    public void None()
    {
        this.name = null;
        this.commandNum = -1;
        this.commandList = null;
        this.ID = -1;
        this.isPlay = false;
        this.isComp = false;
    }
}
public class MischiefManager : Singleton_Class<MischiefManager>
{
    // 変数宣言------------------------------------------------------------------------------
    [Header("読み込みしたいたずらリスト")]
    [SerializeField] private List<Mischief> mischiefTable  // いたずらの一覧リスト
        = new List<Mischief>();
    [Header("ゲーム中に使ういたずらリスト")]
    [SerializeField] private List<Mischief> taskMischiefs   // 処理するいたずらリスト
        = new List<Mischief>();
    [Header("実行中のいたずらリスト")]
    public List<Mischief> inProgressMischiefs               // 実行中のタスクのリスト
        = new List<Mischief>(); 
    [Header("ゲーム中に使うタスク数")]
    [SerializeField] private int taskMischiefsID;           // タスクの数 
															//----------------------------------------------------------------------------------------

	private string[] naughty = new string[9] { "眠り", "おしゃべり", "ダンス", "早弁", "ドミノ", "練り消し", "折り紙", "ゲーム", "菓子" };
	private int[] mischiefAnimNum;

	// シングルトンにする
	protected new void Awake()
    {
        base.Awake();
        instance = this;

		mischiefAnimNum = new int[taskMischiefsID];

        CsvReader reader = new CsvReader();

        // いたずらの読み込み
        mischiefTable = new List<Mischief>(reader.SetCsvData());

        // 無作為にいたずらタスクを抽出
        RandomlySelectedMischief();

        // いたずらタスクに番号を振る
        SetMischiefID();
		SetMischiefAnimNum();

	}

    /// <summary>
    /// フレーム更新処理
    /// </summary>
    void Update()
    {
        for(int mi = 0; mi < taskMischiefsID; mi++)
        {
            if (taskMischiefs[mi].isComp != true) break;

            if(mi == taskMischiefsID -1)
            {
                // 勝利処理を呼ぶ
                LessonManager.instance.WinnerDecided(0);
            }
        }
    }
    
    /// <summary>
    /// 無作為にいたずらタスクを抽出する処理
    /// </summary>
    private void RandomlySelectedMischief()
    {
        // 実行中に使用するいたずらを無作為に抽出)
        for (int mi = 0; mi < taskMischiefsID; mi++)
        {
            Mischief mischief = mischiefTable[UnityEngine.Random.Range(0, mischiefTable.Count)];
            // タスクリストに追加
            taskMischiefs.Add(mischief);
            int choiceNum = mischiefTable.IndexOf(mischief);
            // 重複しないように除外する
            mischiefTable.RemoveAt(choiceNum);
        }
    }

    /// <summary>
    /// いたずらタスクに番号をを設定する処理
    /// </summary>
    private void SetMischiefID()
    {
        for(int mi = 0; mi < taskMischiefsID; mi++)
        {
            Mischief mischief = taskMischiefs[mi];
            mischief.ID = mi;
            taskMischiefs[mi] = mischief;
        }
    }


	private void SetMischiefAnimNum()
	{
		for (int mi = 0; mi < taskMischiefsID; mi++)
		{
			for(int ni = 0; ni < naughty.Length; ni++)
			{
				if(taskMischiefs[mi].name == naughty[ni])
				{
					mischiefAnimNum[mi] = Array.IndexOf(naughty,naughty[ni]);
				}
			}
		}
	}


    /// <summary> 
    /// タスク開始時の処理
    /// 
    /// 呼び出し対象：プレイヤー
    /// ・引数
    /// michiefID いたずらのID
    /// </summary>
    public override Mischief BeginMischiefTask(int mischiefID)
    {
        Mischief mischief = taskMischiefs[mischiefID];
        mischief.isPlay = true;
        taskMischiefs[mischiefID] = mischief;

        return taskMischiefs[mischiefID];
    }

    /// <summary>
    /// いたずらの処理終了時に呼ばれる
    /// </summary>
    /// <param name="mischief">戻ってきたいたずら</param>
    public override void EndMischiefTask(Mischief mischief)
    {
        taskMischiefs[mischief.ID] = mischief;
    }

    /// <summary>
    /// タスクがプレイ中かチェックする
    /// </summary>
    /// <param name="mischiefID">いたずらの番号</param>
    /// <returns></returns>
    public override bool CheckTaskIsPlay(int mischiefID)
    {
        if (taskMischiefs[mischiefID].isPlay) return true;
        return false;
    }

    /// <summary>
    /// タスクがクリア済かチェックする
    /// </summary>
    /// <param name="mischiefID">いたずらの番号</param>
    /// <returns></returns>
    public override bool CheckTaskComplete(int mischiefID)
    {
        if (taskMischiefs[mischiefID].isComp) return true;
        return false;
    }

    public string GetTaskName(int index) { return taskMischiefs[index].name; }

    public bool GetTaskComp(int index)  { return taskMischiefs[index].isComp; }

	public int GetMischifAnimNum(int index) { return mischiefAnimNum[index]; }
}
