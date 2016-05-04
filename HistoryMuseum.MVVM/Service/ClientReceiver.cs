using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace HistoryMuseum.MVVM.Service
{

    public class StateObject
    {
        public TcpClient client = null;
        public const int BufferSize = 1024;

        public byte[] buffer = new byte[BufferSize];
    }


    public class ClientReceiver
    {
        public event EventHandler OnConfigUpdated;
        int _port = 10003;
        TcpListener _listener;
        TcpClient _connector;
        NetworkStream _dataStream;
        string _menuPath = AppDomain.CurrentDomain.BaseDirectory + "Menu//Menu.xml";
        int _blockLength = 1024;

        public void Start()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, _port);

            _listener = new TcpListener(ip);
            _listener.Start();
            _listener.BeginAcceptTcpClient(AcceptCallback, _listener);
        }

        public void Stop()
        {
            _listener.Stop();
            if (_connector != null)
            {
                if (_connector.Connected) _connector.Close();
                _connector = null;
            }
        }


        private void AcceptCallback(IAsyncResult iar)
        {
            try
            {
                _listener = (TcpListener)iar.AsyncState;
                _connector = _listener.EndAcceptTcpClient(iar);
                _dataStream = _connector.GetStream();

                StateObject receiveData = new StateObject();
                receiveData.client = _connector;

                _dataStream.BeginRead(receiveData.buffer, 0, receiveData.buffer.Length, AcceptData, receiveData);
                _listener.BeginAcceptTcpClient(AcceptCallback, _listener);  
            }
            catch (Exception ex)
            {
                SFLib.Logger.Exception(ex.Message);
            }
        }


        private void AcceptData(IAsyncResult ar)
        {
            StateObject receiveData = (StateObject)ar.AsyncState;
            TcpClient client = receiveData.client;

            if (client.Connected)
            {
                int numberOfReadBytes = 0;
                try
                {
                    numberOfReadBytes = client.Client.EndReceive(ar);
                }
                catch
                {
                    numberOfReadBytes = 0;
                }
            }

        }
    }
}
