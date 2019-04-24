using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

/// <summary>
/// The FlightSimulator.ViewModels.Windows Namespace of the View Models' of the Windows.
/// </summary>
namespace FlightSimulator.ViewModels.Windows
{
    /// <summary>
    /// The SettingsWindowViewModel Class of the View Model of the Settings Window.
    /// It extends BaseNotify.
    /// </summary>
    public class SettingsWindowViewModel : BaseNotify
    {
        /// <summary>
        /// The model ISettingsModel member of the Settings Window's Model.
        /// </summary>
        private ISettingsModel model;

        /// <summary>
        /// The SettingsWindowViewModel Constructor which gets as a parameter
        /// an ISettingsModel model of the Settings Window's Model.
        /// <param name="model">The ISettingsModel model of the Settings Window</param>
        /// </summary>
        public SettingsWindowViewModel(ISettingsModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// The FlightServerIP string Property of The Flight Server IP.
        /// </summary>
        public string FlightServerIP
        {
            get { return model.FlightServerIP; }
            set
            {
                if (!isValidIPv4(value))
                {
                    throw new ArgumentException("Please Enter A Valid IPv4!");
                }
                model.FlightServerIP = value;
                NotifyPropertyChanged("FlightServerIP");
            }
        }

        /// <summary>
        /// The isValidIPv4 function gets as a parameter
        /// a String str and returns true if it is a valid IPv4
        /// else false.
        /// <param name="str">The String str to check if it is a valid IPv4.</para>
        /// <retValue>true if the str String is a valid IPv4 else false.</retValue>
        /// </summary>
        private bool isValidIPv4(String str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }
            String[] splittedValues = str.Split('.');
            if (splittedValues.Length != 4)
            {
                return false;
            }
            byte temp;
            return splittedValues.All(value => byte.TryParse(value, out temp));
        }

        /// <summary>
        /// The FlightCommandPort int Property of The Flight Server's Port For Getting Commands
        /// like set commands.
        /// </summary>
        public int FlightCommandPort
        {
            get { return model.FlightCommandPort; }
            set
            {
                model.FlightCommandPort = value;
                NotifyPropertyChanged("FlightCommandPort");
            }
        }

        /// <summary>
        /// The FlightInfoPort int Property of The Flight Server's Port
        /// from which it sends the information of the Plane's Properties' Values.
        /// </summary>
        public int FlightInfoPort
        {
            get { return model.FlightInfoPort; }
            set
            {
                model.FlightInfoPort = value;
                NotifyPropertyChanged("FlightInfoPort");
            }
        }


        /// <summary>
        /// The SaveSettings function saves the settings as the default settings.
        /// </summary>
        public void SaveSettings()
        {
            model.SaveSettings();
        }

        /// <summary>
        /// The ReloadSettings function of reloading the settings from the default settings.
        /// </summary>
        public void ReloadSettings()
        {
            model.ReloadSettings();
            NotifyPropertyChanged("FlightServerIP");
            NotifyPropertyChanged("FlightInfoPort");
            NotifyPropertyChanged("FlightCommandPort");
        }

        #region Commands
        #region ClickCommand
        /// <summary>
        /// The _clickCommand ICommand member of the clicking the Ok Button Command.
        /// </summary>
        private ICommand _clickCommand;
        /// <summary>
        /// The ClickCommand ICommand Property of the clicking the Ok Button Command.
        /// </summary>
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => OnClick()));
            }
        }
        /// <summary>
        /// The OnClick function of handling the clicking on the Ok Button.
        /// </summary>
        private void OnClick()
        {
            SaveSettings();
        }
        #endregion

        #region CancelCommand
        /// <summary>
        /// The _cancelCommand ICommand member of the clicking the cancel button command.
        /// </summary>
        private ICommand _cancelCommand;
        /// <summary>
        /// The CancelCommand ICommand Property of the clicking the cancel button command.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandHandler(() => OnCancel()));
            }
        }
        /// <summary>
        /// The OnCancel function of handling the clicking on the cancel button.
        /// </summary>
        private void OnCancel()
        {
            ReloadSettings();
        }
        #endregion
        #endregion
    }
}

