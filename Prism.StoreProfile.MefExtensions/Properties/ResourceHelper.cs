using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.MefExtensions.Properties
{
    internal static class ResourceHelper
    {
        private readonly static ResourceLoader _resourceLoader;
        static ResourceHelper()
        {
            _resourceLoader = new ResourceLoader("Microsoft.Practices.Prism.StoreProfile.MefExtensions/Resources");
        }

        /// <summary>
        ///   Looks up a localized string similar to Bootstrapper sequence completed.
        /// </summary>
        internal static string BootstrapperSequenceCompleted
        {
            get
            {
                return _resourceLoader.GetString("BootstrapperSequenceCompleted");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Configuring catalog for MEF.
        /// </summary>
        internal static string ConfiguringCatalogForMEF
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringCatalogForMEF");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Configuring default region behaviors.
        /// </summary>
        internal static string ConfiguringDefaultRegionBehaviors
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringDefaultRegionBehaviors");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Configuring MEF container.
        /// </summary>
        internal static string ConfiguringMefContainer
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringMefContainer");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Configuring module catalog..
        /// </summary>
        internal static string ConfiguringModuleCatalog
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringModuleCatalog");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Configuring region adapters.
        /// </summary>
        internal static string ConfiguringRegionAdapters
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringRegionAdapters");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Configuring ServiceLocator singleton.
        /// </summary>
        internal static string ConfiguringServiceLocatorSingleton
        {
            get
            {
                return _resourceLoader.GetString("ConfiguringServiceLocatorSingleton");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Creating catalog for MEF.
        /// </summary>
        internal static string CreatingCatalogForMEF
        {
            get
            {
                return _resourceLoader.GetString("CreatingCatalogForMEF");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Creating Mef container.
        /// </summary>
        internal static string CreatingMefContainer
        {
            get
            {
                return _resourceLoader.GetString("CreatingMefContainer");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Creating module catalog..
        /// </summary>
        internal static string CreatingModuleCatalog
        {
            get
            {
                return _resourceLoader.GetString("CreatingModuleCatalog");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Creating shell.
        /// </summary>
        internal static string CreatingShell
        {
            get
            {
                return _resourceLoader.GetString("CreatingShell");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to locate the module with type &apos;{0}&apos; among the exported modules. Make sure the module name in the module catalog matches that specified on ModuleExportAttribute for the module type..
        /// </summary>
        internal static string FailedToGetType
        {
            get
            {
                return _resourceLoader.GetString("FailedToGetType");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Initializing modules.
        /// </summary>
        internal static string InitializingModules
        {
            get
            {
                return _resourceLoader.GetString("InitializingModules");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Initializing shell.
        /// </summary>
        internal static string InitializingShell
        {
            get
            {
                return _resourceLoader.GetString("InitializingShell");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Logger was created successfully..
        /// </summary>
        internal static string LoggerWasCreatedSuccessfully
        {
            get
            {
                return _resourceLoader.GetString("LoggerWasCreatedSuccessfully");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The CompositionHost is required and cannot be null..
        /// </summary>
        internal static string NullCompositionHostException
        {
            get
            {
                return _resourceLoader.GetString("NullCompositionHostException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The ILoggerFacade is required and cannot be null..
        /// </summary>
        internal static string NullLoggerFacadeException
        {
            get
            {
                return _resourceLoader.GetString("NullLoggerFacadeException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The IModuleCatalog is required and cannot be null in order to initialize the modules..
        /// </summary>
        internal static string NullModuleCatalogException
        {
            get
            {
                return _resourceLoader.GetString("NullModuleCatalogException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Registering Framework Exception Types.
        /// </summary>
        internal static string RegisteringFrameworkExceptionTypes
        {
            get
            {
                return _resourceLoader.GetString("RegisteringFrameworkExceptionTypes");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Setting the RegionManager..
        /// </summary>
        internal static string SettingTheRegionManager
        {
            get
            {
                return _resourceLoader.GetString("SettingTheRegionManager");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Updating Regions..
        /// </summary>
        internal static string UpdatingRegions
        {
            get
            {
                return _resourceLoader.GetString("UpdatingRegions");
            }
        }
    
    }
}

