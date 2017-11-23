using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Indu
{
    /// <summary>
    /// Idee: Da die Command Klassen sich nur in der Execute Methode unterscheiden, geben wir sie
    /// einfach im Konstruktor mit.
    /// Diese Methode bekommt ein object als Parameter und gibt nichts zurück (void). Daher ist dies
    /// eine Instanz des generischen Action<object> Delegates.
    /// </summary>
    class RelayCommand : ICommand
    {
        private Action<object> execMethod;                // Wird in Exec aufgerufen.
        private Func<object, bool> canExecMethod;
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Setzt die Methode, die in Exec aufgerufen werden soll.
        /// </summary>
        /// <param name="execMethod">Eine (anonyme) Methode, die ein object als Parameter
        /// bekommt und nichts zurückgibt.</param>
        public RelayCommand(Action<object> execMethod) : this(execMethod, null)
        { }
        public RelayCommand(Action<object> execMethod, Func<object, bool> canExecMethod)
        {
            // Ist die execMethod null, wird nichts gemacht.
            this.execMethod = execMethod ?? (o => { });
            // Ist die canExecMethod null, dann wird eine Methode verwendet, die immer true liefert.
            this.canExecMethod = canExecMethod ?? (o => true);
        }
        public bool CanExecute(object parameter)
        {
            return canExecMethod(parameter);
        }

        public void Execute(object parameter)
        {
            execMethod(parameter);
        }
    }
}
