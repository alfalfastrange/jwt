using System;
using System.Reflection;
using Jwt.IoC;
using Ninject;

namespace Jwt.Api
{
    public static class NinjectConfig
    {
        public static readonly Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterServices(kernel);
            return kernel;
        });

        private static void RegisterServices(KernelBase kernel)
        {

            kernel.Load(new InternalBinder());
            kernel.Load(new ExternalBinder());
        }
    }
}