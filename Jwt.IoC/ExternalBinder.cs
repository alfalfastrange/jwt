using Jwt.Repository.Interfaces;
using Jwt.Repository.Repositories;
using Jwt.Service.Api.Session;
using Jwt.Service.Session;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Jwt.IoC
{
    public class ExternalBinder : NinjectModule
    {
        public override void Load()
        {
            // Providers
            Bind<ISessionService>().To<SessionService>().InRequestScope();

            // Repositories
            Bind<IClientRepository>().To<ClientRepository>();
            Bind<IProfileRepository>().To<ProfileRepository>();
            Bind<ISessionRepository>().To<SessionRepository>();
        }
    }
}