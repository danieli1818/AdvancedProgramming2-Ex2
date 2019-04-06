using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread InfoServer;
        private bool shouldInfoServerRun;
        private TcpListener InfoServerTCPListener;
        public MainWindow()
        {
            InitializeComponent();
            InfoServer = null;
            shouldInfoServerRun = false;
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.Show();
        }

        private void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            if (InfoServer != null && InfoServer.IsAlive)
            {
                shouldInfoServerRun = false;
                InfoServerTCPListener.Stop();
                InfoServer.Abort();
                InfoServer.Join();
            }
            shouldInfoServerRun = true;
            InfoServer = new Thread(new ThreadStart(createServerAndStartIt));
            InfoServer.Start();

        }

        private void createServerAndStartIt()
        {
            InfoServerTCPListener = new TcpListener(IPAddress.Any, 5400);
            // we set our IP address as server's address, and we also set the port: 5400

            InfoServerTCPListener.Start();  // this will start the server

            while (shouldInfoServerRun)   //we wait for a connection
            {
                TcpClient client = InfoServerTCPListener.AcceptTcpClient();  //if a connection exists, the server will accept it

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                //byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                //hello = Encoding.Default.GetBytes("hello world");  //conversion string => byte array

                //ns.Write(hello, 0, hello.Length);     //sending the message

                while (client.Connected && shouldInfoServerRun)  //while the client is connected, we look for incoming messages
                {
                    StreamReader sr = new StreamReader(ns);
                    string line = sr.ReadLine();
                }
            }
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (InfoServer != null && InfoServer.IsAlive && InfoServerTCPListener != null)
            {
                shouldInfoServerRun = false;
                InfoServerTCPListener.Stop();
                InfoServer.Abort();
            }
        }
    }
}
