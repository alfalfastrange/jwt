using Jwt.Entity.ValueObjects;

namespace Jwt.Service.Api.Authentication
{
    public interface IAuthenticationService
    {
        AuthenticationResult GetAuthenticatedProfile(string email, string password);
    }
}