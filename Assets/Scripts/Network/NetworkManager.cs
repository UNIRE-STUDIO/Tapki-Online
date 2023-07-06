using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UnityEngine;


namespace Pong
{
    class NetworkManager : Singleton<NetworkManager>
    {
        private ClientTcpSocket clientTcpSocket;
        //private ClientUdpSocket clientUdpSocket;

        private NetworkData networkSendDataTcp = new NetworkData();
        private NetworkData networkSendDataUdp = new NetworkData();

        private NetworkData networkReceiveDataTcp = new NetworkData();
        private NetworkData networkReceiveDataUdp = new NetworkData();

        [HideInInspector] public bool isConnect = false;

        private int leftSideScore = 0;
        private int rightSideScore = 0;

        public void Connect(string serverIp, int serverPort)
        {
            clientTcpSocket = new ClientTcpSocket();
            //clientUdpSocket = new ClientUdpSocket();
            clientTcpSocket.eventStart += (handle, ee) =>
            {
                //mainWindow.clentStatus.Content = "Подключение...";
            };
            clientTcpSocket.eventConnect += async (handle, ee) =>
            {
                //mainWindow.clentStatus.Content = "Клиент: Подключение установлено";
                await Task.Run(() => {
                    //SendUdp(clientTcpSocket);
                    //ReceiveUdp(clientTcpSocket);
                    SendTcp(clientTcpSocket);
                    ReceiveTcp(clientTcpSocket);
                });
            };
            clientTcpSocket.eventErrorConnect += (ee, args) =>
            {
                // Вывести через делегаты
                //MessageBox.Show("Клиент: Не удалось подключиться... " + ((Exception)ee).Message);

                /*
                mainWindow.Dispatcher.Invoke((Action)delegate
                {
                    //mainWindow.DisconnectClient_Click();
                });
                */
            };
            /*
            clientUdpSocket.eventErrorReceive += (hendler, ee) => { // Клиент разорвал соединение или что-то ещё
                mainWindow.Dispatcher.Invoke((Action)delegate
                {
                    mainWindow.DisconnectClient_Click();
                });
                MessageBox.Show("Клиент UDP: Не удалось получить данные");
            };
            */
            clientTcpSocket.eventErrorReceive += (hendler, ee) => {
                /*
                mainWindow.Dispatcher.Invoke((Action)delegate
                {
                    mainWindow.DisconnectClient_Click();
                });
                MessageBox.Show("Клиент TCP: Не удалось получить данные");
                */
            };


            //clientUdpSocket.Connect(serverIp, serverPort);
            clientTcpSocket.Connect(serverIp, serverPort);
            isConnect = true;
        }

        public async void ReceiveTcp(ClientTcpSocket tcpSocket)
        {
            while (tcpSocket.isConnect)
            {
                // Получаем данные
                networkReceiveDataTcp.dataDictionary.Clear();
                string ms = tcpSocket.Receive();
                if (ms == null || ms == "") return;
                networkReceiveDataTcp.Unpacking(ms);

                // Присваиваем данные
                

                await Task.Delay(10);
            }
        }
        /*
        public async void ReceiveUdp(UnTcpSocket tcpSocket, IUdpSocket udpSocket)
        {
            while (tcpSocket.isConnect)
            {
                networkReceiveDataUdp.dataDictionary.Clear();

                string ms = udpSocket.Receive();
                if (ms == null || ms == "") return;
                networkReceiveDataUdp.Unpacking(ms);

                if (networkReceiveDataUdp.dataDictionary.TryGetValue((char)Keys.paddlePosY, out string y))
                {
                    paddleOpponent.newPositon = new Vector2(paddleOpponent.position.x, int.Parse(y));
                }

                for (int i = 0; i < balls.Count; i++)
                {
                    if (networkReceiveDataUdp.dataDictionary.TryGetValue((char)(i - 1), out string vec))
                    {
                        Vector2 pos = new Vector2();
                        pos.SetString(vec);
                        balls[i].position = pos;
                        balls[i].newPositon = pos;
                        balls[i].visible = true;
                        activeBalls.Add(balls[i]);
                    }
                    else
                    {
                        balls[i].visible = false;
                        activeBalls.Remove(balls[i]);
                    }
                }
                await Task.Delay(10);
            }
        }
        */

        public async void SendTcp(ClientTcpSocket tcpSocket)
        {
            while (tcpSocket.isConnect)
            {
                tcpSocket.Send(networkSendDataTcp.ToString());

                networkSendDataTcp.dataDictionary.Clear();
                await Task.Delay(200);
            }
        }

        /*
        public async void SendUdp(UnTcpSocket tcpSocket, IUdpSocket udpSocket)
        {
            while (tcpSocket.isConnect)
            {
                networkSendDataUdp.dataDictionary.Add((char)Keys.paddlePosY, ((int)paddleLocal.position.y).ToString());

                udpSocket.Send(networkSendDataUdp.ToString());

                networkSendDataUdp.dataDictionary.Clear();
                await Task.Delay(20);
            }
        }
        */

        public void Disconnect()
        {
            isConnect = false;
            clientTcpSocket.Disconnect();
            //clientUdpSocket.Disconnect();
        }
    }
}
