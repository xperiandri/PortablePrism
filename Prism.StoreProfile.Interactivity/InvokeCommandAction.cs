//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
#if NETFX_CORE
using Windows.UI.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#else
using System.Windows.Controls;
using System.Windows.Interactivity;
#endif

namespace Microsoft.Practices.Prism.Interactivity
{
    /// <summary>
    /// Trigger action that executes a command when invoked. 
    /// It also maintains the Enabled state of the target control based on the CanExecute method of the command.
    /// </summary>
    public class InvokeCommandAction :
#if SILVERLIGHT || NETFX_CORE
        TriggerAction<Control>
#else
        TriggerAction<UIElement>
#endif
    {
        /// <summary>
        /// Dependency property identifying the command to execute when invoked.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(InvokeCommandAction),
            new PropertyMetadata(null, (d, e) => ((InvokeCommandAction)d).OnCommandChanged((ICommand)e.NewValue)));

        /// <summary>
        /// Dependency property identifying the command parameter to supply on command execution.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(InvokeCommandAction),
            new PropertyMetadata(null, (d, e) => ((InvokeCommandAction)d).OnCommandParameterChanged((object)e.NewValue)));

        private ExecutableCommandBehavior commandBehavior;

        /// <summary>
        /// Gets or sets the command to execute when invoked.
        /// </summary>
        public ICommand Command
        {
            get { return this.GetValue(CommandProperty) as ICommand; }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command parameter to supply on command execution.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Public wrapper of the Invoke method. No parameter is passed here, as the parameter in the Invoke method is ignored in this class.
        /// </summary>
        public void Invoke()
        {
            this.Invoke(null);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="unusedParameter">This parameter is ignored; the CommandParameter specified in the CommandParameterProperty is used for command invocation.</param>
        protected override void Invoke(object unusedParameter)
        {
            var behavior = this.GetOrCreateBehavior();

            if (behavior != null)
            {
                behavior.ExecuteCommand();
            }
        }

        /// <summary>
        /// Sets the Command and CommandParameter properties to null.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.Command = null;
            this.CommandParameter = null;

            this.commandBehavior = null;
        }

        /// <summary>
        /// This method is called after the behavior is attached.
        /// It updates the command behavior's Command and CommandParameter properties if necessary.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            // In case this action is attached to a target object after the Command and/or CommandParameter properties are set,
            // the command behavior would be created without a value for these properties.
            // To cover this scenario, the Command and CommandParameter properties of the behavior are updated here.
            var behavior = this.GetOrCreateBehavior();

            if (behavior.Command != this.Command)
            {
                behavior.Command = this.Command;
            }

            if (behavior.CommandParameter != this.CommandParameter)
            {
                behavior.CommandParameter = this.CommandParameter;
            }
        }

        private void OnCommandChanged(ICommand newValue)
        {
            var behavior = this.GetOrCreateBehavior();

            if (behavior != null)
            {
                behavior.Command = newValue;
            }
        }

        private void OnCommandParameterChanged(object newValue)
        {
            var behavior = this.GetOrCreateBehavior();

            if (behavior != null)
            {
                behavior.CommandParameter = newValue;
            }
        }

        private ExecutableCommandBehavior GetOrCreateBehavior()
        {
            // In case this method is called prior to this action being attached, 
            // the CommandBehavior would always keep a null target object (which isn't changeable afterwards).
            // Therefore, in that case the behavior shouldn't be created and this method should return null.
            if (this.commandBehavior == null && this.AssociatedObject != null)
            {
                this.commandBehavior = new ExecutableCommandBehavior(this.AssociatedObject);
            }

            return this.commandBehavior;
        }

        /// <summary>
        /// A CommandBehavior that exposes a public ExecuteCommand method. It provides the functionality to invoke commands and update Enabled state of the target control.
        /// It is not possible to make the <see cref="InvokeCommandAction"/> inherit from <see cref="CommandBehaviorBase{T}"/>, since the <see cref="InvokeCommandAction"/>
        /// must already inherit from <see cref="TriggerAction{T}"/>, so we chose to follow the aggregation approach.
        /// </summary>
        private class ExecutableCommandBehavior :
#if SILVERLIGHT || NETFX_CORE
            CommandBehaviorBase<Control>
#else
            CommandBehaviorBase<UIElement>
#endif
        {
            /// <summary>
            /// Constructor specifying the target object.
            /// </summary>
            /// <param name="target">The target object the behavior is attached to.</param>
#if SILVERLIGHT || NETFX_CORE
            public ExecutableCommandBehavior(Control target)
#else
            public ExecutableCommandBehavior(UIElement target)
#endif
                : base(target)
            {
            }

            /// <summary>
            /// Executes the command, if it's set, providing the <see cref="CommandParameter"/>
            /// </summary>
            public new void ExecuteCommand()
            {
                base.ExecuteCommand();
            }
        }
    }
}