using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBoardText : MonoBehaviour
{
    [SerializeField] private LessonProgress lessonProgress; //授業進捗度を取得するもの
    [SerializeField] private List<string> blackBoardTextList; // あいうえおの順にテキストを格納したリスト
    [SerializeField] private Text blackBoardtextLine; //黒板に乗っけておく用の1行目Text
    [SerializeField] private Camera frontCamera;

    private const float blackBoardtextValueMax = 100; //黒板のText進行度を判断する用の値
    private float progressBarValue; //授業進捗度の値を格納しておく
    private int currentTextIndex; // 現在表示するテキストのインデックス

    private void Start()
    {
        currentTextIndex = 0;
        // テキストを改行文字で分割してリストに追加する
        string[] lines = blackBoardtextLine.text.Split('\n');
        blackBoardTextList = new List<string>(lines);
        blackBoardtextLine.text = new(" ");
    }

    // Update is called once per frame
    void Update()
    {
        AddPorgreesBarValue();
        UpdateProgressText();
    }

    /// <summary>
    /// 授業進行度を常に取得する処理
    /// </summary>
    private void AddPorgreesBarValue()
    {
        //Debug.Log("通ってるよ");
        if (progressBarValue > blackBoardtextValueMax) return;
        progressBarValue = lessonProgress.SendProgressBar();
    }

    /// <summary>
    /// プログレステキストを更新する処理
    /// </summary>
    private void UpdateProgressText()
    {
        if (currentTextIndex > blackBoardTextList.Count) return;

        // progressBarValueに基づいてテキストを更新
        if (progressBarValue >= (currentTextIndex + 1) * (1f / blackBoardTextList.Count))
        {
            blackBoardtextLine.text += blackBoardTextList[currentTextIndex] + "\n";
            currentTextIndex++; // 次のテキストに更新
        }
    }
}
