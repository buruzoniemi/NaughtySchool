using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
	//変数宣言
	[SerializeField] private GameObject[] itemPrefab;   //表示したい小物のPrefabリスト
	private int ItemCount = 8;          //小物の数
	private GameObject[] itemObj = new GameObject[8];		//生成するオブジェクトを格納する
	private int ItemNum = -1;			//生成するアイテムの数

	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        
    }
	/// <summary>
	/// アイテムを生成する関数
	/// </summary>
	public void ItemInstantiate()
	{
		//アイテムのカウント数分繰り返す
		for (int i = 0; i < ItemCount; ++i)
		{
			//Debug.Log("itemObj[" + $"{i}" + "]の中身:" + $"{itemObj[i]}");
			//アイテムオブジェにインスタンス化したオブジェクトを格納
			itemObj[i] = Instantiate(itemPrefab[i],this.gameObject.transform);
			//非表示にする
			itemObj[i].SetActive(false);
			itemObj[i].transform.localPosition = new Vector3(0, 0, 0);
		}
	}
	/// <summary>
	/// いたずらの番号を取得して表示するアイテムの対象を切り替える関数
	/// </summary>
	/// <param name="naughtyNum"></param>
	public void getNaughtyIndex(int naughtyNum)
	{
		//アイテムの番号を決める
		//いたずら番号が2以下のときは０を
		//他のときはいたずら番号から２引いた番号に設定する
		ItemNum = (naughtyNum <= 2) ? 0 : naughtyNum - 2;
	}
	/// <summary>
	/// 対象のアイテムを表示する
	/// </summary>
	public void ItemActivetrue()
	{
		itemObj[ItemNum].SetActive(true);
		itemObj[ItemNum].GetComponent<Animator>().SetBool("ItemUse", true);

	}
	/// <summary>
	/// 対象のアイテムを非表示にする
	/// </summary>
	public void ItemActivefalse()
	{
		itemObj[ItemNum].SetActive(false);
		itemObj[ItemNum].GetComponent<Animator>().SetBool("ItemUse", false);
	}
}
