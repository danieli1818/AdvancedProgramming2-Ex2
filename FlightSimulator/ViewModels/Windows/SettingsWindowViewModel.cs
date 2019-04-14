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

namespace FlightSimulator.ViewModels.Windows
{
    public class SettingsWindowViewModel : BaseNotify
    {
        private ISettingsModel model;

        public SettingsWindowViewModel(ISettingsModel model)
        {
            this.model = model;
        }

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

        public int FlightCommandPort
        {
            get { return model.FlightCommandPort; }
            set
            {
                model.FlightCommandPort = value;
                NotifyPropertyChanged("FlightCommandPort");
            }
        }

        public int FlightInfoPort
        {
            get { return model.FlightInfoPort; }
            set
            {
                model.FlightInfoPort = value;
                NotifyPropertyChanged("FlightInfoPort");
            }
        }

     

        public void SaveSettings()
        {
            model.SaveSettings();
        }

        public void ReloadSettings()
        {
            model.ReloadSettings();
            NotifyPropertyChanged("FlightServerIP");
            NotifyPropertyChanged("FlightInfoPort");
            NotifyPropertyChanged("FlightCommandPort");
        }

        #region Commands
        #region ClickCommand
        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => OnClick()));
            }
        }
        private void OnClick()
        {
            SaveSettings();
        }
        #endregion

        #region CancelCommand
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandHandler(() => OnCancel()));
            }
        }
        private void OnCancel()
        {
            ReloadSettings();
        }
        #endregion
        #endregion
    }
}

