using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Lifecycle
{
    public interface ILifetimeAware
    {
        // *** Methods ***

        Task OnExiting();
        Task OnResuming();
        Task OnSuspending();
    }
}
