using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BroadCast
{
    public class UDPBroadCast
    {
        private static UDPBroadCast instance = null;

        public static UDPBroadCast Instance
        {
            get
            {
                if (instance == null) instance = new UDPBroadCast();
                return UDPBroadCast.instance;
            }
        }

        private Dictionary<string, BroadCastMessenger> serverIpDic;

        public Dictionary<string, BroadCastMessenger> ServerIpDic
        {
            get { return serverIpDic; }
        }

        private Socket sendSock;
        private IPEndPoint sendIep1;
        private byte[] sendData;

        private Socket receiveSock;
        private IPEndPoint receiveIep1;
        private byte[] receiveData;
        private EndPoint receiveEp; 
        private string hostName;
        private string dataBody;
        private Thread reciverThread;
        private UDPBroadCast()
        {
            receiveData = new byte[1024];
            serverIpDic = new Dictionary<string, BroadCastMessenger>();
        }
        public void InitBroadCast(string dataBody = "")
        {
            try
            {
                hostName = Dns.GetHostName();
                sendData = CommonTools.Serialize<BroadCastMessenger>(new BroadCastMessenger(hostName, string.Empty, dataBody));
                sendSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sendIep1 = new IPEndPoint(IPAddress.Broadcast, 9050);
            }
            catch (Exception e)
            {
                Debug.Log("@" + e.Message);
            }
        }
        public void SendBroadCast()
        {
            if (sendData == null) return;
            Debug.Log("send");
            sendSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            sendSock.SendTo(sendData, sendIep1);
        }

        public void CloseBroadCast()
        {
            if (sendSock != null)
            {
                sendSock.Close();
            }

           
        }

        public void StartListenBroadCast()
        {
            Debug.Log("begin");
            serverIpDic.Clear();
            receiveSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            receiveIep1 = new IPEndPoint(IPAddress.Any, 9050);
            receiveSock.Bind(receiveIep1);
            receiveEp = (EndPoint)receiveIep1;
            reciverThread = new Thread(new ThreadStart(ListenBroadCast));
            reciverThread.Start();
        }

        private void ListenBroadCast()
        {
            while (true)
            {

                int recv = receiveSock.ReceiveFrom(receiveData, ref receiveEp);  //同步调用，此处会被阻塞.
                try
                {
                    BroadCastMessenger msg = CommonTools.Deserialize<BroadCastMessenger>(new BroadCastMessenger(), receiveData);
                    string IP = receiveEp.ToString();
                    int index = IP.IndexOf(':');
                    IP = IP.Substring(0, index);
                    msg.IP = IP;
                    Debug.Log("reciver:" + IP + msg.HostName);

                    if (!serverIpDic.ContainsKey(msg.IP))
                    {
                        serverIpDic.Add(msg.IP, msg);
                    }
                }
                catch (Exception e)
                {

                    Debug.LogError("@@" + e.Message);
                }
               
            }
        }

        public void StopListenBroadCast()
        {
            if (receiveSock != null) receiveSock.Close();
            if (reciverThread != null && reciverThread.IsAlive) reciverThread.Abort();
            serverIpDic.Clear();
        }

    }

    [Serializable]
    public class BroadCastMessenger
    {
        public BroadCastMessenger()
        { }

        public BroadCastMessenger(string hostName, string iP, string dataBody = "")
        {
            this.hostName = hostName;
            this.iP = iP;
            this.dataBody = dataBody;
            port = 0;
        }

        public BroadCastMessenger(string hostName, string iP, int port)
        {
            this.hostName = hostName;
            this.iP = iP;
            this.port = port;
        }


        private string hostName;
        /// <summary>
        /// 主机名称 
        /// </summary>
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private string iP;
        /// <summary>
        /// 主机IP地址
        /// </summary>
        public string IP
        {
            get { return iP; }
            set { iP = value; }
        }

        private int port;
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private string dataBody;

        public string DataBody
        {
            get { return dataBody; }
            set { dataBody = value; }
        }
    }

    public class CommonTools
    {
        /// <summary>
        /// 序列化class为byte[]
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T obj) where T : class
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);  //序列化到一个内存流中，不生成物理文件。
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);  //以byte[]形式读取stream中数据。

            stream.Flush();
            stream.Close();

            return buffer;
        }

        /// <summary>
        /// 序列化byte[]为class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newT"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T newT, byte[] buffer) where T : class
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buffer);
            newT = (T)formatter.Deserialize(stream);

            stream.Flush();
            stream.Close();

            return newT;
        }
    }

}

