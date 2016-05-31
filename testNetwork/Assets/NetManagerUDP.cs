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
    public static string serverIP = "10.32.93.177";
    public static int sendPort = 8885;
    public static int recvPort = 8886;

    private static Socket clientUDP;

	// Use this for initialization
	void Start () {

        myIP = GetLocalIPAddress();

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

    void OnDisable()
    {
        clientUDP.Close();
    }

    static void sendMsg(string message)
    {
        EndPoint point = new IPEndPoint(IPAddress.Parse(serverIP), sendPort);
        clientUDP.SendTo(Encoding.UTF8.GetBytes(message), point);
        Debug.Log("Send: " + point.ToString() + " " + message);
    }

    static void ReciveMsg()
    {
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[1024];
            int length = clientUDP.ReceiveFrom(buffer, ref point);
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            Debug.Log("Recv: " + point.ToString() + " " + message);
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("Local IP Address Not Found!");
    }
}
