using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using FlightSimulator.ViewModels.Windows;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

/// <summary>
/// The FlightSimulator.Views Namespace of all the Views.
/// </summary>
namespace FlightSimulator.Views
{
    /// <summary>
    /// The FlightBoard class is a UserControl of the flight board view.
    /// </summary>
    public partial class FlightBoard : UserControl
    {
        /// <summary>
        /// The planeLocations ObservableDataSource of Points
        /// holds all the points in the flight board view.
        /// </summary>
        ObservableDataSource<Point> planeLocations = null;
        /// <summary>
        /// The FlightBoard Constructor.
        /// </summary>
        public FlightBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The UserControl_Loaded function gets as parameters
        /// an object sender and a RoutedEventArgs
        /// and finish the load of the flight board.
        /// <param name="sender">object sender of the sender of the call to this function.</para>
        /// <param name="e">RoutedEventArgs e arguments about the event which caused
        /// the call to this function.</param>
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            planeLocations = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            planeLocations.SetXYMapping(p => p);

            plotter.AddLineGraph(planeLocations, 2, "Route");
        }

        /// <summary>
        /// The Vm_PropertyChanged function gets as parameters
        /// an object sender and PropertyChangedEventArgs e
        /// and handles the property changed event.
        /// <param name="sender">object sender of the event which caused
        /// the call of this function.</para>
        /// <param name="e">PropertyChangedEventArgs e of the information
        /// on the property which changed.</param>
        /// </summary>
        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Lat") || e.PropertyName.Equals("Lon")) 
            {
                MainWindowViewModel mwvm = sender as MainWindowViewModel;
                if (mwvm == null)
                {
                    throw new Exception("Not Valid Sender!!!!");
                }
                Point p1 = new Point(mwvm.Lat, mwvm.Lon);            // Fill here!
                planeLocations.AppendAsync(Dispatcher, p1);
                planeLocations.ResumeUpdate();
            }
        }

        /// <summary>
        /// The addPropertyChangedFunctionToINotifyPropertyChanged function gets as a parameter
        /// an INotifyPropertyChanged npc and adds to its PropertyChanged Event the Vm_PropertyChanged function.
        /// <param name="npc">INotifyPropertyChanged npc to add the Vm_PropertyChanged Function To.</para>
        /// </summary>
        public void addPropertyChangedFunctionToINotifyPropertyChanged(INotifyPropertyChanged npc)
        {
            npc.PropertyChanged += Vm_PropertyChanged;
        }

    }

}

