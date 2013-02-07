using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Microsoft.Practices.Prism.Heplers
{
    internal static class ModuleInitializer
    {
        public static void Initialize()
        {
            InitializeCompositePresentationEvent();
            InitializeWeakEventHandlerManager();
        }

        private static void InitializeCompositePresentationEvent()
        {
            var dispatcher = new Lazy<IDispatcherFacade>(() => new DefaultDispatcher());
            EventBase.InitializeDispatcher(dispatcher);
        }

        private static void InitializeWeakEventHandlerManager()
        {
            EventHandlerManager.Current = new WeakEventHandlerManager();
        }
    }
}