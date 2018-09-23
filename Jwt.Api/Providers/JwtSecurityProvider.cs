using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Jwt.Entity.Entities;
using Jwt.Repository.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Thinktecture.IdentityModel.Tokens;

namespace Jwt.Api.Providers
{
    public class JwtSecurityProvider : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ISessionRepository _sessionRepository;

        private const string ClientPropertyKey = "audience";

        public JwtSecurityProvider(IClientRepository clientRepository,
                                   ISessionRepository sessionRepository)
        {
            _clientRepository = clientRepository;
            _sessionRepository = sessionRepository;
        }

        public string Protect(AuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException("data");
            }
            Client client = GetClient(ticket);
            JwtSecurityToken token = GetJwtSecurityToken(ticket, client);
            string jwt = ProtectJwtSecurityToken(token);
            CreateSession(ticket, client.Id, jwt);
            return jwt;
        }

        private Client GetClient(AuthenticationTicket ticket)
        {
            string clientId = ticket.Properties.Dictionary.ContainsKey(ClientPropertyKey) ? ticket.Properties.Dictionary[ClientPropertyKey] : null;
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new InvalidOperationException("Authentication ticket does not include client Id");
            }
            Client client = _clientRepository.FindSingle(x => x.ClientId == clientId);
            return client;
        }

        private JwtSecurityToken GetJwtSecurityToken(AuthenticationTicket ticket, Client client)
        {
            byte[] securityKey = TextEncodings.Base64Url.Decode(client.Secret);
            var signingKey = new HmacSigningCredentials(securityKey);
            DateTimeOffset? issued = ticket.Properties.IssuedUtc;
            DateTimeOffset? expires = ticket.Properties.ExpiresUtc;
            var token = new JwtSecurityToken(client.Name, client.ClientId, ticket.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);
            return token;
        }

        private string ProtectJwtSecurityToken(JwtSecurityToken token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string jwt = jwtSecurityTokenHandler.WriteToken(token);
            return jwt;
        }

        private void CreateSession(AuthenticationTicket ticket, long clientId, string jwt)
        {
            long createdBy = GetProfileId(ticket);
            var session = new Session(clientId, createdBy, jwt, ticket.Properties.ExpiresUtc.Value.UtcDateTime);
            _sessionRepository.Insert(session);
        }

        private int GetProfileId(AuthenticationTicket ticket)
        {
            Claim profileIdClaim = ticket.Identity.Claims.First(x => x.Type == "profileId");
            return int.Parse(profileIdClaim.Value);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}