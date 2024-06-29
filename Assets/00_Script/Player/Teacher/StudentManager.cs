using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    private int studentTargetRows;         //生徒の行の数を格納する変数
    private int studentTargetClos;         //生徒の列の数を格納する変数
    private bool[,] studentInAction;       //生徒がアクションされたかどうかを判断する変数
    public float[,] selectTimes;           //強制拘束時間計算
    private float LimitTime = 5.0f;        //摘発されて強制拘束時間

    void Start()
    {
        //初期化
        studentInAction = new bool[3, 5];
        selectTimes = new float[3, 5];
        for (int i = 0; i < studentInAction.GetLength(0); i++)
        {
            for (int j = 0; j < studentInAction.GetLength(1); j++)
            {
                studentInAction[i, j] = false;
                selectTimes[i, j] = 0.0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StudentAction();
    }

    private void StudentAction()
    {
        //選択中の目標を確認
        studentTargetRows = SelectManager.currentTargetRows;
        studentTargetClos = SelectManager.currentTargetClos;
        //目標状態更新                                            
        for (int i = 0; i < studentInAction.GetLength(0); i++)
        {
            for (int j = 0; j < studentInAction.GetLength(1); j++)
            {

                studentInAction[i,j] = SelectManager.studentInAction[i,j];

                //選択中になれば
                if (studentInAction[i,j])
                {
                    selectTimes[i,j] += Time.deltaTime;

                    //制限時間が超える場合
                    if (selectTimes[i,j] >= LimitTime)
                    {
                        SelectManager.studentInAction[i,j] = false;
                        selectTimes[i,j] = 0; //タイムリセット
                    }
                }
                else

                {
                    selectTimes[i,j] = 0;
                }
            }
        }
    }
}
