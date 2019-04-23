using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The FlightSimulator.Model Namespace of the models.
/// </summary>
namespace FlightSimulator.Model
{
    /// <summary>
    /// The ApplicationSettingsModel class of the Settings Window Model
    /// which implements the ISettingsModel Interface.
    /// </summary>
    public class ApplicationSettingsModel : ISettingsModel
    {
        #region Singleton
        /// <summary>
        /// The m_Instance ISettingsModel static member
        /// of the only one instance of this class.
        /// </summary>
        private static ISettingsModel m_Instance = null;

        /// <summary>
        /// The Instance ISettingsModel Property which is used
        /// to get the only instance of the class
        /// according to the Singleton Design Pattern.
        /// </summary>
        public static ISettingsModel Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new ApplicationSettingsModel();
                }
                return m_Instance;
            }
        }
        #endregion
        /// <summary>
        /// The FlightServerIP string Property
        /// of the IP of the Flight Server.
        /// </summary>
        public string FlightServerIP
        {
            get { return Properties.Settings.Default.FlightServerIP; }
            set { Properties.Settings.Default.FlightServerIP = value; }
        }

        /// <summary>
        /// The FlightCommandPort int Property
        /// of the port of the Flight Server in which
        /// it is listening to the command from the client
        /// like the set commands.
        /// </summary>
        public int FlightCommandPort
        {
            get { return Properties.Settings.Default.FlightCommandPort; }
            set { Properties.Settings.Default.FlightCommandPort = value; }
        }

        /// <summary>
        /// The FlightInfoPort int Property
        /// of the port of the Flight Server from which
        /// it is sending the Plane Properties Values
        /// according to the generic_small.xml file.
        /// </summary>
        public int FlightInfoPort
        {
            get { return Properties.Settings.Default.FlightInfoPort; }
            set { Properties.Settings.Default.FlightInfoPort = value; }
        }

        /// <summary>
        /// The SaveSettings function
        /// saves the current settings
        /// as the Default Values.
        /// </summary>
        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// The ReloadSettings function
        /// reloads the settings to the default settings.
        /// </summary>
        public void ReloadSettings()
        {
            Properties.Settings.Default.Reload();
        }
    }
}
