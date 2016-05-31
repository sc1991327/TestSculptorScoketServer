using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetManagerUDP : MonoBehaviour {

    public static string myIP = "127.0.0.1";
    public static string targetIP = "127.0.0.1";
    public static int sendPort = 8885;
    public static int recvPort = 8886;

    private static Socket clientUDP;

	// Use this for initialization
	void Start () {

        clientUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        clientUDP.Bind(new IPEndPoint(IPAddress.Parse(myIP), recvPort));
        Thread t = new Thread(ReciveMsg);
        t.Start();
        Debug.Log("Client Start...");
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Jump"))
        {
            sendMsg("Client Say: Hello!");
        }

	}

    static void sendMsg(string message)
    {
        EndPoint point = new IPEndPoint(IPAddress.Parse(targetIP), sendPort);
        clientUDP.SendTo(Encoding.UTF8.GetBytes(message), point);
    }

    static void ReciveMsg()
    {
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[1024];
            int length = clientUDP.ReceiveFrom(buffer, ref point);
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            Debug.Log(point.ToString() + message);
        }
    }
}
