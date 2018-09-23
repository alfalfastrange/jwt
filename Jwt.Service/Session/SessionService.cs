using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Jwt.Service.Api.Session;
using Microsoft.Owin;

namespace Jwt.Service.Session
{
    public class SessionService : ISessionService
    {
        private const string OWIN_CONTEXT = "MS_OwinContext";
        private const string ProfileIdKey = "profileId";

        public string GetIpAddress(HttpRequestMessage requestMessage)
        {
            if (requestMessage.Properties.ContainsKey(OWIN_CONTEXT))
            {
                OwinContext owinContext = requestMessage.Properties[OWIN_CONTEXT] as OwinContext;
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }
            return null;
        }

        public long GetAuthenticatedProfileId(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.TryGetValues(ProfileIdKey, out var values);
            if (values != null && values.Any())
            {
                return long.Parse(values.First());
            }
            return 0;
        }

        public void SetAuthenticatedProfileId(IOwinRequest request, long profileId)
        {
            string[] profileIdValue = { profileId.ToString() };
            var profileIdHeader = new KeyValuePair<string, string[]>(ProfileIdKey, profileIdValue);
            if (request.Headers.Keys.Contains(ProfileIdKey))
            {
                request.Headers.Keys.Remove(ProfileIdKey);
            }
            request.Headers.Add(profileIdHeader);
        }
    }
}