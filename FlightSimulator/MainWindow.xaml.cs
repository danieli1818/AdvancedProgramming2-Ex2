using FlightSimulator.Model;
using FlightSimulator.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/// <summary>
/// The FlightSimulator Namespace.
/// </summary>
namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// The ViewModel MainWindowViewModel Property of the Main Window's View Model.
        /// </summary>
        public MainWindowViewModel ViewModel { get; set; }

        /// <summary>
        /// The MainWindow Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(ApplicationMainModel.Instance);
            FlightBoard.addPropertyChangedFunctionToINotifyPropertyChanged(ViewModel);
            Joystick.KnobMouseMoveCapturedEvent += ViewModel.handleKnobMove;
            Joystick.KnobMouseResetEvent += ViewModel.handleKnobReset;
            this.DataContext = ViewModel;
        }

    }
}
