using System.Collections.Generic;
using System.Security.Principal;
using System.Web;

namespace SSO.Shibboleth.Security
{
    public class ShibbolethPrincipal : IShibbolethPrincipal
    {
        private IShibbolethAuthenticationHandler _authentication_handler;
        private ShibbolethIdentity _identity;

        public ShibbolethPrincipal(IShibbolethAuthenticationHandler Handler)
        {
            _authentication_handler = Handler;
        }

        public void Initialize(IDictionary<string, string> Headers)
        {
            var session = HttpContext.Current.Session;

            string IdentityKey = _authentication_handler.GetIdentifier();
            ShibbolethIdentity shib_identity = null;
            if (string.IsNullOrEmpty(IdentityKey))
                _authentication_handler.OnError("An identifier was not provided by the authentication handler, therefore the identity could not be stored in session.");
            else
                shib_identity = session[IdentityKey] as ShibbolethIdentity;
            
            if (shib_identity == null)
            {
                shib_identity = _authentication_handler.Authenticate(Headers) as ShibbolethIdentity;
                if (!string.IsNullOrEmpty(IdentityKey))
                    session[IdentityKey] = shib_identity;
            }
            
            _identity = shib_identity;
        }

        public IIdentity Identity
        {
            get { return _identity as ShibbolethIdentity; }
            set { _identity = value as ShibbolethIdentity; }
        }

        public bool IsInRole(string Role)
        {
            if (_identity.Roles == null)
                return false;

            foreach (string role in _identity.Roles)
                if (string.Equals(role, Role, System.StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }
    }
}
