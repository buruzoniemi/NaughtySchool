using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Text))]

public class RotateText : UIBehaviour, IMeshModifier
{
    private Text textComponent;
    private char[] characters;

    // 回転させない文字群
    // XXX 別の設定ファイルなりcsvにまとめて最初に読み込んでしまうのが良さそう
    private List<char> nonrotatableCharacters;

    /// <summary>
    /// スクリプトがアクティブになったときに実行される初期化処理
    /// </summary>
    protected override void Start()
    {
        //base.OnValidate();
        textComponent = this.GetComponent<Text>();

        // nonrotatableCharacters リストを初期化する
        nonrotatableCharacters = new List<char>();

        var graphics = base.GetComponent<Graphic>();
        if (graphics != null)
        {
            graphics.SetVerticesDirty();
        }
    }

    /// <summary>
    /// メッシュの変更を行うインターフェース
    /// </summary>
    /// <param name="mesh"></param>
    public void ModifyMesh(Mesh mesh) { }

    /// <summary>
    /// メッシュの変更を行うインターフェース
    /// </summary>
    /// <param name="verts"></param>
    public void ModifyMesh(VertexHelper verts)
    {
        if (!this.IsActive())
        {
            return;
        }

        List<UIVertex> vertexList = new List<UIVertex>();
        verts.GetUIVertexStream(vertexList);

        ModifyVertices(vertexList);

        verts.Clear();
        verts.AddUIVertexTriangleStream(vertexList);
    }

    /// <summary>
    /// 頂点の変更を行うメソッド
    /// </summary>
    /// <param name="vertexList"></param>
    private void ModifyVertices(List<UIVertex> vertexList)
    {
        characters = textComponent.text.ToCharArray();
        if (characters.Length == 0)
        {
            return;
        }

        for (int i = 0, vertexListCount = vertexList.Count; i < vertexListCount; i += 6)
        {
            int index = i / 6;
            if (IsNonrotatableCharactor(characters[index]))
            {
                continue;
            }

            var center = Vector2.Lerp(vertexList[i].position, vertexList[i + 3].position, 0.5f);
            for (int r = 0; r < 6; r++)
            {
                var element = vertexList[i + r];
                var pos = element.position - (Vector3)center;
                var newPos = new Vector2(
                    pos.x * Mathf.Cos(90 * Mathf.Deg2Rad) - pos.y * Mathf.Sin(90 * Mathf.Deg2Rad),
                    pos.x * Mathf.Sin(90 * Mathf.Deg2Rad) + pos.y * Mathf.Cos(90 * Mathf.Deg2Rad)
                );

                element.position = (Vector3)(newPos + center);
                vertexList[i + r] = element;
            }
        }
    }

    /// <summary>
    /// 回転させない文字かどうかを判定するメソッド
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    private bool IsNonrotatableCharactor(char character)
    {
        return nonrotatableCharacters.Any(x => x == character);
    }
}