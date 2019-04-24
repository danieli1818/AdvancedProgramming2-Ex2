using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using FlightSimulator.ViewModels.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

/// <summary>
/// The FlightSimulator Namespace.
/// </summary>
namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        /// <summary>
        /// The ViewModel SettingsWindowViewModel Property of the Setting Window's View Model.
        /// </summary>
        public SettingsWindowViewModel ViewModel { get; set; }

        /// <summary>
        /// The Settings Constructor.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
            ViewModel = new SettingsWindowViewModel(ApplicationSettingsModel.Instance);
            this.DataContext = ViewModel;
        }
    }
}
