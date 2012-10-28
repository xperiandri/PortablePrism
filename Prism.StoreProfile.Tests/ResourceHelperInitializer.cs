using Microsoft.Practices.Prism.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prism.StoreProfile.Tests
{
    internal static class ResourceHelperInitializer
    {
        public static void Initialize()
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(ResourceHelper).TypeHandle);
        }
    }
}
