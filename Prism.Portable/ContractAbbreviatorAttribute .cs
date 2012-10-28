using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism
{
    [AttributeUsageAttribute(AttributeTargets.Method, AllowMultiple = false)]
    [ConditionalAttribute("CONTRACTS_FULL")]
    internal sealed class ContractAbbreviatorAttribute : Attribute
    {
    }
}