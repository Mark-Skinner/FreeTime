using System.Web;

using Utilities.Logging;

namespace Utilities.Network.RequestHandlers
{
    interface IRequestHandler : IHttpHandler
    {
        ILogger GetLogger();
    }
}
