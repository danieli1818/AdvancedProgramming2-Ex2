using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ApplicationMainModel : IMainModel
    {
        #region Singleton
        private static IMainModel m_instance = null;
        public static IMainModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ApplicationMainModel();
                    return m_instance;
                }
                return m_instance;
            }
        }
        private ApplicationMainModel()
        {
            SettingsModel = ApplicationSettingsModel.Instance;
        }
        #endregion

        public TcpListener InfoServerTCPListener { get; set; }
        public TcpClient Client { get; set; }
        private bool ShouldInfoServerRun { get; set; }
        private ISettingsModel SettingsModel { get; set; }
        private Thread InfoServerThread { get; set; }

        public void Connect()
        {
            Stop();
            InfoServerThread = new Thread(ConnectAndStartServer);
            InfoServerThread.Start();
            ConnectClient();
        }

        private void ConnectAndStartServer()
        {
            InfoServerTCPListener = new TcpListener(IPAddress.Any, SettingsModel.FlightInfoPort);
            // we set our IP address as server's address, and we also set the port

            InfoServerTCPListener.Start();  // this will start the server

            ShouldInfoServerRun = true;

            while (ShouldInfoServerRun)   //we wait for a connection
            {
                TcpClient client;
                try
                {
                    client = InfoServerTCPListener.AcceptTcpClient();  //if a connection exists, the server will accept it
                } catch (System.Net.Sockets.SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted)
                    {
                        break;
                    } else
                    {
                        throw e;
                    }
                }
                

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                //byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                //hello = Encoding.Default.GetBytes("hello world");  //conversion string => byte array

                //ns.Write(hello, 0, hello.Length);     //sending the message

                while (client.Connected && ShouldInfoServerRun)  //while the client is connected, we look for incoming messages
                {
                    StreamReader sr = new StreamReader(ns);
                    string line = sr.ReadLine();
                }
            }
        }

        private void ConnectClient()
        {
            while (true)
            {
                try
                {
                    Client = new TcpClient(SettingsModel.FlightServerIP, SettingsModel.FlightCommandPort);
                    break;
                } catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        continue;
                    }
                    throw e;
                }
            }
        }

        public void SendCommand(string command)
        {
            if (Client != null && Client.Connected)
            {
                NetworkStream stream = Client.GetStream();
                StreamWriter sw = new StreamWriter(stream);
                sw.WriteLine(command);
                sw.Flush();
            } // TODO Check What To Do If Not Connected
        }

        public void Stop()
        {
            if (InfoServerThread != null && InfoServerThread.IsAlive)
            {
                ShouldInfoServerRun = false;
                InfoServerTCPListener.Stop();
                //System.Threading.Thread.Sleep(5000);
                if (InfoServerThread.IsAlive)
                {
                    InfoServerThread.Abort();
                }
            }
            if (Client != null && Client.Connected)
            {
                Client.Close();
            }
            
        }
    }
}
