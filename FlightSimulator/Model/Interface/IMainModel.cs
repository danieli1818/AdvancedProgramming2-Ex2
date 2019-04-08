using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model.Interface
{
    public interface IMainModel
    {
        TcpListener InfoServerTCPListener { get; set; } // The Server TCPListener
        TcpClient Client { get; set; } // The Client

        void Connect();
        void Stop();
    }
}
