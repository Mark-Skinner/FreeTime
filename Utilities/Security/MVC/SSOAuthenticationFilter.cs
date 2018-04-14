using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Utilities.Security.Mvc
{
    public abstract class SSOAuthenticationFilter : IActionFilter, IAuthenticationFilter
    {
        public SSOAuthenticationFilter(ISSOAuthenticationHandler AuthenticationHandler)
        {
            this.AuthenticationHandler = AuthenticationHandler;
        }

        public ISSOAuthenticationHandler AuthenticationHandler { get; }

        public abstract void OnAuthentication(AuthenticationContext FilterContext);

        public virtual void OnAuthenticationChallenge(AuthenticationChallengeContext FilterContext) { }

        public virtual void OnActionExecuting(ActionExecutingContext FilterContext) { }

        public virtual void OnActionExecuted(ActionExecutedContext FilterContext) { }
    }
}
