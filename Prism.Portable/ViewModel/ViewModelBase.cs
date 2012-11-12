using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Prism.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        private readonly static Lazy<bool> isInDesignMode = new Lazy<bool>(GetIsInDesignMode);

        protected static bool IsInDesignMode
        {
            get
            {
                return isInDesignMode.Value;
            }
        }

        private static bool GetIsInDesignMode()
        {
            // As a portable lib, we need see what framework we're runnign on
            // and use reflection to get the designer value

            var platform = DesignerLibrary.DetectedDesignerLibrary;

            if (platform == DesignerPlatformLibrary.WinRT)
                return IsInDesignModeWinRT();

            if (platform == DesignerPlatformLibrary.Silverlight)
            {
                var desMode = IsInDesignModeSilverlight();
                if (!desMode)
                    desMode = IsInDesignModeNet(); // hard to tell these apart in the designer

                return desMode;
            }

            if (platform == DesignerPlatformLibrary.Net)
                return IsInDesignModeNet();

            return false;
        }

        private static bool IsInDesignModeSilverlight()
        {
            try
            {
                var dm = Type.GetType("System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");

                var dme = dm.GetProperty("IsInDesignTool", BindingFlags.Public | BindingFlags.Static);
                return (bool)dme.GetValue(null, null);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsInDesignModeWinRT()
        {
            try
            {
                var dm = Type.GetType("Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime");

                var dme = dm.GetProperty("DesignModeEnabled", BindingFlags.Static | BindingFlags.Public);
                return (bool)dme.GetValue(null, null);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsInDesignModeNet()
        {
            try
            {
                var dm =
                    Type.GetType(
                        "System.ComponentModel.DesignerProperties, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                var dmp = dm.GetField("IsInDesignModeProperty", BindingFlags.Public | BindingFlags.Static).GetValue(null);

                var dpd =
                    Type.GetType(
                        "System.ComponentModel.DependencyPropertyDescriptor, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                var typeFe =
                    Type.GetType("System.Windows.FrameworkElement, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                var fromPropertys = dpd.GetMethods(BindingFlags.Public | BindingFlags.Static);
                var fromProperty = fromPropertys.Single(mi => mi.Name == "FromProperty" && mi.GetParameters().Length == 2);

                var descriptor = fromProperty.Invoke(null, new object[] { dmp, typeFe });

                var metaProp = dpd.GetProperty("Metadata", BindingFlags.Public | BindingFlags.Instance);

                var metadata = metaProp.GetValue(descriptor, null);

                var tPropMeta = Type.GetType("System.Windows.PropertyMetadata, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

                var dvProp = tPropMeta.GetProperty("DefaultValue", BindingFlags.Public | BindingFlags.Instance);

                var dv = (bool)dvProp.GetValue(metadata, null);

                return dv;
            }
            catch
            {
                return false;
            }
        }
    }
}