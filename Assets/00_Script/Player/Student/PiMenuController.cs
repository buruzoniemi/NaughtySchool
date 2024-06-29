/*
 * 
 * 回転するパイメニューの実装
 * 
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiMenuController : MonoBehaviour, IUI_Interface
{
    [SerializeField] private List<GameObject> Pi = new();
	[SerializeField] private List<Image> img = new();
	[SerializeField] private List<Sprite> spr = new();

    [SerializeField] private RectTransform cursor;

    [SerializeField] private Canvas canvas;

    [Header("クリア時のアルファ値")]
    [SerializeField] private float compAlpha;

    int selectIndex;

    float angrePerSlot = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
		SetSprite();
        DisableCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.enabled)
        {
            Vector3 Angle = new(0 , 90 , angrePerSlot * selectIndex);
            cursor.eulerAngles = Angle;
            CompMischief();
        }
    }

    // 選択しているスロットを取得する処理
    public int GetSelectedSlot(float stickDagree)
    {
        float currentIndex = stickDagree / angrePerSlot;
        currentIndex = (float)Math.Round(currentIndex, MidpointRounding.AwayFromZero);
        if (currentIndex >= Pi.Count)
        {
            currentIndex = 0;
        }

        selectIndex = (int)currentIndex;
        //Debug.Log($"selectIndex = {selectIndex}");
        return selectIndex;
    }

    private void CompMischief()
    {
        for(int i = 0; i < Pi.Count; ++i)
        {
            if(MischiefManager.instance.CheckTaskComplete(i))
            {
                Color color = Pi[i].GetComponent<Image>().color;
                color.a = compAlpha;
                Pi[i].GetComponent<Image>().color = color;
				img[i].color = color;
            }
        }
    }

	public void SetSprite()
	{
		for(int si = 0; si < img.Count; si++)
		{
			img[si].sprite = spr[MischiefManager.instance.GetMischifAnimNum(si)];
		}
	}

    public void EnableCanvas() { canvas.enabled = true; }
    public void DisableCanvas() { canvas.enabled = false; }
    public void FadeOutUI(GameObject UI, int index) { }
}
