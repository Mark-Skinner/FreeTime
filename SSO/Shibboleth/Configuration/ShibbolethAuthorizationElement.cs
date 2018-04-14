using System.Configuration;

using Utilities.Configuration;

namespace SSO.Shibboleth.Configuration
{
    class ShibbolethAuthorizationElement : ConfigurationHeaderElement
    {
        [ConfigurationProperty("id", IsRequired = true, DefaultValue = "")]
        public string ID
        {
            get { return this["id"] as string; }
        }

        [ConfigurationProperty("identifier", IsRequired = false, DefaultValue = false)]
        public bool Identifier
        {
            get { return (bool)this["identifier"]; }
        }

        [ConfigurationProperty("required", IsRequired = false, DefaultValue = false)]
        public bool Required
        {
            get { return (bool)this["required"]; }
        }
    }
}
