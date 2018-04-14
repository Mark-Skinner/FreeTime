using System;
using System.Diagnostics;
using System.Web;

namespace Utilities.Network.Modules
{
    public class TimingModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            if (Network.IsLocal())
            {
                Stopwatch stopwatch = new Stopwatch();
                HttpContext.Current.Items["Stopwatch"] = stopwatch;
                stopwatch.Start();
            }
        }

        void OnEndRequest(object sender, EventArgs e)
        {
            if (Network.IsLocal())
            {
                Stopwatch stopwatch = (Stopwatch)HttpContext.Current.Items["Stopwatch"];
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.ElapsedMilliseconds + "ms");
            }
        }
    }

}
