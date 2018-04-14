using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

using Utilities.Configuration.Logging;

// for ExceptionPolicy
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using EnterpriseLogging = Microsoft.Practices.EnterpriseLibrary.Logging;
using EnterpriseExceptionLogging = Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;

namespace Utilities.Logging
{
    /// <summary>
    /// Represents a multi-functional logging utility that may be
    /// used to write messages to different file locations and types.
    /// This class may not be inherited.
    /// </summary>
    public sealed class Logger : ILogger
    {
        #region Fields

        /// <summary>
        /// The single instance of the <see cref="Logger"/> class that can be referenced through the <seealso cref="Instance"/> property.
        /// </summary>
        private static readonly Logger _logger = new Logger();
        
        /// <summary>
        /// List of instances for enterprise logging loggers.
        /// </summary>
        private readonly Dictionary<string, EnterpriseExceptionLogging.LoggingExceptionHandler> _temp_enterprise_loggers;

        /// <summary>
        /// The default name used for the the default enterprise logger.
        /// </summary>
        private const string DEFAULT_ENTERPRISE_LOGGER_NAME = "Default_Enterprise_Logger";

        /// <summary>
        /// The default Web.config AppSettings key that is used to obtain a default log file path.
        /// </summary>
        private const string DEFAULT_CONFIG_FILE_PATH = "LogFilePath";

        /// <summary>
        /// The default Web.config AppSettings key that is used to obtain a default logging level for the current environment.
        /// </summary>
        private const string DEFAULT_CONFIG_SEVERITY_TO_LOG = "LoggingSeverity";

        /// <summary>
        /// The default policy name as a last resort if no other Enterprise Library Policies were found and the default logger was not created.
        /// </summary>
        private const string DEFAULT_EXCEPTION_POLICY = "Exception Policy";

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor to utilize singleton pattern.
        /// </summary>
        private Logger()
        {
            _temp_enterprise_loggers = new Dictionary<string, EnterpriseExceptionLogging.LoggingExceptionHandler>();
            _temp_enterprise_loggers.Add(DEFAULT_ENTERPRISE_LOGGER_NAME, CreateTempLogger(Name: DEFAULT_ENTERPRISE_LOGGER_NAME, Save: false));
        }

        #endregion

        #region Properties

        /// <summary>
        /// An instance that provides access to the
        /// logging capabilities of the Logger class.
        /// </summary>
        public static Logger Instance { get { return _logger; } }

        #endregion

        #region Methods

        /// <summary>
        /// Logs the error message accordingly to the provided <paramref name="LoggingType"/>
        /// and <paramref name="ErrorSeverity"/>. If the <paramref name="ErrorSeverity"/> is
        /// <see cref="ErrorSeverity.Critical"/>, the error will always be logged using the
        /// logging type <see cref="LoggingType.EnterpriseLibrary"/>.
        /// </summary>
        /// <param name="ErrorMessage">The message to log.</param>
        /// <param name="LoggingType">The type of logging to perform.</param>
        /// <param name="ErrorSeverity">The severity of the error.</param>
        public void Log(string ErrorMessage, LoggingType LoggingType = LoggingType.EnterpriseLibrary, ErrorSeverity ErrorSeverity = ErrorSeverity.Critical)
        {
            // if the severity includes critical, add exception policy to the logging type automatically
            if (ErrorSeverity.HasFlag(ErrorSeverity.Critical))
                LoggingType |= LoggingType.EnterpriseLibrary;

            // Log the error
            Log(new Exception(ErrorMessage), LoggingType, ErrorSeverity);
        }

        /// <summary>
        /// Logs the exception accordingly to the provided <paramref name="LoggingType"/>
        /// and <paramref name="ErrorSeverity"/>. If the <paramref name="ErrorSeverity"/> is
        /// <see cref="ErrorSeverity.Critical"/>, the error will always be logged using the
        /// logging type <see cref="LoggingType.EnterpriseLibrary"/>.
        /// </summary>
        /// <param name="Exception">The exception to log.</param>
        /// <param name="LoggingType">The type of logging to perform.</param>
        /// <param name="ErrorSeverity">The severity of the error.</param>
        public void Log(Exception Exception, LoggingType LoggingType = LoggingType.EnterpriseLibrary, ErrorSeverity ErrorSeverity = ErrorSeverity.Critical)
        {
            LoggingSection LoggingSection = null;
            try { LoggingSection = ConfigurationManager.GetSection("Logging") as LoggingSection; }
            catch (ConfigurationErrorsException) { LoggingSection = null; }

            bool UseLoggingConfig = (LoggingSection != null) ? LoggingSection.Logging.Count > 0 : false;

            // If the key in the web config doesn't exist, by default only log critical errors
            ErrorSeverity SeverityToLog = (LoggingSection == null) ? ErrorSeverity.Critical : (ErrorSeverity)LoggingSection.LoggingLevel;

            if (LoggingSection == null)
            {
                try
                {
                    // try to use default value from web config
                    string Severity = ConfigurationManager.AppSettings[DEFAULT_CONFIG_SEVERITY_TO_LOG];

                    // If the value in the web config is not null or empty..
                    if (!string.IsNullOrEmpty(Severity))
                    {
                        // Try to parse it into an int.
                        int num;
                        if (int.TryParse(Severity, out num))
                            SeverityToLog = (ErrorSeverity)num; // cast it as ErrorSeverity enumeration value
                                                                // NOTE: If the enumeration ErrorSeverity does not contain the value you are
                                                                // trying to cast it as, it will simply stay as an int with that value (that's called magic).
                    }
                }
                catch (ConfigurationErrorsException) { }
            }

            // Check web config (or default) value to see if we care about logging the error.
            if (SeverityToLog.HasFlag(ErrorSeverity))
            {
                // Log the error using the Exception Policy described in the web config
                if (LoggingType.HasFlag(LoggingType.EnterpriseLibrary))
                    WriteToEnterpriseLibraryFile(Exception, ErrorSeverity, UseLoggingConfig);

                // Log the error using the text file path provided in the web config
                // NOTE: The message is prepended with the severity level for easy lookup
                if (LoggingType.HasFlag(LoggingType.TextFile))
                    WriteToTextFile(ErrorSeverity.ToString().ToUpper() + ": " + Exception.Message, ErrorSeverity, UseLoggingConfig);
            }
        }
        
        /// <summary>
        /// Logs the error message according to the information given
        /// by the logging configuration with the provided <paramref name="LoggingName"/>.
        /// </summary>
        /// <param name="ErrorMessage">The message to log.</param>
        /// <param name="LoggingName">The name of the  logging configuration in the Logging section of the web config.</param>
        public void Log(string ErrorMessage, string LoggingName)
        {
            List<LoggingElement> LoggingElements = GetLoggingElementsByPropertyValue("Name", LoggingName);
            if (LoggingElements.Count != 1)
                return;

            Log(ErrorMessage, LoggingElements[0].LoggingType, LoggingElements[0].Severity);
        }

        /// <summary>
        /// Logs the exception according to the information given
        /// by the logging configuration with the provided <paramref name="LoggingName"/>.
        /// </summary>
        /// <param name="Exception">The exception to log.</param>
        /// <param name="LoggingName">The name of the  logging configuration in the Logging section of the web config.</param>
        public void Log(Exception Exception, string LoggingName)
        {
            List<LoggingElement> LoggingElements = GetLoggingElementsByPropertyValue("Name", LoggingName);
            if (LoggingElements.Count != 1)
                return;

            Log(Exception, LoggingElements[0].LoggingType, LoggingElements[0].Severity);
        }

        /// <summary>
        /// Logs the provided <paramref name="Exception"/> based on the <paramref name="ErrorSeverity"/>.
        /// If <paramref name="UseLoggingConfig"/> is true, the  logging configuration will
        /// attempt to be used to log the <paramref name="Exception"/>.
        /// </summary>
        /// <param name="Message">The message to log.</param>
        /// <param name="ErrorSeverity">The severity level of the message.</param>
        /// <param name="UseLoggingConfig">States whether or not to use the custom  logging configuration.</param>
        private void WriteToTextFile(string Message, ErrorSeverity ErrorSeverity, bool UseLoggingConfig = true)
        {
            string LogFilePath = string.Empty;
            List<LoggingElement> LoggingElements = (UseLoggingConfig) ? GetLoggingElementsByPropertyValue("Severity", ErrorSeverity) : new List<LoggingElement>(0);
            
            int count = 0;
            // loop through each and log them all to the corresponding files
            foreach (LoggingElement LoggingElement in LoggingElements)
            {
                // but remember, only log to files if their logging type
                // is designated as TextFile
                if (!LoggingElement.LoggingType.HasFlag(LoggingType.TextFile))
                    continue;
                
                LogFilePath = LoggingElement.LogFilePath;
                if (string.IsNullOrEmpty(LogFilePath))
                {
                    // if no path is provided, try to use default from configuration
                    try { LogFilePath = ConfigurationManager.AppSettings[DEFAULT_CONFIG_FILE_PATH]; }
                    catch (ConfigurationErrorsException) { }

                    // if there is no default in Web.config, continue
                    if (string.IsNullOrEmpty(LogFilePath))
                        continue;
                }

                // get file name and append time interval
                if (LoggingElement.Interval == LoggingInterval.Second)
                    LogFilePath += LoggingElement.LogFileName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt";
                else if (LoggingElement.Interval == LoggingInterval.Minute)
                    LogFilePath += LoggingElement.LogFileName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm") + ".txt";
                else if (LoggingElement.Interval == LoggingInterval.Hour)
                    LogFilePath += LoggingElement.LogFileName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh") + ".txt";
                else if (LoggingElement.Interval == LoggingInterval.Day)
                    LogFilePath += LoggingElement.LogFileName + "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                else if (LoggingElement.Interval == LoggingInterval.Month)
                    LogFilePath += LoggingElement.LogFileName + "_" + DateTime.Now.ToString("yyyy_MM") + ".txt";
                else if (LoggingElement.Interval == LoggingInterval.Year)
                    LogFilePath += LoggingElement.LogFileName + "_" + DateTime.Now.ToString("yyyy") + ".txt";

                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(LogFilePath, true))
                        sw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString(), Message));
                    count++;
                }
                catch (Exception)
                {
                    // If it failed to write to text file, the file was most likely
                    // in use.. if that is the case, we will have to miss this message
                }
            }

            // if no elements are found with the same severity that was provided, try to use
            // default log file path provided in web config
            if (count == 0)
            {
                // file path from web config
                LogFilePath = ConfigurationManager.AppSettings[DEFAULT_CONFIG_FILE_PATH];
                if (string.IsNullOrEmpty(LogFilePath))
                    return;

                // give it default name
                LogFilePath += "log_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";

                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(LogFilePath, true))
                        sw.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToString(), Message));
                }
                catch (Exception)
                {
                    // If it failed to write to text file, the file was most likely
                    // in use.. if that is the case, we will have to miss this message
                }
            }
        }

        /// <summary>
        /// Logs the provided <paramref name="Exception"/> based on the <paramref name="ErrorSeverity"/>.
        /// If <paramref name="UseLoggingConfig"/> is true, the  logging configuration will
        /// attempt to be used to log the <paramref name="Exception"/>. If the <paramref name="Exception"/>
        /// could not be logged because the policy name in the  logging configuration is incorrect,
        /// that error will be logged separately to a text file.
        /// </summary>
        /// <param name="Exception">The exception to log.</param>
        /// <param name="ErrorSeverity">The severity level of the exception.</param>
        /// <param name="UseLoggingConfig">States whether or not to use the custom  logging configuration.</param>
        private void WriteToEnterpriseLibraryFile(Exception Exception, ErrorSeverity ErrorSeverity, bool UseLoggingConfig = true)
        {
            // get all elements with similar severity from web config if UseLoggingConfig is true,
            // otherwise make it an empty list to skip the foreach loop
            string LogFilePath = string.Empty;
            List<LoggingElement> LoggingElements = (UseLoggingConfig) ? GetLoggingElementsByPropertyValue("Severity", ErrorSeverity) : new List<LoggingElement>(0);
            
            // variables to track whether or not the exception was logged
            int handled_count = 0;
            bool handled = false;

            // loop through each configuration and log the exception to the corresponding files
            foreach (LoggingElement LoggingElement in LoggingElements)
            {
                // but remember, only log to files if their logging type
                // is designated as EnterpriseLibrary
                if (!LoggingElement.LoggingType.HasFlag(LoggingType.EnterpriseLibrary))
                    continue;
                
                // initially, set handled to false to denote that the exception was not logged
                handled = false;
                try
                {
                    // if a policy is provided..
                    if (!string.IsNullOrEmpty(LoggingElement.EnterprisePolicy))
                    {
                        ExceptionPolicy.HandleException(Exception, LoggingElement.EnterprisePolicy);
                        handled_count++;
                    }
                    // if a file path for the log file was provided..
                    else if (!string.IsNullOrEmpty(LoggingElement.LogFilePath))
                    {
                        // for this situation, we will make a new handler for a different path
                        // instead of modifying the default

                        // create new handler if it does not exist, otherwise retrieve handler from _temp_enterprise_loggers
                        EnterpriseExceptionLogging.LoggingExceptionHandler temp_handler =
                        CreateTempLogger(
                            FilePath: LoggingElement.LogFilePath,
                            FileName: LoggingElement.LogFileName + (LoggingElement.LogFileName.EndsWith(".log") ? "" : ".log"),
                            Interval: LoggingElement.Interval/*,
                            ForceCreate: true*/);

                        // handle exception
                        if (temp_handler != null)
                        {
                            temp_handler.HandleException(Exception, Guid.NewGuid());
                            handled_count++;
                        }
                    }
                    handled = true;
                }
                catch (Microsoft.Practices.ServiceLocation.ActivationException aex)
                {
                    // HandleException failed - this occurs when a policy is not present or the exception could not be handled properly
                    Log(aex, LoggingType.TextFile, ErrorSeverity.Warning);
                }

                // if the exception failed to be logged because the policy was incorrect..
                if (!handled)
                {
                    // if the default logger was successfully created..
                    if (_temp_enterprise_loggers.ContainsKey(DEFAULT_ENTERPRISE_LOGGER_NAME))
                    {
                        // try to write to default logger
                        try
                        {
                            _temp_enterprise_loggers[DEFAULT_ENTERPRISE_LOGGER_NAME].HandleException(Exception, Guid.NewGuid());
                            handled_count++;
                        }
                        catch (Microsoft.Practices.ServiceLocation.ActivationException aex)
                        {
                            // HandleException failed - this occurs when a policy is not present or the exception could not be handled properly
                            Log(aex, LoggingType.TextFile, ErrorSeverity.Warning);
                        }
                    }
                    else
                    {
                        // try to log using default policy name
                        try
                        {
                            ExceptionPolicy.HandleException(Exception, DEFAULT_EXCEPTION_POLICY);
                            handled_count++;
                        }
                        catch (Microsoft.Practices.ServiceLocation.ActivationException)
                        {
                            // HandleException failed - this occurs when a policy is not present or the exception could not be handled properly
                            // do not try to log this, this is the last resort
                        }
                    }
                }
            }

            // if no elements are found with the same severity that was provided or
            // the policies were not found, try to use default policy name
            if (handled_count == 0)
            {
                // if the default logger was created (i.e. there are no policies in config)
                if (_temp_enterprise_loggers.ContainsKey(DEFAULT_ENTERPRISE_LOGGER_NAME))
                {
                    // try to write to default logger
                    try { _temp_enterprise_loggers[DEFAULT_ENTERPRISE_LOGGER_NAME].HandleException(Exception, Guid.NewGuid()); }
                    catch (Microsoft.Practices.ServiceLocation.ActivationException aex)
                    {
                        // HandleException failed - this occurs when a policy is not present or the exception could not be handled properly
                        Log(aex, LoggingType.TextFile, ErrorSeverity.Warning);
                    }
                }
                else
                {
                    // try to log using default policy name
                    // if that fails, there is no hope..
                    try { ExceptionPolicy.HandleException(Exception, DEFAULT_EXCEPTION_POLICY); }
                    catch (Microsoft.Practices.ServiceLocation.ActivationException)
                    {
                        // HandleException failed - this occurs when a policy is not present or the exception could not be handled properly
                        // do not try to log this, this is the last resort
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets a list of  logging elements from the Web.config based
        /// on the provided <paramref name="Property"/> and <paramref name="Value"/>.
        /// </summary>
        /// <param name="Property">The name of the property to search by.</param>
        /// <param name="Value">The value that the property must have.</param>
        /// <returns></returns>
        private List<LoggingElement> GetLoggingElementsByPropertyValue(string Property, object Value)
        {
            try
            {
                LoggingSection LoggingSection = ConfigurationManager.GetSection("Logging") as LoggingSection;
                return (LoggingSection != null) ?
                    LoggingSection.Logging.GetElementsByPropertyValue(Property, Value) :
                    new List<LoggingElement>(0);
            }
            catch (ConfigurationErrorsException ceex)
            {
                // issue with web config, log it using text file..
                WriteToTextFile(ceex.Message, ErrorSeverity.Critical, false);
            }

            return new List<LoggingElement>(0);
        }

        /// <summary>
        /// Creates an Enterprise Library exception handler that utilizes
        /// a rolling flat file trace listener to write to log files.
        /// </summary>
        /// <param name="Name">The name of the <see cref="EnterpriseExceptionLogging.LoggingExceptionHandler"/>.</param>
        /// <param name="FilePath">Location of log file. If this is not provided, <see cref="DEFAULT_CONFIG_FILE_PATH"/> is used to try and retrieve file path from Web.config file.</param>
        /// <param name="FileName">Name of log file. If this is not provided, "default_rolling.log" is used.</param>
        /// <param name="Interval">How often a new file should be created.</param>
        /// <param name="Save">States whether or not to store the handler in memory.</param>
        /// <returns></returns>
        private EnterpriseExceptionLogging.LoggingExceptionHandler CreateTempLogger(string Name = "", string FilePath = "", string FileName = "", LoggingInterval Interval = LoggingInterval.Day, /*bool ForceCreate = false,*/ bool Save = true)
        {
            string default_file_path = FilePath;
            if (string.IsNullOrEmpty(default_file_path))
            {
                try { default_file_path = ConfigurationManager.AppSettings[DEFAULT_CONFIG_FILE_PATH]; }
                catch (ConfigurationErrorsException) { }
            }

            if (string.IsNullOrEmpty(default_file_path))
                return default(EnterpriseExceptionLogging.LoggingExceptionHandler);
            
            if (string.IsNullOrEmpty(Name))
                Name = default_file_path + (!string.IsNullOrEmpty(FileName) ? FileName : "default_rolling.log");

            string FullName = default_file_path + (!string.IsNullOrEmpty(FileName) ? FileName : "default_rolling.log");
            if (!FullName.EndsWith(".log"))
                FullName += ".log";

            if (_temp_enterprise_loggers.ContainsKey(Name))
                return _temp_enterprise_loggers[Name];

            EnterpriseExceptionLogging.LoggingExceptionHandler handler = default(EnterpriseExceptionLogging.LoggingExceptionHandler);
            try
            {
                //EnterpriseLogging.LogWriter writer = default(EnterpriseLogging.LogWriter);
                //using (EnterpriseLogging.LogWriterFactory factory = new EnterpriseLogging.LogWriterFactory())
                //using (writer = factory.CreateDefault())
                //{
                //    if (writer == null)
                //        return handler;

                //    if (!ForceCreate && writer.TraceSources.Count > 0)
                //    {
                //        // there already exists listeners in web config that we do
                //        // not want to overwrite, so there is no need to create a
                //        // default listener
                //        return handler;
                //    }
                //}
                
                // create formatter for rolling log file
                EnterpriseLogging.Formatters.TextFormatter formatter = new EnterpriseLogging.Formatters.TextFormatter(
                template:
                    "GMT Timestamp: {timestamp(MM/dd/yyyy HH:mm:ss)}\n" +
                    "Local Timestamp: {timestamp(local:hh:mm:ss:tt)}\n" +
                    "Message: {message}\n" +
                    "Category: {category}\n" +
                    "Priority: {priority}\n" +
                    "EventId: {eventid}\n" +
                    "Severity: {severity}\n" +
                    "Title:{title}\n" +
                    "Machine: {machine}\n" +
                    "Application Domain: {appDomain}\n" +
                    "Process Id: {processId}\n" +
                    "Process Name: {processName}\n" +
                    "Win32 Thread Id: {win32ThreadId}\n" +
                    "Thread Name: {threadName}\n" +
                    "Extended Properties: {dictionary({key} - {value})}\n");

                EnterpriseLogging.TraceListeners.RollInterval interval;
                if (!Enum.TryParse(Enum.GetName(typeof(LoggingInterval), Interval), true, out interval))
                    interval = EnterpriseLogging.TraceListeners.RollInterval.Day;

                // create trace listener for exception handler
                EnterpriseLogging.TraceListeners.RollingFlatFileTraceListener listener =
                    new EnterpriseLogging.TraceListeners.RollingFlatFileTraceListener(
                        fileName: FullName,
                        header: "----------------------------------------",
                        footer: "----------------------------------------",
                        formatter: formatter,
                        rollSizeKB: 0,
                        timeStampPattern: "yyyy-MM-dd",
                        rollFileExistsBehavior: EnterpriseLogging.TraceListeners.RollFileExistsBehavior.Overwrite,
                        rollInterval: interval);
                listener.TraceOutputOptions = TraceOptions.None;
                listener.Name = "Default Rolling Flat File Trace Listener";

                // add trace listener to the log writer's sources
                //if (OverwriteTraceListeners)
                //    writer.TraceSources.Clear();
                //if (writer.TraceSources.ContainsKey("General"))
                //    writer.TraceSources["General"].Listeners.Add(listener);
                //else
                //    writer.TraceSources.Add(
                //        key: "General",
                //        value: new EnterpriseLogging.LogSource(
                //            name: "Default Enterprise Logger",
                //            level: SourceLevels.All,
                //            traceListeners: new List<TraceListener>(1) { listener },
                //            autoFlush: true
                //            ));

                // create the exception handler that will handle the exceptions
                //handler = new EnterpriseExceptionLogging.LoggingExceptionHandler(
                //    logCategory: "General",
                //    eventId: 100,
                //    severity: TraceEventType.Error,
                //    title: "Default Enterprise Library Exception Handler",
                //    priority: 0,
                //    formatterType: typeof(TextExceptionFormatter),
                //    writer: writer);



                //List<EnterpriseLogging.Filters.LogFilter> filters = new List<EnterpriseLogging.Filters.LogFilter>();
                //EnterpriseLogging.LogSource main_source = new EnterpriseLogging.LogSource(
                //    name: "Default Enterprise Logger",
                //    level: SourceLevels.All,
                //    traceListeners: new List<TraceListener>(1) { listener },
                //    autoFlush: true
                //    );
                //IDictionary<string, EnterpriseLogging.LogSource> trace_sources = new Dictionary<string, EnterpriseLogging.LogSource>();
                //trace_sources.Add("General", main_source);
                //EnterpriseLogging.LogWriterStructureHolder holder = new EnterpriseLogging.LogWriterStructureHolder(filters, trace_sources, main_source, main_source, main_source, "General", true, true, false);
                //EnterpriseLogging.LogWriterImpl writer = new EnterpriseLogging.LogWriterImpl(holder, new EnterpriseLogging.Instrumentation.LoggingInstrumentationProvider(false, true, "EnhancedPartnerCenter"), new EnterpriseLogging.LoggingUpdateCoordinator(new Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.ConfigurationChangeEventSourceImpl()));

                //handler = new EnterpriseExceptionLogging.LoggingExceptionHandler(
                //    logCategory: "General",
                //    eventId: 100,
                //    severity: TraceEventType.Error,
                //    title: "Default Enterprise Library Exception Handler",
                //    priority: 0,
                //    formatterType: typeof(TextExceptionFormatter),
                //    writer: writer);

                //if (Save)
                //    _temp_enterprise_loggers.Add(Name, handler);


                // Try to fix this to work..
                List<EnterpriseLogging.Filters.LogFilter> filters = new List<EnterpriseLogging.Filters.LogFilter>();
                EnterpriseLogging.LogSource main_source = new EnterpriseLogging.LogSource(
                    name: "Default Enterprise Logger",
                    level: SourceLevels.All,
                    traceListeners: new List<TraceListener>(1) { listener },
                    autoFlush: true
                    );

                IDictionary<string, EnterpriseLogging.LogSource> trace_sources = new Dictionary<string, EnterpriseLogging.LogSource>();
                trace_sources.Add("General", main_source);

                EnterpriseLogging.LogWriterFactory factory_writer = new EnterpriseLogging.LogWriterFactory();
                EnterpriseLogging.LogWriterStructureHolder holder = new EnterpriseLogging.LogWriterStructureHolder(filters, trace_sources, main_source, main_source, main_source, "General", true, true, false);
                EnterpriseLogging.LogWriter writer = factory_writer.Create();
                // this is where chiz hit the fan
                writer.Configure(new Action<EnterpriseLogging.LoggingConfiguration>((EnterpriseLogging.LoggingConfiguration lc) =>
                {
                    lc.AddLogSource("");
                }));

                handler = new EnterpriseExceptionLogging.LoggingExceptionHandler(
                    logCategory: "General",
                    eventId: 100,
                    severity: TraceEventType.Error,
                    title: "Default Enterprise Library Exception Handler",
                    priority: 0,
                    formatterType: typeof(TextExceptionFormatter),
                    writer: writer);
            }
            catch (Exception)
            {
                handler = default(EnterpriseExceptionLogging.LoggingExceptionHandler);
            }

            return handler;
        }

        #endregion
    }
}