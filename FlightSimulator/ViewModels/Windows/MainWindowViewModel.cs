using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace FlightSimulator.ViewModels.Windows
{
    public class MainWindowViewModel : BaseNotify
    {
        private IMainModel model;

        private String m_autoPilotText;

        //private FlightBoardViewModel flightBoardViewModel;

        public MainWindowViewModel(IMainModel model)
        {
            this.model = model;
            model.PropertyChanged += handlePropertyChanged;
            Aileron = 0;
            Lat = 0;
            m_autoPilotText = "";
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

        public void handleKnobMove(Point startPoint, Point newPoint)
        {
            model.handleKnobMouseMove(startPoint, newPoint);
            if (Aileron != model.ValueXKnob)
            {
                if (sendCommand("set /controls/flight/aileron " + model.ValueXKnob) == 0)
                {
                    Aileron = model.ValueXKnob;
                    NotifyPropertyChanged("Aileron");
                } else
                {
                    return;
                }
            }
            if (Elevator != model.ValueYKnob)
            {
                if (sendCommand("set /controls/flight/elevator " + model.ValueYKnob) == 0)
                {
                    Elevator = model.ValueYKnob;
                    NotifyPropertyChanged("Elevator");
                } else
                {
                    return;
                }
            }

        }

        private void sendConnectionErrorMessage()
        {
            System.Windows.MessageBox.Show("Error The Client Isn't Connected Please Click The Connect Button First!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public double Aileron
        {
            get;
            private set;
        }

        public double Elevator
        {
            get;
            private set;
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

        public String AutoPilotText
        {
            get
            {
                return m_autoPilotText;
            }
            set
            {
                if (m_autoPilotText != value)
                {
                    m_autoPilotText = value;
                    NotifyPropertyChanged("AutoPilotText");
                    if (m_autoPilotText != "")
                    {
                        AutoPilotTextBoxColor = Brushes.LightPink;
                    } else
                    {
                        AutoPilotTextBoxColor = Brushes.White;
                    }
                }
            }
        }

        private Brush m_autoPilotTextBoxColor;

        public Brush AutoPilotTextBoxColor
        {
            get
            {
                return m_autoPilotTextBoxColor;
            }
            private set
            {
                if (m_autoPilotTextBoxColor == null || !m_autoPilotTextBoxColor.Equals(value))
                {
                    m_autoPilotTextBoxColor = value;
                    NotifyPropertyChanged("AutoPilotTextBoxColor");
                }
            }
        }

        private int sendCommand(string command)
        {
            int returnValue = model.SendCommand(command);
            if (returnValue == -1)
            {
                sendConnectionErrorMessage();
            }
            return returnValue;
        }

        private double m_throttle;

        public double Throttle
        {
            get
            {
                return m_throttle;
            }
            set
            {
                double newValue = Math.Floor(value * 100) / 100;
                if (newValue != m_throttle)
                {
                    if (sendCommand("set /controls/engines/current-engine/throttle " + newValue) == 0)
                    {
                        m_throttle = newValue;
                        NotifyPropertyChanged("Throttle");
                    }
                }
            }
        }

        private double m_rudder;

        public double Rudder
        {
            get
            {
                return m_rudder;
            }
            set
            {
                double newValue = Math.Floor(value * 100) / 100;
                if (newValue != m_rudder)
                {
                    if (sendCommand("set /controls/flight/rudder " + Rudder) == 0)
                    {
                        m_rudder = newValue;
                        NotifyPropertyChanged("Rudder");
                    }
                }
            }
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
        #region OkButtonClickCommand
        private ICommand _okButtonClickCommand;
        public ICommand OkButtonClickCommand
        {
            get
            {
                return _okButtonClickCommand ?? (_okButtonClickCommand = new CommandHandlerWithParameter<string>((String command) => OnOkButtonClick(command)));
            }
        }
        private void OnOkButtonClick(String command)
        {
            if (sendCommand(command) == 0)
            {
                AutoPilotTextBoxColor = Brushes.White;
            }
        }
        #endregion
        #region ClearButtonClickCommand
        private ICommand _clearButtonClickCommand;
        public ICommand ClearButtonClickCommand
        {
            get
            {
                return _clearButtonClickCommand ?? (_clearButtonClickCommand = new CommandHandler(() => OnClearButtonClick()));
            }
        }
        private void OnClearButtonClick()
        {
            AutoPilotText = "";
        }
        #endregion
        #endregion
    }
}
