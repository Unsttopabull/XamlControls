using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Frost.XamlControls.Commands {

    public class RelayCommand<T> : ICommand<T> {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        /// <summary>Creates a new command.</summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null) {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute((T) parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
            _execute((T) parameter);
        }

        #endregion // ICommand Members
    }

    public class RelayCommand : RelayCommand<object> {

        public RelayCommand(Action execute) : base(o => execute()){
        }

        public RelayCommand(Action execute, Predicate<object> canExecute) : base(o => execute(), canExecute) {
            
        }
    }
}