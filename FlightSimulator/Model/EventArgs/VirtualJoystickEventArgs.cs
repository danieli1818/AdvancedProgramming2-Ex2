using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// FlightSimulator.Model.EventArgs Namespace.
/// </summary>
namespace FlightSimulator.Model.EventArgs
{
    /// <summary>
    /// VirtualJoystickEventArgs Class
    /// is the class of the Aileron and Elevator Values
    /// which are used as the Virtual Joystick's Event Args.
    /// </summary>
    public class VirtualJoystickEventArgs
    {
        /// <summary>
        /// Aileron Value
        /// </summary>
        public double Aileron { get; set; }

        /// <summary>
        /// Elevator Value
        /// </summary>
        public double Elevator { get; set; }
    }
}
