using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;

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
        if (Input.GetKeyDown(KeyCode.Space))
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
            Array.Copy(data, 4, var2,0,4);
            Debug.Log(BitConverter.ToInt32(var1));
            Debug.Log(BitConverter.ToInt32(var2));
            MenuManager.Instance.pBet.text = BitConverter.ToString(var1);
            MenuManager.Instance.pDice.text = BitConverter.ToString(var2);


            send.Close();

        }
    }

    private void OnDestroy()
    {
        server.Close();
        send.Close();
    }
}
