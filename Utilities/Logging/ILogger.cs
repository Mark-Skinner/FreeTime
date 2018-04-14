using System;

namespace Utilities.Logging
{
    #region Enumerations

    /// <summary>
    /// The type of logger that will be used to log the errors.
    /// </summary>
    /// <example>
    /// // When logging an error with the following type variable, it will be logged to a text file (path in web config)
    /// LoggingType type = LoggingType.TextFile;
    /// 
    /// // When logging an error with the followng type variable, it will be logged using the Exception Policy described in the web config
    /// LoggingType type = LoggingType.EnterpriseLibrary;
    /// 
    /// // When logging an error with one of the following type variables, it will be logged using both
    /// LoggingType type = LoggingType.EnterpriseLibrary | LoggingType.TextFile;
    /// LoggingType type = LoggingType.TextFile | LoggingType.EnterpriseLibrary;
    /// </example>
    [Flags]
    public enum LoggingType
    {
        /// <summary>
        /// This type will use the Enterprise Library logger providied by Microsoft.
        /// </summary>
        EnterpriseLibrary = 1,
        /// <summary>
        /// This type will write to a file in the location specified in the Web.config file.
        /// </summary>
        TextFile = 2
    }

    /// <summary>
    /// The level of severity of an error in the application.
    /// </summary>
    [Flags]
    public enum ErrorSeverity
    {
        /// <summary>
        /// The error will cause core functionality to stop working.
        /// </summary>
        Critical = 1,
        /// <summary>
        /// The error may influence problems elsewhere in the application.
        /// </summary>
        Warning = 2,
        /// <summary>
        /// The error is strictly informational (mainly used for debugging).
        /// </summary>
        Informational = 4
    }

    /// <summary>
    /// The frequency of new log files. The file name will
    /// change depending on the <see cref="LoggingInterval"/>.
    /// </summary>
    public enum LoggingInterval
    {
        /// <summary>
        /// Create a new log file if the previous one is over a second old.
        /// </summary>
        Second = 1,
        /// <summary>
        /// Create a new log file if the previous one is over a minute old.
        /// </summary>
        Minute = 2,
        /// <summary>
        /// Create a new log file if the previous one is over an hour old.
        /// </summary>
        Hour = 3,
        /// <summary>
        /// Create a new log file if the previous one is over a day old.
        /// </summary>
        Day = 4,
        /// <summary>
        /// Create a new log file if the previous one is over a month old.
        /// </summary>
        Month = 5,
        /// <summary>
        /// Create a new log file if the previous one is over a year old.
        /// </summary>
        Year = 6
    }

    #endregion

    /// <summary>
    /// Represents an outline for a basic application logger.
    /// </summary>
    public interface ILogger
    {
        void Log(string Message, LoggingType LoggingType, ErrorSeverity ErrorSeverity);

        void Log(Exception Exception, LoggingType LoggingType, ErrorSeverity ErrorSeverity);
    }
}
