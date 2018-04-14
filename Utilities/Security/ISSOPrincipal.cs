using System.Collections.Generic;
using System.Security.Principal;

namespace Utilities.Security
{
    public interface ISSOPrincipal : IPrincipal
    {
        void Initialize(IDictionary<string, string> Headers);
    }
}
