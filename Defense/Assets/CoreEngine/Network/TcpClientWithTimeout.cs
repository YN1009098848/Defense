﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace CoreEngine.Network
{
    /// <summary>
    /// TcpClientWithTimeout 用来设置一个带连接超时功能的类
    /// 使用者可以设置毫秒级的等待超时时间 (1000=1second)
    /// 例如:
    /// TcpClient connection = new TcpClientWithTimeout('127.0.0.1',80,1000).Connect();
    /// </summary>
    public class TcpClientWithTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected TcpClient connection;
        protected bool connected;
        protected Exception exception;

        public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }
        public TcpClient Connect()
        {
			Debug.Log("StartConnect");
            // kick off the thread that tries to connect
            connected = false;
            exception = null;
            Thread thread = new Thread(new ThreadStart(BeginConnect));
            thread.IsBackground = true; // 作为后台线程处理
                                        // 不会占用机器太长的时间
            thread.Start();

            // 等待如下的时间
            thread.Join(_timeout_milliseconds);

            if (connected == true)
            {
				Debug.Log("connect success");
                // 如果成功就返回TcpClient对象
                //thread.Abort();
                return connection;
            }
            if (exception != null)
            {
				Debug.Log("connect faild");
                // 如果失败就抛出错误
                //thread.Abort();
                throw exception;
            }
            else
            {
				Debug.Log("connect timeout");
                // 同样地抛出错误
                //thread.Abort();
                string message = string.Format("TcpClient connection to {0}:{1} timed out",
                  _hostname, _port);
                throw new TimeoutException(message);
            }
        }
        protected void BeginConnect()
        {
            try
            {
				Debug.Log("StartConnect in the subthread");
                connection = new TcpClient(_hostname, _port);
                // 标记成功，返回调用者
                connected = true;
            }
            catch (Exception ex)
            {
				Debug.Log("exception in the subthread");
                // 标记失败
                exception = ex;
            }
        }
    }
}
