using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels.Windows
{
    public class MainWindowViewModel : BaseNotify
    {
        private IMainModel model;

        private FlightBoardViewModel flightBoardViewModel;

        public MainWindowViewModel(IMainModel model)
        {
            this.model = model;
            model.PropertyChanged += handlePropertyChanged;
            //flightBoardViewModel = new FlightBoardViewModel();
        }

        private void handlePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals("longitude-deg"))
            {
                Lon = model.getValueOfProperty(args.PropertyName);
                NotifyPropertyChanged("Lon");
            } else if (args.PropertyName.Equals("latitude-deg"))
            {
                Lat = model.getValueOfProperty(args.PropertyName);
                NotifyPropertyChanged("Lat");
            }
        }

        public double Lon
        {
            get;
            private set;
        }

        public double Lat
        {
            get;
            private set;
        }

        /*
        public string FlightServerIP
        {
            get { return model.FlightServerIP; }
            set
            {
                model.FlightServerIP = value;
                NotifyPropertyChanged("FlightServerIP");
            }
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
        }
        */

        #region Commands
        #region ConnectCommand
        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandHandler(() => OnClick()));
            }
        }
        private void OnClick()
        {
            model.Connect();
        }
        #endregion
        #region StopCommand
        private ICommand _stopCommand;
        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new CommandHandler(() => OnStop()));
            }
        }
        private void OnStop()
        {
            model.Stop();
        }
        #endregion
        #region SendCommand
        private ICommand _sendCommand;
        public ICommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new CommandHandlerWithParameter<string>((string param) => OnSend(param)));
            }
        }
        private void OnSend(string param)
        {
            model.SendCommand(param);
        }
        #endregion
        #region ClickSettingsButtonCommand
        private ICommand _clickSettingsButtonCommand;
        public ICommand ClickSettingsButtonCommand
        {
            get
            {
                return _clickSettingsButtonCommand ?? (_clickSettingsButtonCommand = new CommandHandler(() => OnClickSettingsButton()));
            }
        }
        private Settings _settingsWindow;
        private void OnClickSettingsButton()
        {
            if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            {
                _settingsWindow = new Settings();
                _settingsWindow.Show();
            }
        }
        #endregion
        #endregion
    }
}
