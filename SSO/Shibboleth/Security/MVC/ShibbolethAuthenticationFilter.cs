using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Security.Principal;

using Utilities.Security.Mvc;

namespace SSO.Shibboleth.Security.Mvc
{
    public class ShibbolethAuthenticationFilter : SSOAuthenticationFilter
    {
        public ShibbolethAuthenticationFilter(IShibbolethAuthenticationHandler AuthenticationHandler) : base(AuthenticationHandler)
        {

        }

        public override void OnAuthentication(AuthenticationContext FilterContext)
        {
            try
            {
                if (FilterContext == null)
                {
                    AuthenticationHandler.OnError("FilterContext is null.");
                    return;
                }

                if (!FilterContext.Principal.Identity.IsAuthenticated)
                {
                    // check if they are allowed to visit page without authentication
                    string controller = FilterContext.RouteData.Values["controller"].ToString().ToLower();
                    string security_bypass_controllers = "";// _authSectionHandler.ErrorController;
                    if (!string.IsNullOrEmpty(security_bypass_controllers))
                    {
                        List<string> errorsControllers_split = new List<string>(security_bypass_controllers.Replace(" ", "").ToLower().Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                        if (errorsControllers_split.Contains(controller))
                        {
                            var anonymous_principal = new GenericPrincipal(new GenericIdentity("anonymous"), null);
                            FilterContext.Principal = anonymous_principal;
                            return;
                        }
                    }

                    // Otherwise, authenticate the user
                    ShibbolethPrincipal principal = new ShibbolethPrincipal(AuthenticationHandler as IShibbolethAuthenticationHandler);
                    Dictionary<string, string> headers = GetServerVariables(FilterContext);
                    IDictionary<string, string> matches;
                    bool successfully_parsed = AuthenticationHandler.TryParseHeaders(headers, out matches);

                    // no headers were found, or required headers were not found
                    if (!successfully_parsed)
                    {
                        // log that some required headers were not found
                        System.Text.StringBuilder Message = new System.Text.StringBuilder("Missing one or more required HTTP headers. Headers: ");

                        // append the provided headers so we can see which ones are missing..
                        foreach (KeyValuePair<string, string> pair in matches)
                            Message.AppendFormat("matches[{0}] = {1}; ", pair.Key, pair.Value);

                        // log the message and redirect to error page IF it is not a head request (Most likely WebSeal)
                        AuthenticationHandler.OnError(Message.ToString());
                        FilterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Index" }));//new HttpUnauthorizedResult();
                        return;
                    }

                    if (matches != null)
                        principal.Initialize(matches);

                    if (principal.Identity != null && principal.Identity.IsAuthenticated)
                    {
                        FilterContext.Principal = principal;
                    }
                    else
                    {
                        string error = "User NOT Authenticated!";
                        if (principal.Identity != null)
                        {
                            ShibbolethIdentity shib_identity = principal.Identity as ShibbolethIdentity;
                            if (shib_identity != null)
                            {
                                // Key = Employee ID
                                if (!string.IsNullOrEmpty(shib_identity.Key))
                                    error = string.Format("User [{0}] NOT Authenticated!", shib_identity.Key);
                                // NetworkId = NS ID
                                else if (!string.IsNullOrEmpty(shib_identity.NetworkId))
                                    error = string.Format("User [{0}] NOT Authenticated!", shib_identity.NetworkId);
                            }
                        }

                        AuthenticationHandler.OnError(error);
                        FilterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Index" }));//new HttpUnauthorizedResult();
                    }
                }
            }
            catch (Exception ex)
            {
                AuthenticationHandler.OnError(ex);
                FilterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Index" }));//new HttpUnauthorizedResult();
            }
        }

        private Dictionary<string,string> GetServerVariables(AuthenticationContext FilterContext)
        {
            Dictionary<string, string> server_variables = new Dictionary<string, string>();
            try
            {
                // read headers
                var context = FilterContext.HttpContext;
                if (context != null)
                {
                    var request = context.Request;
                    if (request != null)
                    {
                        server_variables = new Dictionary<string, string>(request.ServerVariables.Count);

                        foreach (var key in request.ServerVariables.AllKeys)
                            server_variables.Add(key.ToLower(), request.ServerVariables[key]);
                    }
                }
            }
            catch (Exception ex)
            {
                AuthenticationHandler.OnError(ex);
            }

            return server_variables;
        }
    }
}
