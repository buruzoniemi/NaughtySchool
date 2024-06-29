using UnityEngine;
using UnityEngine.UI;

public class Teacher_Bar_UI : MonoBehaviour
{
    // 変数宣言------------------------------------------
    private static readonly float maxTeacherGage = 100.0f; //ゲージの最大値
    private static readonly float minTeacherGage = 0.0f; //ゲージの最小値
    private float nowTeacherGage; //現在のゲージの値

    [SerializeField] private Slider slider; //スライダーを格納する変数
    //--------------------------------------------------

    /// <summary>
    /// ゲーム開始時のいろいろな処理
    /// </summary>
    void Start()
    {
        //各変数の初期化
        //現在のゲージの値 = ゲージの最小値
        nowTeacherGage = minTeacherGage;
        //スライダーのvalue値を初期する
        slider.value = nowTeacherGage;
        //ゲージの最大値、最小値の範囲の固定
        Mathf.Clamp(nowTeacherGage, minTeacherGage, maxTeacherGage);
    }

    /// <summary>
    /// 現在の授業ゲージの値を引数分足していき、その値をSliderに反映していく処理
    /// </summary>
    /// <param name="sliderGage">ゲージの上昇値</param>
    public void TeacherGageUp(float sliderGage)
    {
        //現在の授業ゲージの値を引数分足していく
        nowTeacherGage += sliderGage;
        //そのゲージを最大値で割り、割合に直し、それをスライダーのvalueに代入する
        slider.value = nowTeacherGage / maxTeacherGage;
    }

    /// <summary>
    /// 現在のsliderの値がmaxの値を超えた時勝利判定を送るメソッド
    /// </summary>
    /// <returns>勝利判定</returns>
    public  bool WinCheck()
    {
        if(slider.value > maxTeacherGage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
