using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    public Light Light; // 対象のライト
    public float rotationSpeed = 10f; //ライト回転スピード
    public Color duskColor = new Color(1, 0.5f, 0); // 夕暮れの色
    public float duration = 5f; // 色を切り替える時間

    private Color originalColor; // 初期色
    public float changeTime;    //色変わるタイム
    private Vector3 lightRotation; //角度記録

    // Start is called before the first frame update
    void Start()
    {
        if (Light == null) Light = GetComponent<Light>(); // 対象のライトが指定されていない場合、現在のゲームオブジェクトからライトコンポーネントを取得
        Light.color = Color.white;
        originalColor = Light.color; // 初期色を保存
        //ライト座標と角度初期化
        Light.transform.position = new Vector3(0, 3, 0);
        lightRotation = new Vector3(50, 0, 0);
        //今の角度を取得
        lightRotation = Light.transform.eulerAngles;
        changeTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        RotationChange();
        ColorChange();
    }


    //ライト角度変換
    private void RotationChange()
    {
        //ライトのY角度を時間に伴って変換
        lightRotation.y += rotationSpeed * Time.deltaTime;
        //今の角度変更
        Light.transform.eulerAngles = new Vector3(lightRotation.x, lightRotation.y, lightRotation.z);
    }

    private void ColorChange()
    {
        //時間を計算
        changeTime = Mathf.Sin(Time.time / duration * Mathf.PI) * 0.8f - 0.2f;
        //初期色と夕暮れ色の間で変換
        Light.color = Color.Lerp(originalColor, duskColor, changeTime);
    }
}

