using System;
using System.Collections.Generic;

namespace Utilities.Security
{
    public interface ISSOAuthenticationHandler
    {
        SSOIdentity Authenticate(IDictionary<string, string> headers);
        string GetIdentifier();
        IList<string> GetRequiredHeaders();
        IList<string> GetSupportedHeaders();
        bool TryParseHeaders(IDictionary<string, string> Headers, out IDictionary<string, string> ParsedHeaders);
        void OnError(string Error);
        void OnError(Exception Exception);
    }
}
