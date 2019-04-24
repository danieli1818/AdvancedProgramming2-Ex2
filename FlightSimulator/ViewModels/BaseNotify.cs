using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The FlightSimulator.ViewModels Namespace of all the classes which are connected to The View Models
/// and aren't Window's View Models.
/// </summary>
namespace FlightSimulator.ViewModels
{
    /// <summary>
    /// The BaseNotify abstract class of easy notifying
    /// about a property that changed.
    /// It implements INotifyPropertyChanged.
    /// </summary>
    public abstract class BaseNotify : INotifyPropertyChanged
    {
        /// <summary>
        /// The PropertyChanged PropertyChangedEventHandler Event of all the functions
        /// which handles a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The NotifyPropertyChanged function gets as a parameter
        /// a string propName and notify that Property with the name of propName
        /// changed.
        /// <param name="propName">The string propName which is the name of the property
        /// that changed.</para>
        /// </summary>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
