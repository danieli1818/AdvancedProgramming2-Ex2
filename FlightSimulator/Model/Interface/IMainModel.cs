using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model.Interface
{
    public interface IMainModel : INotifyPropertyChanged
    {
        TcpListener InfoServerTCPListener { get; set; } // The Server TCPListener
        TcpClient Client { get; set; } // The Client

        double getValueOfProperty(String propertyName);

        void Connect();
        void SendCommand(string command);
        void Stop();
    }
}
