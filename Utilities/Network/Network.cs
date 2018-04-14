using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Utilities.Network
{
    /// <summary>
    /// Supplies methods to help resolve the current host and
    /// analyze requests made to the application.
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// Gets the host name of the local computer running
        /// the application.
        /// </summary>
        public static string CurrentHostName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        /// <summary>
        /// Gets the NetBIOS name of this local computer which
        /// is defined in the registry at system startup.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown by <see cref="Environment.MachineName"/>.</exception>
        public static string CurrentNetBIOSHostName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Gets an IPHostEntry instance using the
        /// <see cref="CurrentNetBIOSHostName"/> as the
        /// host name.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if valid IP address is not found.</exception>
        /// <exception cref="SocketException">Thrown if the host does not exist.</exception>
        public static IPHostEntry CurrentHostEntry()
        {
            string Host = CurrentNetBIOSHostName();

            // This ensures that an ArgumentException, ArgumentNullException,
            // and an ArgumentOutOfRangeException will not be thrown by
            // Dns.GetHostEntry(Host)
            if (string.IsNullOrEmpty(Host))
                throw new InvalidOperationException("The current net BIOS host name was null or empty.");
            if (Host.Length > 255)
                throw new InvalidOperationException(
                    string.Format(
                        "The current net BIOS host name was too long. Host: {0}.",
                        Host));
            IPAddress address;
            if (!IPAddress.TryParse(Host, out address))
                throw new InvalidOperationException(
                    string.Format(
                        "The current net BIOS host name was not formatted as a valid IP address. Host: {0}.",
                        Host));
                
            return Dns.GetHostEntry(Host);
        }

        /// <summary>
        /// Gets an array of IP addresses associated
        /// with the current host using the
        /// <see cref="CurrentNetBIOSHostName"/> as the
        /// host name.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if valid IP address is not found.</exception>
        /// <exception cref="SocketException">Thrown if the host does not exist.</exception>
        public static IPAddress[] CurrentHostAddresses()
        {
            string Host = CurrentNetBIOSHostName();

            // This ensures that an ArgumentException, ArgumentNullException,
            // and an ArgumentOutOfRangeException will not be thrown by
            // Dns.GetHostEntry(Host)
            if (string.IsNullOrEmpty(Host))
                throw new InvalidOperationException("The current net BIOS host name was null or empty.");
            if (Host.Length > 255)
                throw new InvalidOperationException(
                    string.Format(
                        "The current net BIOS host name was too long. Host: {0}.",
                        Host));
            IPAddress address;
            if (!IPAddress.TryParse(Host, out address))
                throw new InvalidOperationException(
                    string.Format(
                        "The current net BIOS host name was not formatted as a valid IP address. Host: {0}.",
                        Host));

            return Dns.GetHostAddresses(Host);
        }

        /// <summary>
        /// Gets the first IP address in the list of
        /// IP addresses of the current host.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if valid IP address is not found.</exception>
        /// <exception cref="SocketException">Thrown if the host does not exist.</exception>
        public static string CurrentHostIP()
        {
            IPAddress[] Addresses = CurrentHostAddresses();
            if (Addresses == null || Addresses.Length == 0)
                throw new InvalidOperationException("There were no host addresses found.");
                    
            return Addresses[0].ToString();
        }

        /// <summary>
        /// States whether or not the the current
        /// host IP address matches the production
        /// environment IP address.
        /// 
        /// This property will
        /// only be accurate if the following key exists
        /// in the Web.config file: "ProdWebServerIPAddress"
        /// </summary>
        /// <exception cref="ConfigurationErrorsException">
        /// Thrown when the "ProdWebServerIPAddress" key in the web config could not be found/read.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by <see cref="CurrentHostIP"/>.
        /// </exception>
        /// <exception cref="SocketException">
        /// Thrown by <see cref="CurrentHostIP"/>.
        /// </exception>
        public static bool IsProdEnvironment()
        {
            // check if key exists in web config
            string IP;
            if (!Configuration.Configuration.TryGetApplicationSetting("ProdWebServerIPAddress", out IP))
                throw new ConfigurationErrorsException("There was an issue obtaining the Production web server address from the web config. Could not find key: \"ProdWebServerIPAddress\"");

            // check if it has a value
            if (string.IsNullOrEmpty(IP))
                throw new ConfigurationErrorsException("There was no value in the web config for the key \"ProdWebServerIPAddress\".");

            // check if it is correctly formatted as an ip address
            IPAddress IPA_out;
            if (!IPAddress.TryParse(IP, out IPA_out))
                throw new ConfigurationErrorsException("The IP address in the web config could not be parsed. Please make sure it is formatted correctly.");

            return CurrentHostIP().Equals(IP);
        }

        /// <summary>
        /// States whether or not the current
        /// process is running locally by checking
        /// the IP address and environment.
        /// </summary>
        /// <exception cref="HttpException">
        /// Thrown if the context or request could not be obtained.
        /// </exception>
        public static bool IsLocal()
        {
            if (HttpContext.Current == null)
                throw new HttpException("The current http context is not available.");

            HttpRequest Request = HttpContext.Current.Request;
            if (Request == null)
                throw new HttpException("The current http request is not available.");

            // IsLocal checks for 127.0.0.1 and ::1
            // UserInteractive checks for user interface (like VS)

            // We are checking both because an IP can be spoofed,
            // but environment variables are trickier to manipulate
            // without code injection
            return Request.IsLocal && Environment.UserInteractive;
        }

        /// <summary>
        /// States whether or not the current request's Http method
        /// is HEAD.
        /// </summary>
        /// <exception cref="HttpException">
        /// Thrown if the context or request could not be obtained.
        /// </exception>
        public static bool IsHeadRequest()
        {
            if (HttpContext.Current == null)
                throw new HttpException("The current http context is not available.");

            HttpRequest Request = HttpContext.Current.Request;
            if (Request == null)
                throw new HttpException("The current http request is not available.");
            
            return Request.HttpMethod.ToLower().Equals("head");
        }
    }
}