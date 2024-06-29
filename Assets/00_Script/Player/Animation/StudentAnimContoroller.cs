using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//生徒のアニメーションパラメータを管理するスクリプト
//他のスクリプトから関数を呼び出して更新する
//呼び出したスクリプトには
//河上追加アニメーション動作
//とコメント

/// <summary>
/// Managerでも使いたいので外に置きました(田中)
/// </summary>
public enum NaughtyName
{
	None = -1,
	Sleep = 0,
	Speak = 1,
	dance = 2,
	Lunch = 3,
	Domino = 4,
	eraser = 5,
	origami = 6,
	game = 7,
	snackfood = 8,

};

//AnimNaughtyNumのいたずら番号は現在仮としてマジックナンバーをおいているため、後に修正必須
public class StudentAnimContoroller : MonoBehaviour
{
    string[] naughty = new string[9] { "眠り", "おしゃべり","ダンス" ,  "早弁", "ドミノ", "練り消し", "折り紙", "ゲーム", "菓子"};

    NaughtyName naughtyNum;
    
    //アニメーターを入れる変数
    [SerializeField] private Animator animator;
    //パラメータ名を入れる変数
    string NaughtyNum = "NaughtyNum", Naughty = "DoNaughty", Deceive = "Deceive", PickUp = "PickUp",Normal = "Normal",ReadAloud = "ReadAloud";

    // Start is called before the first frame update
    void Start()
    {
        naughtyNum = NaughtyName.None;
        //現在アタッチされているオブジェクトのアニメーターを入れる
        //animator = this.GetComponent<Animator>();
        //パラメータを初期化
        //ここの第２引数を変えるとアニメーション勝手に動き出すので変えない
        //------------------------------------------
        animator.SetInteger(NaughtyNum,-1);
        animator.SetBool(Naughty, false);
        animator.SetBool(Deceive, false);
        //------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        //通常時の動きを入れる
        AnimNomalMove();
    }
    /// <summary>
    /// アニメーションパラメータ"NaughtyNum"の番号を切り替える関数
    /// </summary>
    /// いたずらは名前で管理していてランダムで8個自動生成
    /// 生成した順番にIDがつくられるため特定の番号はない
    /// いたずら生成時にパラメータ内のintがどこに向かうか設定する
    /// もしくはQTE開始時に番号を拾ってあげられるように設定するか
    public void AnimNaughtyNum(string NaughtyName)
    {
        //実行しているいたずらの名前がstring型の配列の値にあたるまでループ
        for(int i = 0;i < 9;++i)
        {
            //同じ名前があったとき
            if(NaughtyName == naughty[i])
            {
                //いたずら番号に配列の番号をいれる
                naughtyNum = (NaughtyName)i;
            }
}
        //番号を設定
        animator.SetInteger(NaughtyNum, (int)naughtyNum);
        //いたずら実行を呼び出す
        AnimDoNaughty();
    }

    /// <summary>
    /// アニメーションパラメータの"PickUp"のトリガーをオンにする関数
    /// </summary>
    /// 指摘された時トリガーをオンにする
    public void AnimPickUp()
    {
        //条件用変数：いたずらしているかをとる
        bool DoNaughty = animator.GetBool(Naughty);
        //いたずらしていないときは返す
        if (!DoNaughty) return;
        //指摘されたトリガーを入れる
        animator.SetTrigger(PickUp);
    }
    /// <summary>
    /// アニメーションパラメータ"DoNaughty"の値をtrueにする関数
    /// </summary>
    /// いたずら開始のとき実行
    public void AnimDoNaughty()
    {
        //現在いたずら中かを入れる
        bool flag = animator.GetBool(Naughty);
        //いたずらをしているときは返す
        if (flag) return;
        //いたずらを始める
        animator.SetBool(Naughty, true);
            
    }
    /// <summary>
    /// アニメーションパラメータ"DoNaughty"の値をfalseにする関数
    /// </summary>
    /// いたずらが完成したときに呼び出す
    public void AnimEndNaughty()
    {
        //現在いたずらしているかを入れる
        bool flag = animator.GetBool(Naughty);
        //いたずらをしていないときは返す
        if (!flag) return;
        //いたずらをやめる
        animator.SetBool(Naughty, false);
    }
    /// <summary>
    /// アニメーションパラメータ"Deceive"の値をtrueにする関数
    /// </summary>
    /// ごまかし完了時
    public void AnimDeceive()
    {
        //条件用変数：いたずら中かをとる
        bool DoNaughty = animator.GetBool(Naughty);
        //いたずら中でないときは返す
        if (!DoNaughty) return;
        //ごまかし中かをいれる変数
        bool flag = animator.GetBool(Deceive);
        //ごまかしを完了していないとき　且つ　ごまかしが成功している
        if (!flag)
        {
            //ごまかしをオンにする
            animator.SetBool(Deceive, true);
            //いたずらをオフにする
            animator.SetBool(Naughty, false);
        }
    }
    /// <summary>
    /// 通常時にアニメーションをランダムで動かすアニメーション
    /// </summary>
    private void AnimNomalMove()
    {
        //条件用変数
        bool naughty = animator.GetBool(Naughty);           //いたずら中か
        bool deceive = animator.GetBool(Deceive);           //ごまかし中か
        //いたずら もしくは　ごまかし　をしているときは返す
        if ((naughty) || (deceive)) return;

        //変数宣言
        //---------------------------------------------------------------------------------------------
        bool doRandom = false, doAnimation = false;             //ランダム、アニメーションを行うかのフラグ
        float randomIndex = 0.0f;                               //ランダムの値
        int moveCount = 3;                                      //通常時のアニメーションクリップ数
        int normalMoveIndex = Random.Range(0, moveCount);       //動きの番号
        float time = 0.0f;                                      //時間
        //---------------------------------------------------------------------------------------------


        //ランダムがまだ行われていないとき
        if (!doRandom)
        {
            //ランダムで値を入れる
            randomIndex = Random.Range(0.0f, 10.0f);
            //毎フレームで更新しないようにする
            doRandom = true;
        }
        //ランダムの値が９を越えていた　且つ
        //アニメーションが実行されていないとき
        if (randomIndex >= 9.0f && !doAnimation)
        {
            //行うアニメーションの番号を入れる
            Debug.Log($"{normalMoveIndex}");
            animator.SetInteger(Normal, normalMoveIndex);
            //次回用にランダムで値をとる
            normalMoveIndex = Random.Range(0, moveCount);
            //次のランダムまで繰り返さないようにする
            doAnimation = true;
        }
        //タイムを１増やす
        ++time;
        //タイムが４秒以上のとき
        if (time >= Time.deltaTime * 4.0f)
        {
            //タイムをリセット
            time = 0.0f;
            //再度ランダムをかける
            doRandom = false;
            //もう一度アニメーションできるようにしておく
            doAnimation = false;
        }
    }
	/// <summary>
	/// アニメーションパラメータ"ReadAloud"をtrueにする関数
	/// </summary>
	private void AnimStartReadAloud()
	{
		//現在の"ReadAloud"の状態をとる
		bool flag = animator.GetBool(ReadAloud);
		//すでにお音読をしている時は返す
		if (flag) return;
		//パラメータをtrueにする
		animator.SetBool(ReadAloud, true);
	}
	/// <summary>
	/// アニメーションパラメータ"ReadAloud"をfalseにする関数
	/// </summary>
	private void AnimEndReadAloud()
	{
		//現在の"ReadAloud"の状態をとる
		bool flag = animator.GetBool(ReadAloud);
		//すでにお音読をしている時は返す
		if (!flag) return;
		//パラメータをfalseにする
		animator.SetBool(ReadAloud, false);
	}
}
