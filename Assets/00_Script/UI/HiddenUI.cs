using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIを削除する処理
/// </summary>
public class HiddenUI : MonoBehaviour
{
    [SerializeField] private Behaviour hiddenTarget; //非表示にされる対象
    [SerializeField] private PlayerSpawnInGameStop playerSpawnInGameStop; //Playerが生成終わるまでゲームを止める処理のとこ
    [SerializeField] private int MyNumber; //自身が何番目かを判断する変数

    private int currentInPlayerCount; //現在のPlayer参加数
    private int maxInPlayerCount; //Playerの最大参加数
	private bool isHiddentTargetSetActive;


    // Start is called before the first frame update
    void Start()
    {
		isHiddentTargetSetActive = true;
		maxInPlayerCount = StageSelectManager.studentPlayerQuantites;

        hiddenTarget.gameObject.SetActive(isHiddentTargetSetActive);

        //Debug.Log($"{maxInPlayerCount}");

        //もし生徒のPlayer数と自身の番号が同じなら自身を消す
        if (MyNumber > maxInPlayerCount)
        {
			isHiddentTargetSetActive = false;
			hiddenTarget.gameObject.SetActive(isHiddentTargetSetActive);
        }
    }

    private void Update()
    {
        HiddenTargetUI();
    }

    public void HiddenTargetUI()
    {
        //現在の生徒のPlayer数を取得
        currentInPlayerCount = playerSpawnInGameStop.SendCurrentPlayerCount();

        //もし生徒のPlayer数と自身の番号が同じなら自身を消す
        if(MyNumber == currentInPlayerCount)
        {
			isHiddentTargetSetActive = false;

			hiddenTarget.gameObject.SetActive(isHiddentTargetSetActive);
        }
    }
}
