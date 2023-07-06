using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Diagnostics;

namespace Pong
{
    class ClientTcpSocket
    {
        protected IPEndPoint tcpEndPoint;

        public bool isConnect = false;
        protected Socket socketServer;

        public EventHandler eventStart;
        public EventHandler eventConnect;
        public EventHandler eventErrorSend;
        public EventHandler eventErrorReceive;

        public int sendingDelay = 70;
        public int receivingDelay = 30;

        StringBuilder receiveData = new StringBuilder();

        public EventHandler eventErrorConnect;

        public async void Connect(string ipAddress, int port)
        {
            try
            {
                //EndPoint точка подключение
                tcpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                eventStart?.Invoke(this, null);
                isConnect = await Task.Run(() =>
                {
                    try
                    {
                        socketServer.Connect(tcpEndPoint);
                        return true;
                    }
                    catch (SocketException ex) when (ex.ErrorCode == 10061)
                    {
                        return false;
                    }
                });
                if (isConnect)
                {
                    eventConnect?.Invoke(this, null);
                }
                else
                {
                    eventErrorConnect?.Invoke(this, null);
                }
                
            }
            catch (Exception e)
            {
                eventErrorConnect?.Invoke(e, null);
            }
        }

        public string Receive()
        {
            if (socketServer == null)
            {
                eventErrorReceive?.Invoke(this, null);
                return null;
            }
            socketServer.ReceiveBufferSize = 8;
            try
            {
                byte[] buffer = new byte[8];
                int countBytes = 0;
                receiveData.Clear();
                do
                {
                    countBytes = socketServer.Receive(buffer);
                    receiveData.Append(Encoding.UTF8.GetString(buffer, 0, countBytes));
                }
                while (socketServer.Available > 0);
                return receiveData.ToString();
            }
            catch (Exception)
            {
                eventErrorReceive?.Invoke(this, null);
                return null;
            }
        }

        public void Send(string message)
        {
            if (socketServer == null)
            {
                eventErrorReceive?.Invoke(this, null);
                return;
            }
            try
            {
                socketServer.Send(Encoding.UTF8.GetBytes(message));
            }
            catch (Exception)
            {
                eventErrorSend?.Invoke(this, null);
            }
        }

        public void Disconnect()
        {
            // Нельзя прописывать socketListner.Shutdown(), так-как сервер не понимает, что клиент отключился
            socketServer?.Close();
            isConnect = false;
        }
    }
}
