using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FlightSimulator.Model.Interface
{
    public interface IMainModel : INotifyPropertyChanged
    {
        TcpListener InfoServerTCPListener { get; set; } // The Server TCPListener
        TcpClient Client { get; set; } // The Client
        double ValueXKnob { get; set; } // X Value Knob
        double ValueYKnob { get; set; } // Y Value Knob

        double getValueOfProperty(String propertyName);

        void handleKnobMouseMove(Point startPoint, Point newPoint);

        void Connect();
        void SendCommand(string command);
        void Stop();
    }
}
