using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using CoreEngine.Network;

public delegate void OnMessageCallBack(string msg);
public class SocketComponent : MonoBehaviour {
    
    public String host = "localhost";
    public Int32 port = 50000;
    public Boolean autoConnect = true;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket = null;
    TcpClientWithTimeout tcp_socketTimeout = null;
    NetworkStream net_stream;

    BinaryWriter socket_writer;
    BinaryReader socket_reader;

    internal int waitingCount = 0;
    private MemoryStream _Readbuffer = new MemoryStream(65536);

    private OnMessageCallBack onMessage = null;
    private OnMessageCallBack onOpen = null;
    private OnMessageCallBack onClose = null;

    private bool isConnecting = false;

    private List<string> msgForInvoking = new List<string>();

    void Update()
    {
        if (tcp_socket == null)
            return;

        if(isConnecting)
        {
            isConnecting = false;

            waitingCount = 0;
            _Readbuffer.Position = 0;
        }

        try
        {
            readSocket();
        }
        catch(Exception e)
        {
            Debug.Log("Socket error: " + e);
            isConnecting = true;
        }

        if(msgForInvoking.Count > 0)
        {
            if(onMessage != null)
            {
                foreach (var str in msgForInvoking)
                {
                    onMessage(str);
                }
            }
            
            msgForInvoking.Clear();
        }
    }


    void Awake()
    {
        tcp_socketTimeout = new TcpClientWithTimeout(host, port, 2000);

        if (autoConnect)
        {
            connect();
        }
            
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    public bool IsConnected()
    {
        return tcp_socket != null && tcp_socket.Connected;
    }

    public bool IsConnecting
    {
        set { }
        get { return isConnecting; }
    }
    
    public OnMessageCallBack OnMessage
    {
        get { return onMessage;  }
        set { onMessage = value;  }
    }

    public OnMessageCallBack OnOpen
    {
        get { return onOpen; }
        set { onOpen = value; }
    }


    public OnMessageCallBack OnClose
    {
        get { return onClose; }
        set { onClose = value; }
    }

    public void setupSocket()
    {
        if (tcp_socket == null)
            return;

        try
        {
            net_stream = tcp_socket.GetStream();
            socket_writer = new BinaryWriter(net_stream);
            socket_reader = new BinaryReader(net_stream);
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    public void connect()
    {
        if (isConnecting)
            return;
        try
        {
            isConnecting = true;
            tcp_socket = tcp_socketTimeout.Connect();

            isConnecting = false;
            setupSocket();
        }
        catch (Exception ex)
        {
            isConnecting = false;
            Debug.Log(ex.ToString());
        }
    }

    public void Send(string msg)
    {
        if (!IsConnected())
            return;

        msg = msg.Trim();
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(msg);
        UInt32 size = (UInt32)byteArray.Length;
        socket_writer.Write(size);
        socket_writer.Write(byteArray);
        socket_writer.Flush();
    }

    public void readSocket()
    {
        if (!IsConnected())
            return;

        if (net_stream.DataAvailable)
        {
            if(waitingCount > 0)
            {
                Byte[] arr = socket_reader.ReadBytes((int)waitingCount);
                if(arr.Length < waitingCount)
                {
                    waitingCount = waitingCount - arr.Length;
                    _Readbuffer.Write(arr, 0, arr.Length);
                }
                else
                {
                    if (onMessage != null)
                    {
                        int len = (int)_Readbuffer.Position;
                        msgForInvoking.Add(System.Text.Encoding.UTF8.GetString(_Readbuffer.GetBuffer(), 0, len));

                        _Readbuffer.Position = 0;
                        waitingCount = 0;
                    }
                    readSocket();
                }
            }
            else
            {
                UInt32 size = socket_reader.ReadUInt32(); 
                if(size > 0)
                {
                    Byte[] arr = socket_reader.ReadBytes((int)size);
                    if (arr.Length < size)
                    {
                        waitingCount = (int)(size - arr.Length);
                        _Readbuffer.Write(arr, 0, arr.Length);
                    }
                    else
                    {

                        msgForInvoking.Add(System.Text.Encoding.UTF8.GetString(arr));

                        readSocket();
                    }

                }
                
            }
            
        }

        return ;
    }

    public void closeSocket()
    {
        if (tcp_socket == null)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }
}
