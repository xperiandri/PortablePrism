using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.Properties
{
    internal static class ResourceHelper
    {
        private readonly static ResourceLoader _resourceLoader;
        static ResourceHelper()
        {
            _resourceLoader = new ResourceLoader("Microsoft.Practices.Prism.StoreProfile/Resources");
        }

        /// <summary>
        ///   Looks up a localized string similar to The object must be of type &apos;{0}&apos; in order to use the current region adapter..
        /// </summary>
        internal static string AdapterInvalidTypeException
        {
            get
            {
                return _resourceLoader.GetString("AdapterInvalidTypeException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot change the region name once is set. The current region name is &apos;{0}&apos;..
        /// </summary>
        internal static string CannotChangeRegionNameException
        {
            get
            {
                return _resourceLoader.GetString("CannotChangeRegionNameException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot create navigation target &apos;{0}&apos;..
        /// </summary>
        internal static string CannotCreateNavigationTarget
        {
            get
            {
                return _resourceLoader.GetString("CannotCreateNavigationTarget");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot register a CompositeCommand in itself..
        /// </summary>
        internal static string CannotRegisterCompositeCommandInItself
        {
            get
            {
                return _resourceLoader.GetString("CannotRegisterCompositeCommandInItself");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot register the same command twice in the same CompositeCommand..
        /// </summary>
        internal static string CannotRegisterSameCommandTwice
        {
            get
            {
                return _resourceLoader.GetString("CannotRegisterSameCommandTwice");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Type &apos;{0}&apos; does not implement from IRegionBehavior..
        /// </summary>
        internal static string CanOnlyAddTypesThatInheritIFromRegionBehavior
        {
            get
            {
                return _resourceLoader.GetString("CanOnlyAddTypesThatInheritIFromRegionBehavior");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The ConfigurationStore cannot contain a null value. .
        /// </summary>
        internal static string ConfigurationStoreCannotBeNull
        {
            get
            {
                return _resourceLoader.GetString("ConfigurationStoreCannotBeNull");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to ContentControl&apos;s Content property is not empty. 
        ///    This control is being associated with a region, but the control is already bound to something else. 
        ///    If you did not explicitly set the control&apos;s Content property, 
        ///    this exception may be caused by a change in the value of the inherited RegionManager attached property..
        /// </summary>
        internal static string ContentControlHasContentException
        {
            get
            {
                return _resourceLoader.GetString("ContentControlHasContentException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to At least one cyclic dependency has been found in the module catalog. Cycles in the module dependencies must be avoided..
        /// </summary>
        internal static string CyclicDependencyFound
        {
            get
            {
                return _resourceLoader.GetString("CyclicDependencyFound");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Deactivation is not possible in this type of region..
        /// </summary>
        internal static string DeactiveNotPossibleException
        {
            get
            {
                return _resourceLoader.GetString("DeactiveNotPossibleException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to {1}: {2}. Priority: {3}. Timestamp:{0:u}..
        /// </summary>
        internal static string DefaultTextLoggerPattern
        {
            get
            {
                return _resourceLoader.GetString("DefaultTextLoggerPattern");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Neither the executeMethod nor the canExecuteMethod delegates can be null..
        /// </summary>
        internal static string DelegateCommandDelegatesCannotBeNull
        {
            get
            {
                return _resourceLoader.GetString("DelegateCommandDelegatesCannotBeNull");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to T for DelegateCommand&lt;T&gt; is not an object nor Nullable..
        /// </summary>
        internal static string DelegateCommandInvalidGenericPayloadType
        {
            get
            {
                return _resourceLoader.GetString("DelegateCommandInvalidGenericPayloadType");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot add dependency for unknown module {0}.
        /// </summary>
        internal static string DependencyForUnknownModule
        {
            get
            {
                return _resourceLoader.GetString("DependencyForUnknownModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A module declared a dependency on another module which is not declared to be loaded. Missing module(s): {0}.
        /// </summary>
        internal static string DependencyOnMissingModule
        {
            get
            {
                return _resourceLoader.GetString("DependencyOnMissingModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Directory {0} was not found..
        /// </summary>
        internal static string DirectoryNotFound
        {
            get
            {
                return _resourceLoader.GetString("DirectoryNotFound");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A duplicated module with name {0} has been found by the loader..
        /// </summary>
        internal static string DuplicatedModule
        {
            get
            {
                return _resourceLoader.GetString("DuplicatedModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to A duplicated module group with name {0} has been found by the loader..
        /// </summary>
        internal static string DuplicatedModuleGroup
        {
            get
            {
                return _resourceLoader.GetString("DuplicatedModuleGroup");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to retrieve the module type {0} from the loaded assemblies.  You may need to specify a more fully-qualified type name..
        /// </summary>
        internal static string FailedToGetType
        {
            get
            {
                return _resourceLoader.GetString("FailedToGetType");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An exception occurred while initializing module &apos;{0}&apos;. 
        ///    - The exception message was: {2}
        ///    - The Assembly that the module was trying to be loaded from was:{1}
        ///    Check the InnerException property of the exception for more information. If the exception occurred while creating an object in a DI container, you can exception.GetRootException() to help locate the root cause of the problem. 
        ///  .
        /// </summary>
        internal static string FailedToLoadModule
        {
            get
            {
                return _resourceLoader.GetString("FailedToLoadModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An exception occurred while initializing module &apos;{0}&apos;. 
        ///    - The exception message was: {1}
        ///    Check the InnerException property of the exception for more information. If the exception occurred 
        ///    while creating an object in a DI container, you can exception.GetRootException() to help locate the 
        ///    root cause of the problem. .
        /// </summary>
        internal static string FailedToLoadModuleNoAssemblyInfo
        {
            get
            {
                return _resourceLoader.GetString("FailedToLoadModuleNoAssemblyInfo");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Failed to load type for module {0}. 
        ///
        ///If this error occurred when using MEF in a Silverlight application, please ensure that the CopyLocal property of the reference to the MefExtensions assembly is set to true in the main application/shell and false in all other assemblies. 
        ///
        ///Error was: {1}..
        /// </summary>
        internal static string FailedToRetrieveModule
        {
            get
            {
                return _resourceLoader.GetString("FailedToRetrieveModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to HostControl cannot have null value when behavior attaches. .
        /// </summary>
        internal static string HostControlCannotBeNull
        {
            get
            {
                return _resourceLoader.GetString("HostControlCannotBeNull");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The HostControl property cannot be set after Attach method has been called..
        /// </summary>
        internal static string HostControlCannotBeSetAfterAttach
        {
            get
            {
                return _resourceLoader.GetString("HostControlCannotBeSetAfterAttach");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to HostControl type must be a TabControl..
        /// </summary>
        internal static string HostControlMustBeATabControl
        {
            get
            {
                return _resourceLoader.GetString("HostControlMustBeATabControl");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The IModuleEnumerator interface is no longer used and has been replaced by ModuleCatalog..
        /// </summary>
        internal static string IEnumeratorObsolete
        {
            get
            {
                return _resourceLoader.GetString("IEnumeratorObsolete");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The argument must be a valid absolute Uri to an assembly file..
        /// </summary>
        internal static string InvalidArgumentAssemblyUri
        {
            get
            {
                return _resourceLoader.GetString("InvalidArgumentAssemblyUri");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The Target of the IDelegateReference should be of type {0}..
        /// </summary>
        internal static string InvalidDelegateRerefenceTypeException
        {
            get
            {
                return _resourceLoader.GetString("InvalidDelegateRerefenceTypeException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to ItemsControl&apos;s ItemsSource property is not empty. 
        ///    This control is being associated with a region, but the control is already bound to something else. 
        ///    If you did not explicitly set the control&apos;s ItemSource property, 
        ///    this exception may be caused by a change in the value of the inherited RegionManager attached property..
        /// </summary>
        internal static string ItemsControlHasItemsSourceException
        {
            get
            {
                return _resourceLoader.GetString("ItemsControlHasItemsSourceException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Mapping with the given type is already registered: {0}..
        /// </summary>
        internal static string MappingExistsException
        {
            get
            {
                return _resourceLoader.GetString("MappingExistsException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Module {0} depends on other modules that don&apos;t belong to the same group..
        /// </summary>
        internal static string ModuleDependenciesNotMetInGroup
        {
            get
            {
                return _resourceLoader.GetString("ModuleDependenciesNotMetInGroup");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Module {0} was not found in the catalog..
        /// </summary>
        internal static string ModuleNotFound
        {
            get
            {
                return _resourceLoader.GetString("ModuleNotFound");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The ModulePath cannot contain a null value or be empty.
        /// </summary>
        internal static string ModulePathCannotBeNullOrEmpty
        {
            get
            {
                return _resourceLoader.GetString("ModulePathCannotBeNullOrEmpty");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Failed to load type &apos;{0}&apos; from assembly &apos;{1}&apos;..
        /// </summary>
        internal static string ModuleTypeNotFound
        {
            get
            {
                return _resourceLoader.GetString("ModuleTypeNotFound");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Navigation is already in progress on region with name &apos;{0}&apos;..
        /// </summary>
        internal static string NavigationInProgress
        {
            get
            {
                return _resourceLoader.GetString("NavigationInProgress");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Navigation cannot proceed until a region is set for the RegionNavigationService..
        /// </summary>
        internal static string NavigationServiceHasNoRegion
        {
            get
            {
                return _resourceLoader.GetString("NavigationServiceHasNoRegion");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The IRegionAdapter for the type {0} is not registered in the region adapter mappings. You can register an IRegionAdapter for this control by overriding the ConfigureRegionAdapterMappings method in the bootstrapper..
        /// </summary>
        internal static string NoRegionAdapterException
        {
            get
            {
                return _resourceLoader.GetString("NoRegionAdapterException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to There is currently no moduleTypeLoader in the ModuleManager that can retrieve the specified module..
        /// </summary>
        internal static string NoRetrieverCanRetrieveModule
        {
            get
            {
                return _resourceLoader.GetString("NoRetrieverCanRetrieveModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An exception has occurred while trying to add a view to region &apos;{0}&apos;. 
        ///    - The most likely causing exception was was: &apos;{1}&apos;.
        ///    But also check the InnerExceptions for more detail or call .GetRootException(). .
        /// </summary>
        internal static string OnViewRegisteredException
        {
            get
            {
                return _resourceLoader.GetString("OnViewRegisteredException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The member access expression does not access a property..
        /// </summary>
        internal static string PropertySupport_ExpressionNotProperty_Exception
        {
            get
            {
                return _resourceLoader.GetString("PropertySupport_ExpressionNotProperty_Exception");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The expression is not a member access expression..
        /// </summary>
        internal static string PropertySupport_NotMemberAccessExpression_Exception
        {
            get
            {
                return _resourceLoader.GetString("PropertySupport_NotMemberAccessExpression_Exception");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The referenced property is a static property..
        /// </summary>
        internal static string PropertySupport_StaticExpression_Exception
        {
            get
            {
                return _resourceLoader.GetString("PropertySupport_StaticExpression_Exception");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The Attach method cannot be called when Region property is null..
        /// </summary>
        internal static string RegionBehaviorAttachCannotBeCallWithNullRegion
        {
            get
            {
                return _resourceLoader.GetString("RegionBehaviorAttachCannotBeCallWithNullRegion");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The Region property cannot be set after Attach method has been called..
        /// </summary>
        internal static string RegionBehaviorRegionCannotBeSetAfterAttach
        {
            get
            {
                return _resourceLoader.GetString("RegionBehaviorRegionCannotBeSetAfterAttach");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An exception occurred while creating a region with name &apos;{0}&apos;. The exception was: {1}. .
        /// </summary>
        internal static string RegionCreationException
        {
            get
            {
                return _resourceLoader.GetString("RegionCreationException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The region being added already has a name of &apos;{0}&apos; and cannot be added to the region manager with a different name (&apos;{1}&apos;)..
        /// </summary>
        internal static string RegionManagerWithDifferentNameException
        {
            get
            {
                return _resourceLoader.GetString("RegionManagerWithDifferentNameException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The region name cannot be null or empty..
        /// </summary>
        internal static string RegionNameCannotBeEmptyException
        {
            get
            {
                return _resourceLoader.GetString("RegionNameCannotBeEmptyException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Region with the given name is already registered: {0}.
        /// </summary>
        internal static string RegionNameExistsException
        {
            get
            {
                return _resourceLoader.GetString("RegionNameExistsException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to This RegionManager does not contain a Region with the name &apos;{0}&apos;..
        /// </summary>
        internal static string RegionNotFound
        {
            get
            {
                return _resourceLoader.GetString("RegionNotFound");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The region manager does not contain the {0} region..
        /// </summary>
        internal static string RegionNotInRegionManagerException
        {
            get
            {
                return _resourceLoader.GetString("RegionNotInRegionManagerException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to View already exists in region..
        /// </summary>
        internal static string RegionViewExistsException
        {
            get
            {
                return _resourceLoader.GetString("RegionViewExistsException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to View with name &apos;{0}&apos; already exists in the region..
        /// </summary>
        internal static string RegionViewNameExistsException
        {
            get
            {
                return _resourceLoader.GetString("RegionViewNameExistsException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Module {0} is marked for automatic initialization when the application starts, but it depends on modules that are marked as OnDemand initialization. To fix this error, mark the dependency modules for InitializationMode=WhenAvailable, or remove this validation by extending the ModuleCatalog class..
        /// </summary>
        internal static string StartupModuleDependsOnAnOnDemandModule
        {
            get
            {
                return _resourceLoader.GetString("StartupModuleDependsOnAnOnDemandModule");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The provided String argument {0} must not be null or empty..
        /// </summary>
        internal static string StringCannotBeNullOrEmpty
        {
            get
            {
                return _resourceLoader.GetString("StringCannotBeNullOrEmpty");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The provided String argument {0} must not be null or empty..
        /// </summary>
        internal static string StringCannotBeNullOrEmpty1
        {
            get
            {
                return _resourceLoader.GetString("StringCannotBeNullOrEmpty1");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to No BehaviorType with key &apos;{0}&apos; was registered..
        /// </summary>
        internal static string TypeWithKeyNotRegistered
        {
            get
            {
                return _resourceLoader.GetString("TypeWithKeyNotRegistered");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An exception occurred while trying to create region objects. 
        ///    - The most likely causing exception was: &apos;{0}&apos;.
        ///    But also check the InnerExceptions for more detail or call .GetRootException(). .
        /// </summary>
        internal static string UpdateRegionException
        {
            get
            {
                return _resourceLoader.GetString("UpdateRegionException");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The value must be of type ModuleInfo..
        /// </summary>
        internal static string ValueMustBeOfTypeModuleInfo
        {
            get
            {
                return _resourceLoader.GetString("ValueMustBeOfTypeModuleInfo");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to {0} not found..
        /// </summary>
        internal static string ValueNotFound
        {
            get
            {
                return _resourceLoader.GetString("ValueNotFound");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The region does not contain the specified view..
        /// </summary>
        internal static string ViewNotInRegionException
        {
            get
            {
                return _resourceLoader.GetString("ViewNotInRegionException");
            }
        }     
    }
}

