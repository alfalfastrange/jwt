namespace Jwt.Entity.Enums
{
    public enum AuthenticationResultType
    {
        NotAuthenticated = 1,
        Authenticated = 2,
        LockedOut = 3,
        Disabled = 4
    }
}