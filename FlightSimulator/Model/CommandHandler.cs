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
    /// The CommandHandler class implements ICommand Interface
    /// handles a command.
    /// </summary>
    public class CommandHandler : ICommand
    {
        /// <summary>
        /// The Action _action member of the class of the Action
        /// which should run when executing the command.
        /// </summary>
        private Action _action;

        /// <summary>
        /// The CommandHandler Constructor.
        /// It gets as a parameter an Action action
        /// and sets it as the command's action.
        /// <param name="action">String command to send to the simulator.</para>
        /// </summary>
        public CommandHandler(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// The CanExecutre function gets as a parameter
        /// an object parameter and returns true that this command
        /// can be executed.
        /// <param name="parameter">object parameter.</para>
        /// <retValue>true that this command can be executed.</retValue>
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// The CanExecuteChanged EventHandler event.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// The Execute function gets as a parameter
        /// an object parameter and executes the action.
        /// <param name="parameter">object parameter</para>
        /// </summary>
        public void Execute(object parameter)
        {
            _action();
        }
    }
}
