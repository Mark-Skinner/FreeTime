using System;
using System.IO;
using System.Threading;
using System.Web;

using Utilities.Logging;

namespace Utilities.Network.RequestHandlers
{
    public class OfficeFileRequestHandler : FileRequestHandler
    {        
        public override void ProcessRequest(HttpContext Context)
        {
            // stream the file
            try
            {
                HttpRequest Request = Context.Request;
                string RelativePath = Request.Path;
                if (string.IsNullOrEmpty(RelativePath))
                    return;
                if (!RelativePath.StartsWith("/"))
                    RelativePath = "/" + RelativePath;
                if (RelativePath.EndsWith("/"))
                    RelativePath = RelativePath.Remove(RelativePath.Length - 1);
                string FileName = RelativePath.Substring(RelativePath.LastIndexOf("/") + 1);
                if (string.IsNullOrEmpty(FileName))
                    return;
                string Path = Context.Server.MapPath("~" + RelativePath);
                if (string.IsNullOrEmpty(Path))
                    return;

                if (File.Exists(Path))
                {
                    string MimeType = MimeMapping.GetMimeMapping(FileName);
                    if (string.IsNullOrEmpty(MimeType))
                        return;
                    HttpResponse Response = Context.Response;
                    Response.ContentType = MimeType;
                    Response.Charset = ""; // default value is "utf-8" - if it is an empty string, it uses default encoding provided by the BOM (byte order mark, AKA magic number)
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.WriteFile(Path);
                    // If Response.End() is used, it will complete thread execution
                    // and throw ThreadAbortException. This is expected, and means no more
                    // code will run after this method. If not used, you will still
                    // run code after the request (e.g. other http handlers, EndRequest event handler, etc.).
                    //Response.End();

                    // If Context.ApplicationInstance.CompleteRequest() is used,
                    // then all code will be skipped except for the EndRequest event
                    // handler. Also, this method will not throw an exception.
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (HttpException hex)
            {
                // error obtaining Request or Response, mapping relative file path to full path, or setting the Response
                GetLogger().Log(hex, LoggingType.EnterpriseLibrary, ErrorSeverity.Critical);
            }
            //catch (ThreadAbortException)
            //{
            //    // response successfully ended
            //}
            catch (Exception ex)
            {
                // other exceptions
                GetLogger().Log(ex, LoggingType.EnterpriseLibrary, ErrorSeverity.Critical);
            }
        }
    }
}
