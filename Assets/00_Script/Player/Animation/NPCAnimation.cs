using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{

    private Animator animator;                                  //アニメータ管理用
    private string StudyTrigger = "Study", moving = "move";     //パラメータ名
    private bool doRandom,doAnimation;                          //ランダムを行うか、アニメーションを行うか
    private float randomIndex;                                  //ランダムの値
    private int moveIndex;                                      //行動番号
    private int moveCount;                                      //行動のアニメーション数
    
    // Start is called before the first frame update
    void Start()
    {
        //アタッチされているAnimatorを入れる
        animator = this.GetComponent<Animator>();
        doRandom = false;
        doAnimation = false;
        moveCount = 4;
        moveIndex = Random.Range(0, moveCount);
    }

    // Update is called once per frame
    void Update()
    {
        //時間
        float time = 0.0f;
        //ランダムがまだ行われていないとき
        if (!doRandom)
        {
            //ランダムで値を入れる
            randomIndex = Random.Range(0.0f, 10.0f);
            //毎フレームで更新しないようにする
            doRandom = true;
            
        }
        //ランダムの値が９を越えていた且つ
        //アニメーションが実行されていないとき
        if(randomIndex >= 9.0f && doAnimation == false)
        {
            //行うアニメーションの番号を入れる
            animator.SetInteger(moving, moveIndex);
            //次回用にランダムで値をとる
            moveIndex = Random.Range(0, moveCount);
            //アニメーションを行うためのトリガーをオンにする
            animator.SetTrigger(StudyTrigger);
            //次のランダムまで繰り返さないようにする
            doAnimation = true;
        }
        //タイムを１増やす
        ++time;
        //タイムが４秒以上のとき
        if(time >= Time.deltaTime * 4.0f)
        {
            //タイムをリセット
            time = 0.0f;
            //再度ランダムをかける
            doRandom = false;
            //もう一度アニメーションできるようにしておく
            doAnimation = false;
        }

    }
}
