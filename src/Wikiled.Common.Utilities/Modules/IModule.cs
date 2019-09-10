using Microsoft.Extensions.DependencyInjection;

namespace Wikiled.Common.Utilities.Modules
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
