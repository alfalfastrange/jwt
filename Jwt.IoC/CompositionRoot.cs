using Ninject;
using Ninject.Modules;

namespace Jwt.IoC
{
    public class CompositionRoot
    {
        private IKernel _kernel;

        public IKernel Kernel
        {
            get { return _kernel ?? (_kernel = new StandardKernel()); }
            set { _kernel = value; }
        }

        public T Resolve<T>()
        {
            return Kernel.Get<T>();
        }

        private void Wire(INinjectModule ninjectModule)
        {
            Kernel.Load(ninjectModule);
        }
    }
}