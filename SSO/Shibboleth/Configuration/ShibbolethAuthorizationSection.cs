using System.Configuration;

using Utilities.Configuration;

namespace SSO.Shibboleth.Configuration
{
    class ShibbolethAuthorizationSection : ConfigurationSection
    {
        [ConfigurationProperty("headers", IsRequired = true)]
        [ConfigurationCollection(typeof(ShibbolethAuthorizationElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ConfigurationElementCollection<ShibbolethAuthorizationElement> SupportedHeaders
        {
            get { return (ConfigurationElementCollection<ShibbolethAuthorizationElement>)this["headers"]; }
        }
    }
}
