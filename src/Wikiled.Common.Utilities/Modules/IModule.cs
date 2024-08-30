using Microsoft.Extensions.DependencyInjection;

namespace Wikiled.Common.Utilities.Modules;

public interface IModule
{
    IServiceCollection ConfigureCommonServices(IServiceCollection services);
}