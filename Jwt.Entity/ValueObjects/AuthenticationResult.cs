using Jwt.Entity.Entities;
using Jwt.Entity.Enums;

namespace Jwt.Entity.ValueObjects
{
    public class AuthenticationResult
    {
        public AuthenticationResultType AuthenticationResultType { get; private set; }

        public Profile Profile { get; private set; }

        public AuthenticationResult()
        {
            AuthenticationResultType = AuthenticationResultType.NotAuthenticated;
        }

        public void SetuAuthenticationResultType(AuthenticationResultType authenticationResultType)
        {
            AuthenticationResultType = authenticationResultType;
        }

        public void SetProfile(Profile profile)
        {
            Profile = profile;
        }
    }
}