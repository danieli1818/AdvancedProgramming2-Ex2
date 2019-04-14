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

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(ApplicationMainModel.Instance);
            this.DataContext = ViewModel;
            FlightBoard.addPropertyChangedFunctionToINotifyPropertyChanged(ViewModel);
            Joystick.KnobMouseMoveCapturedEvent += ViewModel.handleKnobMove;
            //ViewModel.PropertyChanged += (object sender, PropertyChangedEventArgs args) =>
            //{
                /*DependencyObject property = FindName(args.PropertyName + "Value") as DependencyObject;
                if (property != null)
                {
                    BindingOperations.GetBindingExpression(property, ContentProperty).UpdateTarget();
                }*/
            //};
            //ViewModel.UpdatePropertiesEvent += () =>
            //{
                //BindingOperations.GetBindingExpression(AileronValue, ContentProperty).UpdateTarget();
                //BindingOperations.GetBindingExpression(ElevatorValue, ContentProperty).UpdateTarget();
            //};
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.StopCommand.Execute(null);
        }

    }
}
