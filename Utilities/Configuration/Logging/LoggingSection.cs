using System.Configuration;

namespace Utilities.Configuration.Logging
{
    public class LoggingSection : ConfigurationSection
    {
        [ConfigurationProperty("loggingLevel", IsRequired = false, DefaultValue = 1)]
        public int LoggingLevel
        {
            get { return (int)this["loggingLevel"]; }
        }

        [ConfigurationProperty("logging", IsRequired = true)]
        [ConfigurationCollection(typeof(LoggingElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ConfigurationElementCollection<LoggingElement> Logging
        {
            get { return (ConfigurationElementCollection<LoggingElement>)this["logging"]; }
        }
    }
}
