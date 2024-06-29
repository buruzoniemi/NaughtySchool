using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReceiveFragSceneChange : MonoBehaviour
{
    /// <summary>
    /// Sceneを変更するメソッド
    /// </summary>
    /// <param name="SceneName"> 変更先のSceneの名前を記述する </param>
    public static IEnumerator SceneChange(float delayTime, string sceneName)
    {
        Debug.Log("シーンを切り替える");
        //何秒間か処理を遅延させる
        yield return new WaitForSeconds(delayTime);
        //引数の名前のシーンのロード
        SceneManager.LoadScene(Scene_EnumManager.GetSceneName(Name.ModeSelect));
    }
}
