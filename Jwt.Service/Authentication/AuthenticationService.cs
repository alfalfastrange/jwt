using System;
using Jwt.Entity.Entities;
using Jwt.Entity.Enums;
using Jwt.Entity.ValueObjects;
using Jwt.Repository.Interfaces;
using Jwt.Service.Api.Authentication;
using Jwt.Service.Domain.Encryption;

namespace Jwt.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IProfileRepository _profileRepository;

        public AuthenticationService(IEncryptionService encryptionService,
                                     IProfileRepository profileRepository)
        {
            _encryptionService = encryptionService;
            _profileRepository = profileRepository;
        }

        public AuthenticationResult GetAuthenticatedProfile(string username, string password)
        {
            var authenticationResult = new AuthenticationResult();
            Profile profile = _profileRepository.FindSingle(x => string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase));
            if (profile != null)
            {
                authenticationResult.SetProfile(profile);
                if (!profile.IsEnabled)
                {
                    authenticationResult.SetuAuthenticationResultType(AuthenticationResultType.Disabled);
                    return authenticationResult;
                }
                if (profile.FailedLoginCount >= Convert.ToInt32(Common.ErrorMessages.MAX_FAILED_LOGIN_ATTEMPTS))
                {
                    authenticationResult.SetuAuthenticationResultType(AuthenticationResultType.LockedOut);
                    return authenticationResult;
                }
                if (IsAuthenticated(profile, password))
                {
                    authenticationResult.SetuAuthenticationResultType(AuthenticationResultType.Authenticated);
                }
            }
            return authenticationResult;
        }

        private bool IsAuthenticated(Profile profile, string password)
        {
            string passwordHash = _encryptionService.GetHash(password, profile.Salt);
            bool isValid = passwordHash == profile.PasswordHash;
            if (isValid)
            {
                ClearFailedLoginAttempts(profile);
            }
            else
            {
                TrackFailedLoginAttempt(profile);
            }
            return isValid;
        }

        private void TrackFailedLoginAttempt(Profile profile)
        {
            profile.IncrementFailedLoginCount();
            _profileRepository.Update(profile);
        }

        private void ClearFailedLoginAttempts(Profile profile)
        {
            profile.ClearFailedLoginCount();
            _profileRepository.Update(profile);
        }
    }
}