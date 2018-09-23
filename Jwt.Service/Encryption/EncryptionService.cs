using System;
using System.Security.Cryptography;
using Jwt.Service.Domain.Encryption;

namespace Jwt.Service.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        public string GetSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var bytes = new byte[256];
            randomNumberGenerator.GetBytes(bytes);
            string salt = Convert.ToBase64String(bytes);
            return salt;
        }

        public string GetHash(string password, string salt)
        {
            string passwordHash;
            string source = salt + password;
            var encoder = new System.Text.UnicodeEncoding();
            byte[] bytes = encoder.GetBytes(source);
            using (var cryptoProvider = new SHA256CryptoServiceProvider())
            {
                byte[] hash = cryptoProvider.ComputeHash(bytes);
                passwordHash = Convert.ToBase64String(hash);
            }
            return passwordHash;
        }
    }
}