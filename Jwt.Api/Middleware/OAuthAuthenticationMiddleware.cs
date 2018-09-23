using System;
using Jwt.Api.Providers;
using Jwt.IoC;
using Jwt.Repository.Interfaces;
using Jwt.Service.Api.Authentication;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Jwt.Api.Middleware
{
    internal static class OAuthAuthenticationMiddleware
    {
        internal static void ConfigureOAuthAuthentication(this IAppBuilder appBuilder, CompositionRoot compositionRoot)
        {
            compositionRoot.Kernel = NinjectConfig.CreateKernel.Value;
            IAuthenticationService authenticationService = compositionRoot.Resolve<IAuthenticationService>();
            IClientRepository clientRepository = compositionRoot.Resolve<IClientRepository>();
            ISessionRepository sessionRepository = compositionRoot.Resolve<ISessionRepository>();

            var oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new AuthorizationServerProvider(authenticationService, clientRepository),
                AccessTokenFormat = new JwtSecurityProvider(clientRepository, sessionRepository)
            };
            appBuilder.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
        }
    }
}