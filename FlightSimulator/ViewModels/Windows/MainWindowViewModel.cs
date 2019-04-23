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

/// <summary>
/// The FlightSimulator.ViewModels.Windows Namespace of the Windows View Models.
/// </summary>
namespace FlightSimulator.ViewModels.Windows
{
    /// <summary>
    /// The MainWindowViewModel Class is The View Model Class of the Main Window.
    /// It implements BaseNotify.
    /// </summary>
    public class MainWindowViewModel : BaseNotify
    {
        /// <summary>
        /// The model IMainModel member which holds the model of the Main Window.
        /// </summary>
        private IMainModel model;

        /// <summary>
        /// The m_autoPilotText String member which holds the Auto Pilot Text.
        /// </summary>
        private String m_autoPilotText;

        /// <summary>
        /// The MainWindowViewModel constructor gets as a parameter
        /// an IMainModel model of the Main Window.
        /// <param name="model">IMainModel model of the Main Window.</para>
        /// </summary>
        public MainWindowViewModel(IMainModel model)
        {
            this.model = model;
            model.PropertyChanged += handlePropertyChanged;
            Aileron = 0;
            Lat = 0;
            m_autoPilotText = "";
        }

        /// <summary>
        /// The handlePropertyChanged function gets as parameters
        /// an object sender and PropertyChangedEventArgs args
        /// of the property that changed and takes the value of the changed property
        /// of the model and updates the value in this instance of the class.
        /// <param name="sender">object sender of the property changed event.</para>
        /// <param name="args">PropertyChangedEventArgs args of the information
        /// about the property that changed.</param>
        /// </summary>
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

        /// <summary>
        /// The handleKnobMove function gets as parameters
        /// a Point startPoint, a Point newPoint, double width and double height
        /// and handles the movement of the knob from the startPoint to the newPoint.
        /// <param name="startPoint">Point startPoint of the knob.</para>
        /// <param name="newPoint">Point newPoint of the knob.</param>
        /// <param name="width">double width of the Joystick.</param>
        /// <param name="height">double height of the Joystick.</param>
        /// </summary>
        public void handleKnobMove(Point startPoint, Point newPoint, double width, double height)
        {
            model.handleKnobMouseMove(startPoint, newPoint);
            double newAileronValue = Math.Floor((model.ValueXKnob / (width / 2)) * 100) / 100;
            if (Aileron != newAileronValue)
            {
                if (sendCommand("set /controls/flight/aileron " + model.ValueXKnob) == 0)
                {
                    Aileron = newAileronValue;
                    NotifyPropertyChanged("Aileron");
                } else
                {
                    return;
                }
            }
            double newElevatorValue = Math.Floor((model.ValueYKnob / (height / 2)) * 100) / 100;
            if (Elevator != newElevatorValue)
            {
                if (sendCommand("set /controls/flight/elevator " + model.ValueYKnob) == 0)
                {
                    Elevator = newElevatorValue;
                    NotifyPropertyChanged("Elevator");
                } else
                {
                    return;
                }
            }

        }

        /// <summary>
        /// The handleKnobReset function
        /// handles when knob reset its place.
        /// </summary>
        public void handleKnobReset()
        {
            Aileron = 0;
            Elevator = 0;
            NotifyPropertyChanged("Aileron");
            NotifyPropertyChanged("Elevator");

        }

        /// <summary>
        /// The sendConnectionErrorMessage function sends an error message.
        /// </summary>
        private void sendConnectionErrorMessage()
        {
            System.Windows.MessageBox.Show("Error The Client Isn't Connected Please Click The Connect Button First!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// The Aileron double Property.
        /// </summary>
        public double Aileron
        {
            get;
            private set;
        }

        /// <summary>
        /// The Elevator Double Property.
        /// </summary>
        public double Elevator
        {
            get;
            private set;
        }

        /// <summary>
        /// The Lon Double Property.
        /// </summary>
        public double Lon
        {
            get;
            private set;
        }

        /// <summary>
        /// The Lat Double Property.
        /// </summary>
        public double Lat
        {
            get;
            private set;
        }

        /// <summary>
        /// The AutoPilotText String Property of the text of the Auto Pilot TextBox.
        /// </summary>
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

        /// <summary>
        /// The m_autoPilotTextBoxColor Brush member of the Background Color of the Auto Pilot TextBox.
        /// </summary>
        private Brush m_autoPilotTextBoxColor;

        /// <summary>
        /// The AutoPilotTextBoxColor Brush Property of the Background Color of the Auto Pilot TextBox.
        /// </summary>
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

        /// <summary>
        /// The sendCommand function gets as a parameter
        /// a String command of a the command to send to the simulator
        /// from the client and returns an int value of the return status of the function
        /// for example 0 for success -1 for error.
        /// <param name="command">String command to send to the simulator.</para>
        /// <retValue>int value of return status of the function.</retValue>
        /// </summary>
        private int sendCommand(string command)
        {
            int returnValue = model.SendCommand(command);
            if (returnValue == -1)
            {
                sendConnectionErrorMessage();
            }
            return returnValue;
        }

        /// <summary>
        /// The m_throttle Double member of the throttle value.
        /// </summary>
        private double m_throttle;

        /// <summary>
        /// The Throttle Double Property of the throttle value.
        /// </summary>
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

        /// <summary>
        /// The m_rudder double member of the rudder value.
        /// </summary>
        private double m_rudder;

        /// <summary>
        /// The Rudder double Property of the rudder value.
        /// </summary>
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
