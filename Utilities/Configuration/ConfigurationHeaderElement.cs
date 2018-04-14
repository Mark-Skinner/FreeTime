using System.Configuration;

namespace Utilities.Configuration
{
    public class ConfigurationHeaderElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }
    }
}
