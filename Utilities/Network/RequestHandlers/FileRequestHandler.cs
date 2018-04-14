using System;
using System.IO;
using System.Web;

using Utilities.Logging;

namespace Utilities.Network.RequestHandlers
{
    public abstract class FileRequestHandler : IFileRequestHandler
    {
        #region Fields

        private ILogger _logger;
        private object _lock = new object();

        #endregion

        #region Methods

        public virtual ILogger GetLogger()
        {
            if (_logger == null)
            {
                lock (_lock)
                {
                    if (_logger == null)
                        _logger = Logger.Instance;
                }
            }
            return _logger;
        }

        public virtual bool IsReusable { get { return true; } }
        
        public abstract void ProcessRequest(HttpContext Context);

        #endregion
    }
}
