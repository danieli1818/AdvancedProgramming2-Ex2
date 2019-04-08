using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels.Windows
{
    public class MainWindowViewModel : BaseNotify
    {
        private IMainModel model;

        public MainWindowViewModel(IMainModel model)
        {
            this.model = model;
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
        #endregion
    }
}
