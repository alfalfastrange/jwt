using Jwt.Service.Api.Authentication;
using Jwt.Service.Authentication;
using Jwt.Service.Domain.Encryption;
using Jwt.Service.Encryption;
using Ninject.Modules;

namespace Jwt.IoC
{
    public class InternalBinder : NinjectModule
    {
        public override void Load()
        {
            // Services
            Bind<IAuthenticationService>().To<AuthenticationService>();
            Bind<IEncryptionService>().To<EncryptionService>();
        }
    }
}