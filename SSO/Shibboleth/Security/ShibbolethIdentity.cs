using System;

using Utilities.Security;

namespace SSO.Shibboleth.Security
{
    [Serializable]
    public class ShibbolethIdentity : SSOIdentity
    {
        public override string AuthenticationType { get { return "Shibboleth Authentication"; } }
    }
}
