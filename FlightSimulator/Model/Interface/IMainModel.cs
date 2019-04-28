using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

/// <summary>
/// The FlightSimulator.Model.Interface Namespace
/// which holds all the Models' Interfaces.
/// </summary>
namespace FlightSimulator.Model.Interface
{
    /// <summary>
    /// The IMainModel Interface is the main window's model interface.
    /// It extends the Interface INotifyPropertyChanged.
    /// </summary>
    public interface IMainModel : INotifyPropertyChanged
    {
        /// <summary>
        /// InfoServerTCPListener is the TCP Listener of the Info Server.
        /// </summary>
        TcpListener InfoServerTCPListener { get; set; } // The Server TCPListener

        /// <summary>
        /// Client is the TcpClient of the client which is used
        /// to send set commands to the simulator.
        /// </summary>
        TcpClient Client { get; set; } // The Client

        /// <summary>
        /// ValueXKnob is the x value of the knob.
        /// </summary>
        double ValueXKnob { get; set; } // X Value Knob

        /// <summary>
        /// ValueYKnob is the y value of the knob.
        /// </summary>
        double ValueYKnob { get; set; } // Y Value Knob

        /// <summary>
        /// The getValueOfProperty function gets as a parameter
        /// a String propertyName of a property name and returns
        /// its current value.
        /// <param name="propertyName">String propertyName is the property name
        /// of the property we want to get the value of.</para>
        /// <retValue>current value of the property
        /// with the name propertyName</retValue>
        /// </summary>
        double getValueOfProperty(String propertyName);

        /// <summary>
        /// handleKnobMouseMove function gets as parameters
        /// 2 points which are named startPoint and newPoint
        /// and handles the change of the knob position
        /// form startPoint to the newPoint.
        /// <param name="startPoint">the startPoint Point is the Point
        /// of the start position of the knob.</para>
        /// <param name="newPoint">the newPoint Point is the Point
        /// of the new position of the knob.</para>
        /// </summary>
        void handleKnobMouseMove(Point startPoint, Point newPoint);

        /// <summary>
        /// Connect function doesn't get parameters
        /// and starts the server and connects it to the Simulator.
        /// </summary>
        void ConnectServer(EventHandler serverConnected);

        /// <summary>
        /// ConnectClient function doesn't get parameters
        /// and connects the client to the Simulator.
        /// </summary>
        void ConnectClientToSimulator();

        /// <summary>
        /// The SendCommand function gets as a parameter
        /// a String command of a the command to send to the simulator
        /// from the client and returns an int value of the return status of the function
        /// for example 0 for success -1 for error.
        /// <param name="command">String command to send to the simulator.</para>
        /// <retValue>int value of return status of the function.</retValue>
        /// </summary>
        int SendCommand(string command);

        /// <summary>
        /// The Stop function doesn't get parameters.
        /// It stops the server and the client.
        /// </summary>
        void Stop();
    }
}
