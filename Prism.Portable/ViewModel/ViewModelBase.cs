using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Practices.Prism.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        private static Func<bool> designModeAccessor;

        public static bool IsDesignModeAccessorSet
        {
            get
            {
                return designModeAccessor != null;
            }
        }

        protected static bool IsInDesignMode
        {
            get
            {
                return designModeAccessor();
            }
            
        }

        //static ViewModelBase()
        //{
        //    if (!IsDesignModeAccessorSet)
        //        SetDesignModeAccessor(GetDesignModeAccessor());
        //}

        //private static Func<bool> GetDesignModeAccessor()
        //{
        //    // We check Silverlight first because when in the VS designer, the .NET libraries will resolve
        //    // If we can resolve the SL libs, then we're in SL or WP
        //    // Then we check .NET because .NET will load the WinRT library (even though it can't really run it)
        //    // When running in WinRT, it will not load the PresentationFramework lib

        //    // Check Silverlight
        //    var dm = Type.GetType("System.ComponentModel.DesignerProperties, System.Windows");
        //    if (dm != null)
        //    {
        //        var mi = dm.GetProperty("IsInDesignTool").GetGetMethod();
        //        return ()=> ((bool) mi.Invoke(null, new object[0]));
        //    }

        //    // Check .NET 
        //    //var cmdm = Type.GetType("System.ComponentModel.DesignerProperties, PresentationFramework");
        //    //if (cmdm != null) // loaded the assembly, could be .net 
        //    //{
        //    //    var mi = dm.GetMethod("GetIsInDesignMode");
        //    //    return ()=> ((bool) mi.Invoke(null, new object[] {}));
        //    //}

        //    // check WinRT next
        //    var wadm = Type.GetType("Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime");
        //    if (wadm != null)
        //    {
        //        var mi = dm.GetProperty("DesignModeEnabled").GetGetMethod();
        //        return ()=> ((bool) mi.Invoke(null, new object[0]));
        //    }

        //    throw new InvalidOperationException();
        //}

        public static void SetDesignModeAccessor(Func<bool> accessor)
        {
            Contract.Requires<InvalidOperationException>(!IsDesignModeAccessorSet);
            designModeAccessor = accessor;
        }
        
    }
}
