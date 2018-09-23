using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jwt.Entity.Entities;
using Jwt.IoC;
using Jwt.Repository.Interfaces;
using Jwt.Service.Api.Session;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Jwt.Api.Middleware
{
    internal static class JwtAuthorizationMiddleware
    {
        internal static void ConfigureJwtAuthorization(this IAppBuilder appBuilder, CompositionRoot compositionRoot)
        {
            compositionRoot.Kernel = NinjectConfig.CreateKernel.Value;
            ISessionService sessionService = compositionRoot.Resolve<ISessionService>();
            IClientRepository clientRepository = compositionRoot.Resolve<IClientRepository>();
            ISessionRepository sessionRepository = compositionRoot.Resolve<ISessionRepository>();
            IProfileRepository profileRepository = compositionRoot.Resolve<IProfileRepository>();

            IList<Client> allowedClients = JwtAuthorizationMiddleware.GetAllowedClients(clientRepository);
            appBuilder.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = GetAllowedClientIds(allowedClients),
                IssuerSecurityTokenProviders = GetIssuerSecurityTokenProviders(allowedClients),
                Provider = new OAuthBearerAuthenticationProvider
                {
                    OnValidateIdentity = context =>
                    {
                        string authorizationHeader = context.OwinContext.Request.Headers["Authorization"];
                        string jwt = authorizationHeader.Replace("Bearer ", "");
                        Session session = sessionRepository.FindSingle(x => x.Token == jwt && !x.IsForceExpired);
                        Profile profile = profileRepository.GetById(session.ProfileId);
                        if (SessionIsValid(context, session) && ProfileIsValid(profile))
                        {
                            sessionService.SetAuthenticatedProfileId(context.Request, profile.Id);
                            context.Validated();
                            return Task.FromResult<object>(null);
                        }
                        context.Rejected();
                        return Task.FromResult<object>(null);
                    }
                }
            });
        }

        private static bool SessionIsValid(OAuthValidateIdentityContext context, Session session)
        {
            return session != null && (DateTime.UtcNow <= session.ExpirationDate && DateTime.UtcNow <= context.Ticket.Properties.ExpiresUtc);
        }

        private static bool ProfileIsValid(Profile profile)
        {
            return profile != null && profile.IsEnabled;
        }

        private static IList<Client> GetAllowedClients(IClientRepository clientRepository)
        {
            IList<Client> clients = clientRepository.GetAll();
            return clients;
        }

        private static IEnumerable<string> GetAllowedClientIds(IList<Client> allowedClients)
        {
            var clientIds = new List<string>();
            foreach (Client allowedClient in allowedClients)
            {
                clientIds.Add(allowedClient.ClientId);
            }
            return clientIds;
        }

        private static IEnumerable<IIssuerSecurityTokenProvider> GetIssuerSecurityTokenProviders(IList<Client> allowedClients)
        {
            var providers = new List<IIssuerSecurityTokenProvider>();
            foreach (Client allowedClient in allowedClients)
            {
                providers.Add(new SymmetricKeyIssuerSecurityTokenProvider(allowedClient.Name, TextEncodings.Base64Url.Decode(allowedClient.Secret)));
            }
            return providers;
        }
    }
}