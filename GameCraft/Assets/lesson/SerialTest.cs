using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialTest : MonoBehaviour
{
    //先ほど作成したクラス
    public SerialHandler serialHandler;


  void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            serialHandler.Write("hello");
        }
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        Debug.Log(message);
    }
}
