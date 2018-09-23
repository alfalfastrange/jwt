using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Jwt.Entity.Entities;
using Jwt.Entity.Enums;
using Jwt.Entity.ValueObjects;
using Jwt.Repository.Interfaces;
using Jwt.Service.Api.Authentication;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Jwt.Api.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IClientRepository _clientRepository;

        public AuthorizationServerProvider(IAuthenticationService authenticationService,
                                           IClientRepository clientRepository)
        {
            _authenticationService = authenticationService;
            _clientRepository = clientRepository;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            context.TryGetFormCredentials(out clientId, out clientSecret);
            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }
            Client client = _clientRepository.FindSingle(x => x.ClientId == clientId);
            if (client == null || client.ClientId != context.ClientId)
            {
                context.SetError("invalid_clientId", "Client Id not found");
                return Task.FromResult<object>(null);
            }
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            AuthenticationResult authenticationResult = _authenticationService.GetAuthenticatedProfile(context.UserName, context.Password);
            if (authenticationResult.AuthenticationResultType == AuthenticationResultType.Authenticated)
            {
                ClaimsIdentity claimsIdentity = GetClaimsIdentity(context, authenticationResult.Profile);
                CreateAuthenticationTicket(context, claimsIdentity);
                return Task.FromResult<object>(null);
            }
            return GetAuthenticationFailure(context, authenticationResult);
        }

        private ClaimsIdentity GetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, Profile profile)
        {
            var claimsIdentity = new ClaimsIdentity("JWT");
            claimsIdentity.AddClaim(new Claim("sub", context.UserName));
            claimsIdentity.AddClaim(new Claim("profileId", profile.Id.ToString()));
            claimsIdentity.AddClaim(new Claim("username", profile.Username));
            claimsIdentity.AddClaim(new Claim("firstName", profile.FirstName));
            claimsIdentity.AddClaim(new Claim("lastName", profile.LastName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, profile.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Enum.GetName(typeof(ProfileType), profile.ProfileTypeId)));
            return claimsIdentity;
        }

        private void CreateAuthenticationTicket(OAuthGrantResourceOwnerCredentialsContext context, ClaimsIdentity claimsIdentity)
        {
            var authenticationProperties = new AuthenticationProperties(new Dictionary<string, string>
            {
                { "audience", context.ClientId },
                { "issuer", context.Request.Host.Value }
            });
            var authenticationTicket = new AuthenticationTicket(claimsIdentity, authenticationProperties);
            context.Validated(authenticationTicket);
        }

        private Task GetAuthenticationFailure(OAuthGrantResourceOwnerCredentialsContext context, AuthenticationResult authenticationResult)
        {
            Client client = _clientRepository.FindSingle(x => x.ClientId == context.ClientId);
            if (authenticationResult.Profile == null ||
                authenticationResult.AuthenticationResultType == AuthenticationResultType.Disabled ||
                authenticationResult.AuthenticationResultType == AuthenticationResultType.NotAuthenticated)
            {
                context.SetError("Invalid username and password combination.");
            }
            else if (authenticationResult.AuthenticationResultType == AuthenticationResultType.LockedOut)
            {
                context.SetError("Locked Out. Please try the 'Forgot Password' link to reset your account.");
            }
            return Task.FromResult<object>(null);
        }
    }
}