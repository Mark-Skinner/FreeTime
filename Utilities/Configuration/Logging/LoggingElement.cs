using System.Configuration;

using Utilities.Logging;

namespace Utilities.Configuration.Logging
{
    public class LoggingElement : ConfigurationHeaderElement
    {
        [ConfigurationProperty("filePath", IsRequired = false)]
        public string LogFilePath
        {
            get { return this["filePath"] as string; }
        }

        [ConfigurationProperty("fileName", IsRequired = false, DefaultValue = "log")]
        public string LogFileName
        {
            get { return this["fileName"] as string; }
        }

        [ConfigurationProperty("interval", IsRequired = false, DefaultValue = LoggingInterval.Day)]
        public LoggingInterval Interval
        {
            get { return (LoggingInterval)this["interval"]; }
        }

        [ConfigurationProperty("severity", IsRequired = false, DefaultValue = ErrorSeverity.Critical)]
        public ErrorSeverity Severity
        {
            get { return (ErrorSeverity)this["severity"]; }
        }

        [ConfigurationProperty("loggingType", IsRequired = false, DefaultValue = LoggingType.TextFile)]
        public LoggingType LoggingType
        {
            get { return (LoggingType)this["loggingType"]; }
        }

        [ConfigurationProperty("enterprisePolicy", IsRequired = false)]
        public string EnterprisePolicy
        {
            get { return this["enterprisePolicy"] as string; }
        }
    }
}
