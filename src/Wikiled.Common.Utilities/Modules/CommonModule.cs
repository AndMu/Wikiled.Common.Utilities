using System.Reactive.Concurrency;
using Autofac;
using Wikiled.Common.Utilities.Config;
using Wikiled.Common.Utilities.Rx;

namespace Wikiled.Common.Utilities.Modules
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(TaskPoolScheduler.Default).As<IScheduler>();
            builder.RegisterType<ApplicationConfiguration>().As<IApplicationConfiguration>();
            builder.RegisterType<ObservableTimer>().As<IObservableTimer>();
        }
    }
}
