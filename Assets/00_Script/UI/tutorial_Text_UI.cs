using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class tutorial_Text_UI : MonoBehaviour
{
    // 変数宣言-------------------------------------------------------
    private bool shouldChangeToTutorialScene; //チュートリアルシーンに飛ぶかどうか
    private bool isInTutorialSceneTransition; //シーンの切替処理に入ったかどうか
    private string tutorialSceneName; //チュートリアルシーンの名前を入れるとこ
    private string inGameSceneName; //インゲームの名前を入れるとこ
    private float flashAlpha; //点滅する際のアルファ値

    private static readonly float displayAlpha = 1.0f; //表示
    private static readonly float hiddenAlpha = 0.2f; //非表示 
    
    [SerializeField, Header("点滅するスピードの倍率")]private float flashSpeed; //点滅するスピード
    [SerializeField,Header("シーンの切り替えの待ち時間")] private float sceneChangeDelayTime; //シーン切替の待ち時間
    [SerializeField, Header("YesのTextのPrafub")] private Text textYes; // Text YesのPrafub
    [SerializeField, Header("NoのTextのPrafub")] private Text textNo; // Text NoのPrafub
    //---------------------------------------------------------------

    /// <summary>
    /// スタート時に処理する
    /// </summary>
    void Start()
    {
        //変数の初期化
        shouldChangeToTutorialScene = false;
        isInTutorialSceneTransition = false;
        tutorialSceneName = "SampleScene";
        inGameSceneName = "SampleScene";
        flashAlpha = 0.0f;

        //Yesテキストのアルファ値を下げる
        textYes.color = new(1.0f, 1.0f, 1.0f, hiddenAlpha);
        //Noテキストのアルファ値を上げる
        textNo.color = new(1.0f, 1.0f, 1.0f, displayAlpha);
    }

    /// <summary>
    /// 毎時処理
    /// </summary>
    private void Update()
    {
        //文字の点滅
        flashAlpha = Mathf.Sin(Time.time * flashSpeed);
        //シーンの切り替え処理に入ったかどうか
        if(isInTutorialSceneTransition == true)
        {
            TextFlash();
        }
    }

    /// <summary>
    /// 特定のボタンを押したときYesのTextを光らせる
    /// </summary>
    /// <param name="playerInput">押された値</param>
    public void OnTextYesFlash(InputAction.CallbackContext context)
    {
        //押された瞬間のみ取得する
        if (!context.performed) return;

        //Yesテキストのアルファ値を上げる
        textYes.color = new(1.0f, 1.0f, 1.0f, displayAlpha);
        //Noテキストのアルファ値を下げる
        textNo.color = new(1.0f, 1.0f, 1.0f, hiddenAlpha);
        //チュートリアルシーンに移行するフラグを立てる
        shouldChangeToTutorialScene = true;
    }

    /// <summary>
    /// 特定のボタンを押したときNoのTextを光らせる
    /// </summary>
    /// <param name="playerInput">押された値</param>
    public void OnTextNoFlash(InputAction.CallbackContext context)
    {
        //押された瞬間のみ取得する
        if (!context.performed) return;

        //Yesテキストのアルファ値を下げる
        textYes.color = new(1.0f, 1.0f, 1.0f, hiddenAlpha);
        //Noテキストのアルファ値を上げる
        textNo.color = new(1.0f, 1.0f, 1.0f, displayAlpha);
        //チュートリアルシーンに移行するフラグを折る
        shouldChangeToTutorialScene = false;
    }

    /// <summary>
    /// 特定のボタンを押したとき決定し、シーンを移行させる
    /// </summary>
    public void OnTextDecisionFlash(InputAction.CallbackContext context)
    {
        //押された瞬間のみ取得する
        if (!context.performed) return;

        //シーンの切替処理に入った
        isInTutorialSceneTransition = true;
        //チュートリアルシーンに移行するフラグが立っていたらチュートリアルシーンに移行する
        if (shouldChangeToTutorialScene == true)
        {
            //シーンの切替処理
            StartCoroutine(ReceiveFragSceneChange.SceneChange(sceneChangeDelayTime, tutorialSceneName));
        }
        //フラグが立っていなかったらインゲームに移行する
        else
        {
            //シーンの切替処理
            StartCoroutine(ReceiveFragSceneChange.SceneChange(sceneChangeDelayTime, inGameSceneName));
        }
    }

    /// <summary>
    /// 各種テキストを光らせたり暗くする処理
    /// </summary>
    private void TextFlash()
    {
        //もしチュートリアルシーンに遷移するかどうかのフラグをチェック
        if(shouldChangeToTutorialScene == true)
        {
            //Yesのテキストを点滅させる
            textYes.color = new(1.0f, 1.0f, 1.0f, flashAlpha);
        }
        else
        {
            //Noのテキストを点滅させる
            textNo.color = new(1.0f, 1.0f, 1.0f, flashAlpha);
        }
    }
}
