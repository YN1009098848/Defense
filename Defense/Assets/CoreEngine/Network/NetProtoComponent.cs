////#define DEBUG_SERVER

//using UnityEngine;
//using System.Collections;
//using System;
//using System.IO;
//using System.Net.Sockets;
//using System.Collections.Generic;
//using CoreEngine.Network;
//using System.Threading;

//public class NetProtoComponent : MonoBehaviour
//{
//    public String host = "localhost";
//    public Int32 port = 50000;
//    public Boolean autoConnect = true;

//    internal Boolean socket_ready = false;

//    private bool isConnecting = false;

//    void Update()
//    {
//		if(NetCore.connected)
//		{
//			NetCore.Dispatch();
//        }
//    }

//    void SendHeartBeat()
//    {
//        if(NetCore.connected)
//        {
//            //NetSender.Send<Protocol.HeartBeat>(null,(_rephb) => { });
//        }
//        else
//        {
//            CancelInvoke("SendHeartBeat");
//        }
//    }

//    void Awake()
//    {
//		NetCore.Init();
//        if (autoConnect)
//        {
//            connect();
//			NetSender.Init();
//			NetReceiver.Init();
//        }

//#if DEBUG_SERVER
//        host = "10.0.16.106";
//#endif
//    }

//    void OnApplicationQuit()
//    {
//        closeSocket();
//    }

//    public bool IsConnected()
//    {
//        return NetCore.connected;
//    }

//    public bool IsConnecting
//    {
//        set { }
//        get { return isConnecting; }
//    }

//    public void connect()
//    {
//        if (isConnecting)
//            return;
//        try
//        {
//            isConnecting = true;
//            NetCore.Connect(host, port, () =>
//            {
//                Debug.Log("Connect Success!");

//				NetCore.enabled = true;
//                InvokeRepeating("SendHeartBeat", 5.0f, 5.0f);
//            });

//            isConnecting = false;
//        }
//        catch (Exception ex)
//        {
//            isConnecting = false;
//            Debug.Log(ex.ToString());
//        }
//    }
//    public void closeSocket()
//    {
//        NetCore.Disconnect();
//    }
//}
