using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The FlightSimulator.Model.Interface Interface
/// which holds all the Models' Interfaces.
/// </summary>
namespace FlightSimulator.Model.Interface
{
    /// <summary>
    /// The ISettingsModel Interface is the Interface of the Settings Window.
    /// </summary>
    public interface ISettingsModel
    {
        /// <summary>
        /// The FlightServerIP String Property which holds the IP of the Flight Server.
        /// </summary>
        string FlightServerIP { get; set; }          // The IP Of the Flight Server

        /// <summary>
        /// The FlightInfoPort int Property which holds the Port of the Flight Server
        /// from which it sends the values of the Components of the plane according to the xml file.
        /// </summary>
        int FlightInfoPort { get; set; }           // The Port of the Flight Server

        /// <summary>
        /// The FlightCommandPort int Property which holds the port of the Flight Server
        /// from which it gets commands to do like set commands.
        /// </summary>
        int FlightCommandPort { get; set; }           // The Port of the Flight Server

        /// <summary>
        /// The SaveSettings Function saves the settings to be the default settings.
        /// </summary>
        void SaveSettings();

        /// <summary>
        /// The ReloadSettings sets the values of the current settings to the saved default settings.
        /// </summary>
        void ReloadSettings();
    }
}
