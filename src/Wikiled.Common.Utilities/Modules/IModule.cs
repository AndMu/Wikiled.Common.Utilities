using Microsoft.Extensions.DependencyInjection;

namespace Wikiled.Common.Utilities.Modules
{
    public interface IModule
    {
        IServiceCollection ConfigureServices(IServiceCollection services);
    }
}
