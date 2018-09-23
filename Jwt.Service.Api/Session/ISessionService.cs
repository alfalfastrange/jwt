using System.Net.Http;
using Microsoft.Owin;

namespace Jwt.Service.Api.Session
{
    public interface ISessionService
    {
        string GetIpAddress(HttpRequestMessage requestMessage);
        long GetAuthenticatedProfileId(HttpRequestMessage requestMessage);
        void SetAuthenticatedProfileId(IOwinRequest request, long profileId);
    }
}