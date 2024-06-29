using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インターフェースを作ってUIを管理してみる
/// </summary>
public interface IUI_Interface
{
    public void EnableCanvas();

    public void DisableCanvas();

    public void FadeOutUI(GameObject UI, int index);
}