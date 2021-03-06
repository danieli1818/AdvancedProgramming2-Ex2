﻿using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// It extends BaseNotify.
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
        /// <param name="fb">FlightBoard fb which we change when Lan or Lat changes.</param>
        /// <param name="j">Joystick j to which we handle moving the knob.</param>
        /// </summary>
        public MainWindowViewModel(IMainModel model, FlightBoard fb, Joystick j)
        {
            this.model = model;
            model.PropertyChanged += handlePropertyChanged;
            onServerConnected += connectClientEventHandler;
            Aileron = 0;
            Lat = 0;
            AutoPilotText = "";
            AutoPilotTextBoxBackgroundColor = "White";
            fb.addPropertyChangedFunctionToINotifyPropertyChanged(this);
            j.KnobMouseMoveCapturedEvent += handleKnobMove;
            j.KnobMouseResetEvent += handleKnobReset;
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
                if (sendCommand("set /controls/flight/aileron " + newAileronValue) == 0)
                {
                    Aileron = newAileronValue;
                } else
                {
                    return;
                }
            }
            double newElevatorValue = Math.Floor((model.ValueYKnob / (height / 2)) * 100) / 100;
            if (Elevator != newElevatorValue)
            {
                if (sendCommand("set /controls/flight/elevator " + newElevatorValue) == 0)
                {
                    Elevator = newElevatorValue;
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
            double newAileron = 0;
            double newElevator = 0;
            if (sendCommand("set /controls/flight/aileron " + newAileron) == 0)
            {
                Aileron = newAileron;
            }
            else
            {
                return;
            }
            if (sendCommand("set /controls/flight/elevator " + newElevator) == 0)
            {
                Elevator = newElevator;
            }
            else
            {
                return;
            }

        }

        /// <summary>
        /// The sendConnectionErrorMessage function sends an error message.
        /// </summary>
        private void sendConnectionErrorMessage()
        {
            System.Windows.MessageBox.Show("Error The Client Isn't Connected Please Click The Connect Button First!", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// The m_aileron double member.
        /// </summary>
        private double m_aileron;

        /// <summary>
        /// The Aileron double Property.
        /// </summary>
        public double Aileron
        {
            get
            {
                return m_aileron;
            }
            private set
            {
                if (m_aileron != value)
                {
                    m_aileron = value;
                    NotifyPropertyChanged("Aileron");
                }
            }
        }

        /// <summary>
        /// The m_elevator double member.
        /// </summary>
        private double m_elevator;

        /// <summary>
        /// The Elevator Double Property.
        /// </summary>
        public double Elevator
        {
            get
            {
                return m_elevator;
            }
            private set
            {
                if (m_elevator != value)
                {
                    m_elevator = value;
                    NotifyPropertyChanged("Elevator");
                }
            }
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
                        AutoPilotTextBoxBackgroundColor = "LightPink";
                    } else
                    {
                        AutoPilotTextBoxBackgroundColor = "White";
                    }
                }
            }
        }

        /// <summary>
        /// The m_autoPilotTextBoxBackgroundColor member
        /// of the string background color of the auto pilot text box.
        /// </summary>
        private string m_autoPilotTextBoxBackgroundColor;

        /// <summary>
        /// The AutoPilotTextBoxBackgroundColor Property
        /// of the String background color of the auto pilot text box.
        /// </summary>
        public String AutoPilotTextBoxBackgroundColor
        {
            get
            {
                return m_autoPilotTextBoxBackgroundColor;
            }
            private set
            {
                if (AutoPilotTextBoxBackgroundColor == null || (value != null && !m_autoPilotTextBoxBackgroundColor.Equals(value)))
                {
                    m_autoPilotTextBoxBackgroundColor = value;
                    NotifyPropertyChanged("AutoPilotTextBoxBackgroundColor");
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
                    if (sendCommand("set /controls/flight/rudder " + newValue) == 0)
                    {
                        m_rudder = newValue;
                        NotifyPropertyChanged("Rudder");
                    }
                }
            }
        }

        /// <summary>
        /// The EventHandler Event Of Server Connected Means Finished Connecting To The Client.
        /// </summary>
        private event EventHandler onServerConnected;

        /// <summary>
        /// connectClientEventHandler function is an EventHandler which connects the client.
        /// </summary>
        /// <param name="sender">object sender of who sent the Event.</param>
        /// <param name="args">EventArgs args arguments of the Event.</param>
        private void connectClientEventHandler(object sender, EventArgs args)
        {
            if (sender as IMainModel == model)
            {
                model.ConnectClientToSimulator();
            }
        }

        #region Commands
        #region ConnectCommand
        /// <summary>
        /// The _connectCommand ICommand member of the connect command.
        /// </summary>
        private ICommand _connectCommand;

        /// <summary>
        /// The ConnectCommand ICommand Property of the connect command.
        /// </summary>
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandHandler(() => OnConnectClick()));
            }
        }

        /// <summary>
        /// The OnConnectClick function of handling Connect Button Click.
        /// </summary>
        private void OnConnectClick()
        {
            model.ConnectServer(onServerConnected);
        }
        #endregion
        #region StopCommand
        /// <summary>
        /// The _stopCommand ICommand member of the stop command.
        /// </summary>
        private ICommand _stopCommand;
        /// <summary>
        /// The StopCommand ICommand Property of the stop command.
        /// </summary>
        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new CommandHandler(() => OnStop()));
            }
        }
        /// <summary>
        /// The OnStop function of the stop command action.
        /// </summary>
        private void OnStop()
        {
            model.Stop();
        }
        #endregion
        #region ClickSettingsButtonCommand
        /// <summary>
        /// The _clickSettingsButtonCommand ICommand member of the click settings button command.
        /// </summary>
        private ICommand _clickSettingsButtonCommand;
        /// <summary>
        /// The ClickSettingsButtonCommand ICommand Property of the click settings button command.
        /// </summary>
        public ICommand ClickSettingsButtonCommand
        {
            get
            {
                return _clickSettingsButtonCommand ?? (_clickSettingsButtonCommand = new CommandHandler(() => OnClickSettingsButton()));
            }
        }
        /// <summary>
        /// The _settingsWindow Settings window of the settings.
        /// </summary>
        private Settings _settingsWindow;
        /// <summary>
        /// The OnClickSettingsButton function of handling the settings button click.
        /// </summary>
        private void OnClickSettingsButton()
        {
            if (_settingsWindow == null || !_settingsWindow.IsLoaded)
            {
                _settingsWindow = new Settings();
                _settingsWindow.ShowDialog();
            }
        }
        #endregion
        #region OkButtonClickCommand
        /// <summary>
        /// The _okButtonClickCommand ICommand member of the command of clicking the ok button.
        /// </summary>
        private ICommand _okButtonClickCommand;
        /// <summary>
        /// The OkButtonClickCommand ICommand Property of the command of clicking the ok button.
        /// </summary>
        public ICommand OkButtonClickCommand
        {
            get
            {
                return _okButtonClickCommand ?? (_okButtonClickCommand = new CommandHandler(() => OnOkButtonClick()));
            }
        }

        /// <summary>
        /// The sendingCommandsThread Thread of sending the commands in The Auto Pilot Text Box.
        /// </summary>
        private Thread sendingCommandsThread;

        /// <summary>
        /// The OnOkButtonClick function of handling the clicking on the ok button.
        /// </summary>
        private void OnOkButtonClick()
        {
            String command = AutoPilotText;
            if (sendingCommandsThread != null && sendingCommandsThread.IsAlive)
            {
                sendingCommandsThread.Abort();
            }
            sendingCommandsThread = new Thread(new ParameterizedThreadStart(sendCommandsOneAfterAnother));
            sendingCommandsThread.Start(command.Split("\r\n".ToArray<char>()));
        }

        /// <summary>
        /// The sendCommandsOneAfterAnother function
        /// which gets as a parameter an object commands
        /// which is string[] of commands to send to the flight simulator
        /// and sends each one one after another with 2 seconds cooldown
        /// between each one.
        /// </summary>
        /// <param name="commands">object commands which is string[] commands
        /// of commands to send to the flight simulator.</param>
        private void sendCommandsOneAfterAnother(object commands)
        {
            if (commands == null)
            {
                return;
            }
            string[] commandsArray = commands as string[];
            if (commandsArray == null)
            {
                throw new Exception("Not Valid Parameter!!!!");
            }
            int i = 0;
            foreach (string command in commandsArray)
            {
                if (sendCommand(command) == -1)
                {
                    return;
                }
                if (i != commandsArray.Length - 1)
                {
                    Thread.Sleep(2000);
                    i++;
                }
            }
            AutoPilotTextBoxBackgroundColor = "White";
        }
        #endregion
        #region ClearButtonClickCommand
        /// <summary>
        /// The _clearButtonClickCommand ICommand member of the command of clicking the clear button.
        /// </summary>
        private ICommand _clearButtonClickCommand;
        /// <summary>
        /// The ClearButtonClickCommand ICommand Property of the command of clicking the clear button.
        /// </summary>
        public ICommand ClearButtonClickCommand
        {
            get
            {
                return _clearButtonClickCommand ?? (_clearButtonClickCommand = new CommandHandler(() => OnClearButtonClick()));
            }
        }
        /// <summary>
        /// The OnClearButtonClick function of handling the clicking on the clear button.
        /// </summary>
        private void OnClearButtonClick()
        {
            AutoPilotText = "";
        }
        #endregion
        #endregion
    }
}
