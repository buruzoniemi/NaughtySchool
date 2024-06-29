using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Spawner : MonoBehaviour
{
    // 変数宣言-----------------------------

    [SerializeField] private GameObject studentPrefab; // 生成する生徒オブジェクト
    [SerializeField] private GameObject teacherPrefab; // 生成する先生オブジェクト
    [SerializeField] private Vector3 teacherPos; //生成する先生オブジェの位置
    [SerializeField] private Vector3 studentPos; //生成する生徒オブジェの位置
    [SerializeField] private SelectManager selectManager; //先生オブジェクトのSelectManagerを取得する

    [SerializeField] private float objAddRotate;
    [SerializeField] private float objAddScale;

    private static readonly int arrayRows = 3; //配列の横の要素数
    private static readonly int arrayCols = 5; //配列の縦の要素数

    private static readonly float testEqualIntervalsColsPos = 25; //テスト時の縦に等間隔に配置する用の基準点
    private static readonly float testEqualIntervalsRowsPos = 35; //テスト時の横に等間隔に配置する用の基準点
    private static readonly float equalIntervalsStartColsPos = -3.06f; //縦に等間隔に配置する用のスタートの基準点
    private static readonly float equalIntervalsEndColsPos = 1.29f; //縦に等間隔に配置する用の終わりの基準点
    private static readonly float equalIntervalsStartRowsPos = -3.96f; //横に等間隔に配置する用のスタートの基準点
    private static readonly float equalIntervalsEndRowsPos = -0.88f; //横に等間隔に配置する用のの基準点
    private static readonly float equalIntervalsRealignment = -0.11f; //等間隔のずれなおし

    //以下の変数は実装が確認でき次第変数名の変更をする。
    public GameObject[,] studentObjArray; //生徒のオブジェクトを配列で配置する
    //------------------------------------

    private void Awake()
    {
        //初期化
        studentObjArray = new GameObject[arrayRows, arrayCols];

        //TestSeceneにしか使わないメソッド
        //生徒をNPC体分生成する処理
        //TestStudentNPCSpawn();
        StudentNPCSpawn();
    }

    /// <summary>
    /// TestSeceneにしか使わないメソッド
    /// PlayerCountを受け取ってその数分StudentObjを生成する関数
    /// </summary>
    private void TestStudentNPCSpawn()
    {
        //Debug_Manager.instance.DebugLog($"{Player_Count.playerCount}");
        //studentObjに中身があるかどうかを判断
        if (studentPrefab != null)
        {
            //プレイヤー数分繰り返し
            // 二次元配列内の各要素にオブジェクトを生成
            for (int i = 0; i < studentObjArray.GetLength(0); i++)
            {
                for (int j = 0; j < studentObjArray.GetLength(1); j++)
                {
                    //あらかじめスポーンする前に位置を変えておく
                    studentPos.x = testEqualIntervalsRowsPos * (i + 1);    //xの位置
                                                                       //yの位置(今回は使わない)
                    studentPos.z = testEqualIntervalsColsPos * (j + 1);  //zの位置

                    // 異なる位置にオブジェクトを生成するために、位置情報を利用してインスタンス化
                    GameObject newObject = Instantiate(studentPrefab,
                                                       new Vector3(studentPos.x, studentPos.y, studentPos.z),
                                                       Quaternion.identity);
                    newObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    // 生成したオブジェクトを二次元配列に格納
                    studentObjArray[i, j] = newObject;

                    //仮の処理　後で絶対消す-----------------------------------
                    Vector3 lossyScale = studentObjArray[i, j].transform.lossyScale;
                    //----------------------------------

                    //大きさ変更
                    studentObjArray[i, j].transform.localScale = new Vector3(studentObjArray[i, j].transform.localScale.x * objAddScale,
                                                                             studentObjArray[i, j].transform.localScale.y * objAddScale,
                                                                             studentObjArray[i, j].transform.localScale.z * objAddScale);
                }
            }
        }
        else
        {
            //エラーメッセージの表示
            Debug.LogError("StudentObj：null");
            return; // 処理を中断する
        }
    }

    /// <summary>
    /// PlayerCountを受け取ってその数分StudentObjを生成する関数
    /// </summary>
    private void StudentNPCSpawn()
    {
        float equalIntervalsRowsPos   = (UnsignedVariable(equalIntervalsEndRowsPos) + UnsignedVariable(equalIntervalsStartRowsPos)) / (arrayRows) + equalIntervalsRealignment; //横に等間隔に配置する用の基準点
        float equalIntervalsColsPos = (UnsignedVariable(equalIntervalsEndColsPos) + UnsignedVariable(equalIntervalsStartColsPos)) / (arrayCols - 1);   //縦に等間隔に配置する用の基準点

        //Debug.Log("equalIntervalsRowsPos：" + $"{equalIntervalsRowsPos}");
        //Debug.Log("equalIntervalsColsPos：" + $"{equalIntervalsColsPos}");

        //Debug_Manager.instance.DebugLog($"{Player_Count.playerCount}");
        //studentObjに中身があるかどうかを判断
        if (studentPrefab == null)
        {
            //エラーメッセージの表示
            Debug.LogError("StudentObj：null");
            return; // 処理を中断する
        }

        //プレイヤー数分繰り返し
        // 二次元配列内の各要素にオブジェクトを生成
        for (int i = 0; i < studentObjArray.GetLength(0); i++)
        {
            for (int j = 0; j < studentObjArray.GetLength(1); j++)
            {
                //Debug.Log("equalIntervalsColsPos" + $"{j}" + "：" + $"{equalIntervalsRowsPos * j}");
                //Debug.Log("equalIntervalsRowsPos："  + $"{i}" + "：" + $"{equalIntervalsRowsPos * i}");

                //あらかじめスポーンする前に位置を変えておく
                studentPos.x = equalIntervalsEndColsPos - equalIntervalsColsPos * (j);  //xの位置
                studentPos.y = 0.5f;                                                    //yの位置(今回は使わない)
                studentPos.z = equalIntervalsEndRowsPos - equalIntervalsRowsPos * (i);  //zの位置

                // 異なる位置にオブジェクトを生成するために、位置情報を利用してインスタンス化
                GameObject newObject = Instantiate(studentPrefab,
                                                   new Vector3(studentPos.x, studentPos.y, studentPos.z),
                                                   Quaternion.identity);
                newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                // 生成したオブジェクトを二次元配列に格納
                studentObjArray[i, j] = newObject;

                //仮の処理　後で絶対消す-----------------------------------
                Vector3 lossyScale = studentObjArray[i, j].transform.lossyScale;
                //----------------------------------

                //大きさ変更
                studentObjArray[i, j].transform.localScale = new Vector3(studentObjArray[i, j].transform.localScale.x * objAddScale,
                                                                         studentObjArray[i, j].transform.localScale.y * objAddScale,
                                                                         studentObjArray[i, j].transform.localScale.z * objAddScale);
            }
        }
    }

    /// <summary>
    /// 生徒NPCとPlayerを変更する
    /// </summary>
    /// <param name="Rows">行</param>
    /// <param name="Cols">列</param>
    public void StudentObjectsChange(int Rows, int Cols, GameObject student)
    {
        // 臨時処理(Scale変更)
        // Studentをデカくする
        student.transform.localScale = new Vector3(student.transform.localScale.x * objAddScale,
                                                   student.transform.localScale.y * objAddScale,
                                                   student.transform.localScale.z * objAddScale);
        // 令和のクソスクリプト
        // 無理やりUIを小さくします
        // まず仮マップを10倍で作ったのは誰なんですかね...?
        GameObject ParentCanvas = student.transform.Find("Canvas").gameObject;

        RectTransform[] canvas = new RectTransform[4];
        canvas[0] = ParentCanvas.transform.Find("PalletUI").gameObject.GetComponent<RectTransform>();
        canvas[1] = ParentCanvas.transform.Find("OperationUI").gameObject.GetComponent<RectTransform>();
        canvas[2] = ParentCanvas.transform.Find("JudgeUI").gameObject.GetComponent<RectTransform>();
        canvas[3] = ParentCanvas.transform.Find("ProgressCircleUI").gameObject.GetComponent<RectTransform>();

        for (int ci = 0; ci < 4; ci++)
        {
            canvas[ci].localScale = new Vector3(canvas[ci].localScale.x / 10,
                                                canvas[ci].localScale.y / 10,
                                                canvas[ci].localScale.z / 10);
        }


        Debug.Log($"{studentObjArray[Rows, Cols].transform.position}");
        Debug.Log($"{studentObjArray[Rows, Cols].transform.rotation}");
        Debug.Log($"{student}");

        Vector3 Pos = studentObjArray[Rows, Cols].transform.position;
        Quaternion Rotate = studentObjArray[Rows, Cols].transform.rotation;
        Debug.Log("Rotate："+ $"{Rotate}");

        //ランダムで選ばれたオブジェクトを削除する
        Destroy(studentObjArray[Rows, Cols]);
        Destroy(selectManager.studentObjects[Rows, Cols]);
        //削除したオブジェクトにnullを代入する
        studentObjArray[Rows, Cols] = student;
        selectManager.studentObjects[Rows, Cols] = student;
        //転送したObjectを元の位置に送る
        student.transform.position = Pos;
        student.transform.rotation = Rotate;
        Debug.Log($"{studentObjArray[Rows, Cols].transform.position}");
        Debug.Log($"{studentObjArray[Rows, Cols].transform.localRotation.y}");
        Debug.Log($"{studentObjArray[Rows, Cols]}");
    }

    public GameObject SendStudentArray(int Rows, int Cols)
    {
        return studentObjArray[Rows, Cols];
    }

    private float UnsignedVariable(float Variable)
    {
        if(Variable < 0.0f)
        {
            return -Variable;
        }
        return Variable;
    }
}
