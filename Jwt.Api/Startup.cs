using System.Web.Http;
using Jwt.Api.Middleware;
using Jwt.IoC;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Jwt.Api.Startup))]
namespace Jwt.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.UseCors(CorsOptions.AllowAll);

            var compositionRoot = new CompositionRoot();
            HttpConfiguration httpConfiguration = appBuilder.ConfigureHttp();
            appBuilder.ConfigureOAuthAuthentication(compositionRoot);
            appBuilder.ConfigureJwtAuthorization(compositionRoot);
            appBuilder.ConfigureIoCContainer(compositionRoot, httpConfiguration);
        }
    }
}