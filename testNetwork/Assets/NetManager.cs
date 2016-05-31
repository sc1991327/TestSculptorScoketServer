using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

public class NetManager : MonoBehaviour {

    public string myIP = "10.32.93.177";
    public int myProt = 8885;

    private Socket clientSocket;
    private Thread thread;
    private byte[] data = new byte[1024];  
    private string message = "";

    // Use this for initialization
    void Start () {
        ConnectToServer();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string value = "Hello World";
            SendMessage(value);
        }
    }

    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(myIP), myProt));
        thread = new Thread(ReceiveMessage);
        thread.Start();
    }

    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
            {
                break;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
            print(message);
        }
    }

    void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
