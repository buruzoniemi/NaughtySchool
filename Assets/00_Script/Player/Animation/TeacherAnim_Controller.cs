using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//教師のアニメーションパラメータを管理するスクリプト
//他のスクリプトから関数を呼び出して更新する
//呼び出したスクリプトには
//河上追加アニメーション動作
//とコメント
public class TeacherAnim_Controller : MonoBehaviour
{
    //アニメータを入れる変数
    Animator animator;
    //パラメータ名を入れる変数
    private string LookUp = "LookUp", Toward = "Toward", IsWrite = "IsWrite", Point = "Point", Hit = "Hit",ReadAloud = "ReadAloud";
    // Start is called before the first frame update
    void Start()
    {

        //現在アタッチされているオブジェクトのアニメータを入れる
        animator = this.GetComponent<Animator>();
        //アニメーションパラメータの初期化をする
        //ここの第２引数を変えるとアニメーション勝手に動き出すので変えない
        //-----------------------------------
        animator.SetBool(LookUp, true);       //生徒か教卓を見る
        animator.SetBool(Toward, false);      //黒板を向くか生徒側を向くか
        animator.SetBool(IsWrite, false);     //黒板に書いてるか書いてないか
        animator.SetBool(Point, false);       //指摘をしたかしてないか
        animator.SetBool(Hit, false);         //指摘が成功したかしてないか
        //------------------------------------
    }

    /// <summary>
    /// アニメーションパラメータの"LookUp"の値をtrueにする関数
    /// </summary>
    /// <param name="context"></param>
    /// 下向くボタンが離されたとき(上を向くとき)のみ実行
    public void AnimLookUp()
    {
        //現在の"LookUp"の値をとる
        bool flag = animator.GetBool(LookUp);
        //生徒を見ている場合返す
        if (flag) return;
        //生徒の方に向ける
        animator.SetBool(LookUp, true);
    }

    /// <summary>
    /// アニメーションパラメータの"LookUp"の値をfalseにする関数
    /// </summary>
    /// <param name="context"></param>
    ///下向くボタンが押された(下を向く)ときのみ実行
    public void AnimLookDown()
    {
        //現在の"LookUp"の値をとる
        bool flag = animator.GetBool(LookUp);
        //下を向いていないとき返す
        if (!flag) return;
        //下に向ける
        animator.SetBool(LookUp, false);
    }

    /// <summary>
    /// アニメーションパラメータの"Point"の値をtrueにする関数
    /// </summary>
    /// <param name="context"></param>
    /// 指摘をしていないときのみ実行
    public void AnimUsePoint()
    {
		
        //現在の"Point"の値をとる
        bool flag = animator.GetBool(Point);
		Debug.Log("通ってる" + $"{flag}");
		//条件用変数：生徒側を見てるかのアニメーションパラメータをとる
		bool lookUp = animator.GetBool(LookUp);
        //生徒を見ていない、もしくは指摘を既にしているときは返す
        if ((!lookUp) || (flag)) return;
        //指摘を開始する
        animator.SetBool(Point, true);
    }

    /// <summary>
    /// アニメーションパラメータの"Hit"の値をtrueにする関数
    /// </summary>
    /// <param name="context"></param>
    /// 
    public void AnimHit(bool PointHit)
    {
        //条件用変数：指摘を押したかのパラメータをとる
        bool point = animator.GetBool(Point);
        //指摘が押されていないときは返す
        if (!point) return;
        //"Hit"をtrueにする
        animator.SetBool(Hit, PointHit);
        //指摘を行ったのでパラメータをfalseにしておく
        animator.SetBool(Point, false); 
    }

    /// <summary>
    /// アニメーションパラメータの"Toward"の値をtrueにする関数
    /// </summary>
    /// <param name="context"></param>
    public void AnimTowardBlackBoard()
    {
        //現在の"Toward"の値をとる
        bool flag = animator.GetBool(Toward);
        //黒板の方へ振り返っているときは返す
        if (flag) return;
        //黒板の方へ振り返る
        animator.SetBool(Toward, true);
    }

    /// <summary>
    /// アニメーションパラメータの"Toward"の値をflaseにする関数
    /// </summary>
    /// <param name="context"></param>
    public void AnimTowardStudent()
    {
        //現在の"Toward"の値をとる
        bool flag = animator.GetBool(Toward);
        //振り返っていないときは返す
        if (!flag) return;

        //条件用変数
        bool write = animator.GetBool(IsWrite);//黒板に書いているかをとる
        //板書しているとき板書をやめる
        if (write) AnimStopWrite();

        //生徒側に振り返る
        animator.SetBool(Toward, false);
    }

    /// <summary>
    /// アニメーションパラメータの"IsWrite"の値をtrueにする関数
    /// </summary>
    /// <param name="context"></param>
    public void AnimWrite()
    {
        //条件用変数
        bool DoToward = animator.GetBool(Toward);   //黒板の方へ振り返っているか
        bool flag = animator.GetBool(IsWrite);      //現在の"IsWrite"の値をとる
		//黒板に振り返っていないとき、もしくは書いているときは返す
		if ((!DoToward) || (flag)) return;

        //書き始める
        animator.SetBool(IsWrite, true);
    }

    /// <summary>
    /// アニメーションパラメータの"IsWrite"の値をfalseにする関数
    /// </summary>
    /// <param name="context"></param>
    public void AnimStopWrite()
    {
        //現在の"IsWrite"の値をとる
        bool flag = animator.GetBool(IsWrite);
        //描いていない時は返す
        if (!flag) return;
        //板書をやめる
        animator.SetBool(IsWrite, false);
    }

	/// <summary>
	/// 音読のトリガーをオンにする関数
	/// </summary>
	/// 生徒を見ているときのみ使用可
	public void AnimReadAloud()
	{
		//条件用変数
		bool lookUp = animator.GetBool(LookUp);
		//生徒を向いていないときは返す
		if (!lookUp) return;
		//音読のトリガーをオンにする
		animator.SetTrigger(ReadAloud);
	}
}
