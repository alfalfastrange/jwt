using System.Web.Http;
using Jwt.IoC;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace Jwt.Api.Middleware
{
    public static class IoCContainerMiddleware
    {
        internal static void ConfigureIoCContainer(this IAppBuilder appBuilder, CompositionRoot compositionRoot, HttpConfiguration httpConfiguration)
        {
            appBuilder.UseNinjectMiddleware(() => compositionRoot.Kernel).UseNinjectWebApi(httpConfiguration);
        }
    }
}