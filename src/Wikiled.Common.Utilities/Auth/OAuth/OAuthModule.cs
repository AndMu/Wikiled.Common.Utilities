using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Common.Utilities.Auth.OAuth;

public static class OAuthModule
{
    public static IServiceCollection AddOAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IOAuthHelper, OAuthHelper>();
        services.AddTransient(typeof(IAuthentication<>), typeof(OAuthAuthentication<>));
        configuration.ExtractConfig<OAuthConfig>("OAuth", services);
        return services;
    }
}