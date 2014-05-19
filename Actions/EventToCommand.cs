using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

// modified from http://weblogs.asp.net/alexeyzakharov/archive/2010/03/24/silverlight-commands-hacks-passing-eventargs-as-commandparameter-to-delegatecommand-triggered-by-eventtrigger.aspx

namespace Frost.XamlControls.Actions {

    public sealed class EventToCommand : TriggerAction<DependencyObject> {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(default(ICommand)));
        private static Type _commandType;

        public ICommand Command {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public string CommandName { get; set; }

        protected override void Invoke(object parameter) {
            if (AssociatedObject == null) {
                return;
            }

            ICommand command = Command ?? ResolveCommand();
            if (command == null || !command.CanExecute(parameter)) {
                return;
            }
            
            command.Execute(parameter);
        }

        private ICommand ResolveCommand() {
            ICommand command = null;
            FrameworkElement frameworkElement = AssociatedObject as FrameworkElement;
            if (frameworkElement == null) {
                return null;
            }

            object dataContext = frameworkElement.DataContext;
            if (dataContext == null) {
                return null;
            }

            if (_commandType == null) {
                _commandType = typeof(ICommand);
            }

            PropertyInfo commandPropertyInfo = 
                dataContext.GetType()
                           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .FirstOrDefault(p => string.Equals(p.Name, CommandName, StringComparison.Ordinal) && _commandType.IsAssignableFrom(p.PropertyType)
                );

            if (commandPropertyInfo != null) {
                command = (ICommand) commandPropertyInfo.GetValue(dataContext, null);
            }
            return command;
        }
    }

}