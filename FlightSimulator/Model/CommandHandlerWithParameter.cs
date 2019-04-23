using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

/// <summary>
/// The FlightSimulator.Model Namespace of all the models.
/// </summary>
namespace FlightSimulator.Model
{
    /// <summary>
    /// The CommandHandlerWithParameter class implements ICommand
    /// it holds an ActionFunc which is an action that gets a T Generic Type Class
    /// and does an action.
    /// <typeparamref name="T">Type of the Class Which is the parameter to the ActionFunc.</typeparamref>
    /// </summary>
    class CommandHandlerWithParameter<T> : ICommand where T : class
    {
        /// <summary>
        /// The ActionFunc delegate of functions that gets as parameters
        /// a T param and has a void type return.
        /// </summary>
        public delegate void ActionFunc(T param);

        /// <summary>
        /// The ActionFunc _action to execute.
        /// </summary>
        private ActionFunc _action;

        /// <summary>
        /// The CanExecuteChanged EventHandler Event.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// The CommandHandlerWithParameter Constructor gets as a parameter
        /// an ActionFunc action of the action to execute.
        /// </summary>
        public CommandHandlerWithParameter(ActionFunc action)
        {
            _action = action;
        }

        /// <summary>
        /// The CanExecute function gets as a parameter
        /// an object parameter and returns whether this action can execute or not.
        /// <param name="parameter">object parameter for the action.</para>
        /// <retValue>bool of whether the action can execute with the parameter
        /// or not.</retValue>
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            T param = parameter as T;
            return param != null;
        }

        /// <summary>
        /// The Execute function gets as a parameter
        /// an object parameter for the action.
        /// <param name="parameter">object parameter for the action.</para>
        /// </summary>
        public void Execute(object parameter)
        {
            T param = parameter as T;
            if (param == null)
            {
                throw new Exception("Parameter isn't of type " + typeof(T).ToString() + " !!!!");
            }
            _action(param);

        }
    }
}
