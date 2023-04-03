using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using Unity.VisualScripting;

public class Sockets : MonoBehaviour
{
    Socket server;
    Socket send;
    Socket receiveSocket;
    IPEndPoint pythonHost;
    IPEndPoint local;

    byte[] data;
    // Start is called before the first frame update
    void Start()
    {
        pythonHost = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4149);
        local = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4150);


        server=new Socket(SocketType.Stream, ProtocolType.Tcp);
        server.Bind(local);
        server.Listen(1000);
        server.ReceiveTimeout = 15000;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PLayerBet()
    {
        send = new Socket(SocketType.Stream, ProtocolType.Tcp);
        send.Connect(pythonHost);
        data = BitConverter.GetBytes(1);
        send.Send(data);

        //should trigger the python code's voice data

        Socket receiveSocket = server.Accept();
        data = new byte[8];
        receiveSocket.Receive(data);
        byte[] var1 = new byte[4];
        byte[] var2 = new byte[4];
        Array.Copy(data, var1, 4);
        Array.Copy(data, 4, var2, 0, 4);
        int num1 = BitConverter.ToInt32(var1);
        int num2 = BitConverter.ToInt32(var2);

        Debug.Log(num1);
        Debug.Log(num2);
        if (num1 <= DiceGame.instance.pCash||num1!=-1)
        {
            DiceGame.instance.pCash -= num1;
            DiceGame.instance.pBet = num1;
            MenuManager.Instance.pCash.text = DiceGame.instance.pCash.ToString();
            MenuManager.Instance.pBet.text = num1.ToString();
        }
        else
        {
            MenuManager.Instance.pBet.text = "error";
            DiceGame.instance.pBet = 0;
        }
        switch (num2)
        {
            case 0:
                MenuManager.Instance.pWincon.text = "Below";
                DiceGame.instance.pWincon = num2;
                break;
                case 1:
                MenuManager.Instance.pWincon.text = "Exactly";
                DiceGame.instance.pWincon = num2;
                break;
                case 2:
                MenuManager.Instance.pWincon.text = "Above";
                DiceGame.instance.pWincon = num2;
                break;
            default:
                DiceGame.instance.pWincon = -1;
                MenuManager.Instance.pWincon.text = "error";
                break;
        }

        send.Close();
    }

    private void OnDestroy()
    {
        server.Close();
        send.Close();
    }
}
