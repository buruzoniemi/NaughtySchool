using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    //カメラ（二つウィンドウ対応）
    public Camera teacherCamera;
    public Camera studentCamera;
    //花火プレハブ
    public GameObject [] fireworksPrefab;
    //勝負判定
    private bool isTeacherWin;
    private bool isStudentWin;
    //シーンを切り替えるまで待ち時間
    private float resultTime;
    //表示テキスト
    public Text Result;
    public Text PlayerResultText;
    private int ResultIndex;
    public string ResultText = "結果発表";
    private float typingSpeed = 0.7f;
    private float typingTime;
    //結果発表を中央から上に移動処理
    private float middleTime = 2.0f;
    private Vector3 resultPos;
    bool isResultText = false;
    private float resultMoveTime = 0;
    private bool isResultMoved = false;
    //花火処理
    private bool isFireworks = false;
    //表示文字イメージ
    public Image[] teacherWin;
    public Image[] studentWin;
    private int winImageLength = 0;
    private float appearSpeed = 1f;
    private float scaleSpeed = 0.5f;
    private bool lastImages = false;
    private float scaleAmount = 2f;

    //BGM
    public AudioSource backgroundMusic;
    public Image black;
    private float sceneOverTime;          //シーンを切り替える計算
    private float sceneChangeTime = 2.0f; //シーンを切り替えるまでの時間
    private bool isSceneChange;


    void Start()
    {
        //初期化
        isTeacherWin = LessonManager.teacherWin;
        isStudentWin = LessonManager.studentWin;
        //isStudentWin = true;
        //isTeacherWin = true;
        ResultIndex = 0;
        typingTime = 0;
        resultTime = 0;
        sceneOverTime = 0;
        resultPos = Result.rectTransform.anchoredPosition;
        isSceneChange = false;
        //モニターチェック
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            //先生視角はモニター１、生徒は２
            teacherCamera.targetDisplay = 0;
            studentCamera.targetDisplay = 1;
        }
        else
        {
            teacherCamera.targetDisplay = 0;
        }
        //文字透明化
        for (int i = 0; i < teacherWin.Length; i++)
        {
            teacherWin[i].color = new Color(teacherWin[i].color.r, teacherWin[i].color.g, teacherWin[i].color.b, 0);
        }
        for (int i = 0; i < studentWin.Length; i++)
        {
            studentWin[i].color = new Color(studentWin[i].color.r, studentWin[i].color.g, studentWin[i].color.b, 0);
        }
        //BGMを設定
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true; //繰り返すプレイ
            backgroundMusic.Play(); //音声プレイ
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResultTextIndication();// 結果テキストの表示
        if (isResultText)
        {
            MoveResultText();// 結果テキストの移動
        }
        if(isResultMoved)
        {
            PlayerResult();// プレイヤーの結果表示
        }
    }

    //結果表示
    private void ResultTextIndication()
    {
        typingTime += Time.deltaTime;
        if(!isResultText)
        {
            if (typingTime >= typingSpeed)
            {
                typingTime = 0;
                Result.text += ResultText[ResultIndex];
                ResultIndex++;
                if (ResultIndex >= ResultText.Length)
                {
                    // 全てのテキストを表示したら
                    isResultText = true;
                }
            }
        }
        else
        {
            // テキスト表示完了後の処理
            typingTime = 0;
            resultMoveTime += Time.deltaTime;
        }


    }
    //プレイヤーの結果
    private void PlayerResult()
    {
        resultTime += Time.deltaTime;
        if (resultTime >= 0.0f)
        {
            //PlayerResultText.text = "勝者は";
            if(isTeacherWin || isStudentWin)
            {
                if (!isFireworks)
                {
                    FireworksContorl();
                    isFireworks = true;
                }
            }
        }
        if (resultTime >= 1.0f)
        {
            // 勝者に応じたテキストの設定
            if (isTeacherWin && !isStudentWin)
            {
                //PlayerResultText.text = "先生";
                teacherWinImage();
            }
            else if (!isTeacherWin && isStudentWin)
            {
                //PlayerResultText.text = "生徒";
                studentWinImage();
            }
            else
            {
                PlayerResultText.text = "残念、勝者いない(｡•́︿•̀｡)";
            }
        }
       

        if (resultTime >= 4.0f && Input.anyKeyDown)
        {
            isSceneChange = true;
        }
        if(isSceneChange)
        {
            sceneOverTime += Time.deltaTime;
            //不透明になる処理
            if (sceneOverTime < sceneChangeTime)
            {
                float alphaChnage = Time.deltaTime / sceneChangeTime;
                if (black != null)
                    black.color = new Color(black.color.r, black.color.g, black.color.b, black.color.a + alphaChnage * 1.1f);
            }
            //音声の大きさを減らす
            backgroundMusic.volume -= 0.7f * Time.deltaTime;
            if (sceneOverTime >= 2.0f)
            {
                backgroundMusic.Stop();
                SceneManager.LoadScene("Title");
            }
        }
    }
    //結果文字移動
    private void MoveResultText()
    {
        if (resultMoveTime >= middleTime)
        {
            // 目標位置
            Vector3 targetPos = new Vector3(resultPos.x, resultPos.y + 450, resultPos.z);//上に移動の位置
            Vector3 targetScale = new Vector3(0.7f, 0.7f, 1);//縮小の範囲
            //移動と縮小の処理
            Result.rectTransform.anchoredPosition = Vector3.Lerp(Result.rectTransform.anchoredPosition, targetPos, Time.deltaTime * 1.0f);
            Result.transform.localScale = Vector3.Lerp(Result.transform.localScale, targetScale, Time.deltaTime * 1.0f);

            if (Vector3.Distance(Result.rectTransform.anchoredPosition, targetPos) < 2f && Vector3.Distance(Result.transform.localScale, targetScale) < 2f)
            {
                isResultMoved = true;
            }
        }
    }
    private void FireworksContorl()
    {
        //花火の位置設定
        float xDistance = (19 - (-19)) / (fireworksPrefab.Length -1 ); 
        for (int i = 0; i < fireworksPrefab.Length; i++)
        {
            float xPos = -19 + (xDistance * i);
            float yPos;
            if(i%2 == 0)
            {
                yPos = -28f;
            }
            else
            {
                yPos = -22f;
            }
            Vector3 fireworkPosition = new Vector3(xPos, yPos, -940);
            Quaternion fireworkRotation = Quaternion.Euler(-90, 0, 0);
            GameObject fireworkInstance = Instantiate(fireworksPrefab[i % fireworksPrefab.Length], fireworkPosition, fireworkRotation);

            ParticleSystem fireworkParticleSystem = fireworkInstance.GetComponent<ParticleSystem>();
            if (fireworkParticleSystem != null)
            {
                fireworkParticleSystem.Play();// 花火の再生
            }
        }
    }

    //先生勝利の文字処理
    private void teacherWinImage()
    {
        //先生の最初の5文字表示
        if (winImageLength < 5)
        {
            Image image = teacherWin[winImageLength];
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1f, appearSpeed * Time.deltaTime * 2.0f));
            if (image.color.a >= 1f)
            {
                winImageLength++;
            }
        }
        else if (winImageLength >= 4 && !lastImages)
        {
            //文字を2倍になる
            for (int i = 5; i < teacherWin.Length; i++)
            {
                Image image = teacherWin[i];
                image.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);
            }
            lastImages = true;
        }
        //完遂の文字を縮小する
        if (lastImages)
        {
            //文字を表示しながら縮小
            for (int i = 5; i < teacherWin.Length; i++)
            {
                Image image = teacherWin[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1f, appearSpeed * Time.deltaTime * 0.5f));
                image.transform.localScale = Vector3.MoveTowards(image.transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime * 2.0f);
            }
        }
    }

    //生徒勝利の文字処理
    private void studentWinImage()
    {
        //生徒の最初の4文字表示
        if (winImageLength < 4)
        {
            Image image = studentWin[winImageLength];
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1f, appearSpeed * Time.deltaTime * 2.0f));
            if (image.color.a >= 1f)
            {
                winImageLength++;
            }
        }
        else if (winImageLength >= 4 && !lastImages)
        {
            //文字を2倍になる
            for (int i = 4; i < studentWin.Length; i++)
            {
                Image image = studentWin[i];
                image.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);
            }
            lastImages = true;
        }
        //大成功の文字を縮小する
        if (lastImages)
        {
            //文字を表示しながら縮小
            for (int i = 4; i < studentWin.Length; i++)
            {
                Image image = studentWin[i];
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1f, appearSpeed * Time.deltaTime * 0.5f));
                image.transform.localScale = Vector3.MoveTowards(image.transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime *2.0f);
            }
        }
    }
}
