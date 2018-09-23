namespace Jwt.Service.Domain.Encryption
{
    public interface IEncryptionService
    {
        string GetSalt();

        string GetHash(string password, string salt);
    }
}