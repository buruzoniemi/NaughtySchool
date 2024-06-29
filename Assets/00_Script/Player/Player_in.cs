using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_in : MonoBehaviour
{
    //変数宣言------------------------------------
    private InGame_Spawner playerSpawn;
    private PlayerSpawnInGameStop playerSpawnInGameStop;

    private const int arrayMinRows = 0; //二次元配列の行の最小要素数
    private const int arrayMinCols = 0; //二次元配列の列の最小要素数
    private const int arrayMaxRows = 3; //二次元配列の行の最大要素数
    private const int arrayMaxCols = 5; //二次元配列の列の最大要素数

    private int arrayRowsRand = 0;  //二次元配列の行のランダムの数
    private int arrayColsRand = 0;  //二次元配列の行のランダムの数
    //-------------------------------------------

    /// <summary>
    /// プレイヤー入室時に受け取る通知
    /// プレイヤーがingameに参加したらそのプレイヤーの番号を識別するもの
    /// </summary>
    /// <param name="m_playerInput"></param>
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        print($"プレイヤー#{playerInput.user.index}が入室！");

        GameObject playerObj = playerInput.gameObject;

        //Debug.Log($"{playerInput.gameObject}");
        playerSpawn = GameObject.FindWithTag("SpawnManager").GetComponent<InGame_Spawner>();
        playerSpawnInGameStop = GameObject.FindWithTag("SpawnManager").GetComponent<PlayerSpawnInGameStop>();

        //Playerが参加したら呼び出すスクリプト
        arrayRowsRand = (int)Random.Range(arrayMinRows, arrayMaxRows);
        arrayColsRand = (int)Random.Range(arrayMinCols, arrayMaxCols);
        //SpawnMangerが取得できてるかどうか
        if(playerSpawn != null)
        {
            //PlayerInputManagerによってオブジェクトが生成されたかどうか
            if(playerObj.tag == "Student")
            {               
                playerSpawnInGameStop.PlayerJoined();
                playerSpawn.StudentObjectsChange(arrayRowsRand, arrayColsRand, playerObj);
            }
            else
            {
                Debug.Log("そのオブジェクトは配列に格納されてないよ！");
            }
        }
        else
        {
            Debug.LogError("playerSpawnが空です。代入処理またはインスペクタ―上で値を設定してください");
        }
    }

    /// <summary>
    /// プレイヤー退室時に受け取る通知
    /// プレイヤーがingameから出たらそのプレイヤーの番号を識別するもの
    /// </summary>
    /// <param name="m_playerInput"></param>
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"プレイヤー#{playerInput.user.index}が退室！");
    }
}
