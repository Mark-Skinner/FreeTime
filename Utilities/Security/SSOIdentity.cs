using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Utilities.Security
{
    [Serializable]
    public class SSOIdentity : IIdentity
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string NetworkId { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public bool IsAuthenticated { get; set; }
        public virtual string AuthenticationType { get { return "SSO Authentication"; } }
    }
}
