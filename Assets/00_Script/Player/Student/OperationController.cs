/*
 * 生徒のUI表示をするオブジェクト
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyValue
{
    buttonSouth = 0,
    buttonEast ,
    buttonWest,
    buttonNorth,
    up,
    down,
    left,
    right,
    Length
};

public class OperationController : MonoBehaviour,IUI_Interface
{
    [Header("操作説明用のPrefabをここに入れる")]
    [SerializeField] private GameObject[] UIObject;

    [SerializeField] private Canvas canvas;


    void Start()
    {
        DisableCanvas();
    }


    public void EnableUI(KeyValue key)
    {
        for(int i = 0; i < UIObject.Length; ++i)
        {
            if(i == (int)key) { UIObject[i].SetActive(true); }
            else { UIObject[i].SetActive(false); }
        }
    }

    public void EnableCanvas() { canvas.enabled = true; }

    public void DisableCanvas() { canvas.enabled = false; }

    public void FadeOutUI(GameObject UI, int index) { }
}
