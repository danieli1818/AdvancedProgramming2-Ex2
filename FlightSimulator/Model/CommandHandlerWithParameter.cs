using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.Model
{

    class CommandHandlerWithParameter<T> : ICommand where T : class
    {
        public delegate void ActionFunc(T param);
        private ActionFunc _action;

        public event EventHandler CanExecuteChanged;

        public CommandHandlerWithParameter(ActionFunc action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            T param = parameter as T;
            return param != null;
        }

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
