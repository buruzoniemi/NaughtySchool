using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeUIController : MonoBehaviour, IUI_Interface
{
    [SerializeField] private GameObject[] UIObject;

    [SerializeField] private Canvas canvas;

    [SerializeField] private float fadeAlpha;
    [SerializeField] private bool[] isFadeOut;

    public delegate void CompUIEnd();
    private CompUIEnd compUIEndCallBack;

    void Start()
    {
        isFadeOut = new bool [UIObject.Length];
        for (int i = 0; i < UIObject.Length; ++i) { isFadeOut[i] = false; }
        DisableCanvas();
    }

    public void initCompUI(CompUIEnd onCompUI)
	{
        compUIEndCallBack = onCompUI;
	}

    void Update()
    {
        for(int i = 0; i < UIObject.Length; ++i) FadeOutUI(UIObject[i], i);
    }

    /// <summary>
    /// UIを有効化する
    /// </summary>
    /// <param name="isSuccsess">QTEの入力が成功したか</param>
    public void EnableUI(bool isSuccsess)
    {
        if (isSuccsess)
        {
            UIObject[0].SetActive(true);
            Color color = UIObject[0].GetComponent<Image>().color;
            color.a = 1.0f;
            UIObject[0].GetComponent<Image>().color = color;
            UIObject[1].SetActive(false);
        }
        else
        {
            UIObject[1].SetActive(true);
            Color color = UIObject[1].GetComponent<Image>().color;
            color.a = 1.0f;
            UIObject[1].GetComponent<Image>().color = color;

            UIObject[0].SetActive(false);
        }

    }

    public void CompUI()
	{
        UIObject[2].SetActive(true);
        Color color = UIObject[2].GetComponent<Image>().color;
        color.a = 1.0f;

        for(int i = 0; i < UIObject.Length -1; ++i)
		{
            UIObject[i].SetActive(false);
		}

        SetIsFadeOut(2);
	}


    public void EnableCanvas()
    {
        canvas.enabled = true;

        for(int i = 0; i < UIObject.Length; ++i)
        {
            UIObject[i].SetActive(false);
        }
    }

    public void DisableCanvas() { canvas.enabled = false; }

    public void FadeOutUI(GameObject UI, int index) 
    {
        if (!isFadeOut[index]) return;
        Color color = UI.GetComponent<Image>().color;
        if(color.a >= 0.0f)
        {
            color.a -= fadeAlpha;
            UI.GetComponent<Image>().color = color;
        }
        else
        {
            UI.SetActive(false);
            isFadeOut[index] = false;
            if(index == 2)
            {
                compUIEndCallBack();
            }
        }
    }

    public void SetIsFadeOut(int index) { isFadeOut[index] = true; }

}
