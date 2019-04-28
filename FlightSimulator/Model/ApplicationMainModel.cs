using FlightSimulator.Model.Interface;
using FlightSimulator.Utils;
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
using System.Windows.Forms;

/// <summary>
/// The FlightSimulator.Model Interface of All the Models.
/// </summary>
namespace FlightSimulator.Model
{
    /// <summary>
    /// The ApplicationMainModel Class of the model of the Main Window
    /// which implements the IMainModel Interface.
    /// </summary>
    class ApplicationMainModel : IMainModel
    {

        /// <summary>
        /// The m_planeProperties member is a static member which holds
        /// the plane properties names according to the generic_small.xml
        /// file which is in the Files Folder in The Utils Folder.
        /// </summary>
        private static IList<String> m_planeProperties = DataXMLReader.getPlaneProperties();

        /// <summary>
        /// The m_planePropertiesPaths member is a static member which holds
        /// the plane properties paths according to the generic_small.xml
        /// file which is in the Files Folder in The Utils Folder.
        /// </summary>
        private static IList<String> m_planePropertiesPaths = DataXMLReader.getPlanePropertiesPaths();

        /// <summary>
        /// The m_planePropertiesValues member is a member which holds
        /// the plane properties values according to the info it gets from the simulator.
        /// </summary>
        private IList<double> m_planePropertiesValues;

        /// <summary>
        /// The m_mutex member is the Mutex in order
        /// to prevent unordered changes to data by different threads
        /// which can cause to wrong data.
        /// </summary>
        private Mutex m_mutex;

        #region Singleton
        /// <summary>
        /// The m_instance IMainModel is the static member
        /// of the only one instance of this class.
        /// </summary>
        private static IMainModel m_instance = null;

        /// <summary>
        /// The PropertyChanged PropertyChangedEventHandler Event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The Instance IMainModel Property in order to get the only one instance
        /// of this class according to the Singleton Design Patten.
        /// </summary>
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

        /// <summary>
        /// The ApplicationMainModel Constructor.
        /// </summary>
        private ApplicationMainModel()
        {
            SettingsModel = ApplicationSettingsModel.Instance;
            m_planePropertiesValues = null;
            m_mutex = new Mutex();
        }
        #endregion

        /// <summary>
        /// The InfoServerTCPListener TcpListener Property
        /// is the Tcp Listener of the Info Server which
        /// gets the values of the plane properties
        /// from the simulator.
        /// </summary>
        public TcpListener InfoServerTCPListener { get; set; }

        /// <summary>
        /// The Client TcpClient Property
        /// is the TcpClient of the Client
        /// which sends commands to the simulator
        /// like send commands.
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// The ShouldInfoServerRun bool Property
        /// of whether the Info Server should run
        /// or not.
        /// </summary>
        private bool ShouldInfoServerRun { get; set; }

        /// <summary>
        /// The ShouldClientConnect bool Property
        /// of whether the Client Should be connected
        /// or not.
        /// </summary>
        private bool ShouldClientConnect { get; set; }

        /// <summary>
        /// The SettingsModel ISettingsModel Property
        /// of the Settings Window Model.
        /// </summary>
        private ISettingsModel SettingsModel { get; set; }

        /// <summary>
        /// The InfoServerThread Thread Property
        /// of the Thread which runs The Info Server.
        /// </summary>
        private Thread InfoServerThread { get; set; }

        /// <summary>
        /// The ClientConnectionThread Thread Property
        /// of the Thread which runs The Connection function of the Client.
        /// </summary>
        private Thread ClientConnectionThread { get; set; }

        /// <summary>
        /// The ValueXKnob double Priority
        /// of the x position value of the Joystick's Knob.
        /// </summary>
        public double ValueXKnob { get; set; }

        /// <summary>
        /// The ValueYKnob double Priority
        /// of the y position value of the Joystick's Knob.
        /// </summary>
        public double ValueYKnob { get; set; }

        /// <summary>
        /// The Connect function
        /// starts the Info Server and connects the Client
        /// </summary>
        public void Connect()
        {
            Stop();
            ShouldInfoServerRun = true;
            InfoServerThread = new Thread(ConnectAndStartServer);
            InfoServerThread.IsBackground = true;
            InfoServerThread.Start();
            ShouldClientConnect = true;
            ClientConnectionThread = new Thread(ConnectClient);
            ClientConnectionThread.IsBackground = true;
            ClientConnectionThread.Start();
        }

        /// <summary>
        /// The ConnectAndStartServer function starts the Info Server.
        /// </summary>
        private void ConnectAndStartServer()
        {
            InfoServerTCPListener = new TcpListener(IPAddress.Any, SettingsModel.FlightInfoPort);
            // we set our IP address as server's address, and we also set the port

            InfoServerTCPListener.Start();  // this will start the server

            // ShouldInfoServerRun = true;

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

                m_planePropertiesValues = null;
                StreamReader sr = new StreamReader(ns);
                string line = sr.ReadLine();
                
                while (client != null && client.Connected && ShouldInfoServerRun)  //while the client is connected, we look for incoming messages
                {
                    if (line == null)
                    {
                        client.Close();
                        client = null;
                        break;
                    }
                    if (m_planePropertiesValues == null)
                    {
                        m_planePropertiesValues = line.Split(",".ToArray()).ToList().ConvertAll<double>((String str) => double.Parse(str));
                        foreach (String propertyName in m_planeProperties)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                        }
                    } else
                    {
                        List<double> newPlanePropertiesValues = line.Split(",".ToArray()).ToList().ConvertAll<double>((String str) => double.Parse(str));
                        for (int i = 0; i < m_planePropertiesValues.Count; i++)
                        {
                            if (newPlanePropertiesValues[i] != m_planePropertiesValues[i])
                            {
                                m_mutex.WaitOne();
                                m_planePropertiesValues[i] = newPlanePropertiesValues[i];
                                PropertyChanged(this, new PropertyChangedEventArgs(m_planeProperties[i]));
                                m_mutex.ReleaseMutex();
                            }
                        }
                    }
                    line = sr.ReadLine();
                }
            }
        }

        /// <summary>
        /// The ConnectClient function connects the client to the simulator.
        /// </summary>
        private void ConnectClient()
        {
            while (ShouldClientConnect)
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
                    if (e.SocketErrorCode == SocketError.NotConnected)
                    {
                        break;
                    }
                    throw e;
                }
            }
        }

        /// <summary>
        /// The SendCommand function gets as a parameter
        /// a String command of a the command to send to the simulator
        /// from the client and returns an int value of the return status of the function
        /// 0 for success -1 for error.
        /// <param name="command">String command to send to the simulator.</para>
        /// <retValue>int value of return status of the function.</retValue>
        /// </summary>
        public int SendCommand(string command)
        {
            if (Client != null && Client.Connected)
            {
                NetworkStream stream = Client.GetStream();
                StreamWriter sw = new StreamWriter(stream);
                sw.WriteLine(command);
                sw.Flush();
                return 0;
            } else
            {
                return -1;
            }
        }

        /// <summary>
        /// The Stop function stop the running of the Info Server
        /// and closes the Connection of the Client.
        /// </summary>
        public void Stop()
        {
            if (InfoServerThread != null && InfoServerThread.IsAlive)
            {
                ShouldInfoServerRun = false;
                InfoServerTCPListener.Stop();
                if (InfoServerThread.IsAlive)
                {
                    InfoServerThread.Abort();
                }
            }
            if (ClientConnectionThread != null && ClientConnectionThread.IsAlive)
            {
                ShouldClientConnect = false;
                if (ClientConnectionThread.IsAlive)
                {
                    ClientConnectionThread.Abort();
                }
            }
            if (Client != null && Client.Client != null && Client.Connected)
            {
                Client.Close();
            }
            
        }

        /// <summary>
        /// The getValueOfProperty function gets as a parameter
        /// a String propertyName of a property of the plane
        /// and returns its current value.
        /// <param name="propertyName">String propertyName of a property of the plane
        /// we want the current value of.</para>
        /// <retValue>double value of the property of the plane
        /// with the name propertyName</retValue>
        /// </summary>
        public double getValueOfProperty(String propertyName)
        {
            for (int i = 0; i < m_planeProperties.Count; i++)
            {
                if (m_planeProperties[i].Equals(propertyName))
                {
                    m_mutex.WaitOne();
                    double returnValue = m_planePropertiesValues[i];
                    m_mutex.ReleaseMutex();
                    return returnValue;
                }
            }
            throw new Exception("No Property Named: " + propertyName);
        }

        /// <summary>
        /// The handleKnobMouseMove function gets as parameters
        /// 2 Points of startPoint and newPoint and handles the knob move
        /// of the Joystick by setting the new ValueXKnob and ValueYKnob.
        /// <param name="startPoint">Point of the start position of the knob.</para>
        /// <param name="newPoint">Point of the new position of the knob.</param>
        /// </summary>
        public void handleKnobMouseMove(Point startPoint, Point newPoint)
        {


            Point deltaPos = new Point(newPoint.X - startPoint.X, newPoint.Y - startPoint.Y);

            double distance = Math.Round(Math.Sqrt(deltaPos.X * deltaPos.X + deltaPos.Y * deltaPos.Y));

            ValueXKnob = deltaPos.X;
            ValueYKnob = -deltaPos.Y;
            
        }

        /// <summary>
        /// The Finalizer to close the connections.
        /// </summary>
        ~ApplicationMainModel()
        {
            Stop();
        }
    }
}
