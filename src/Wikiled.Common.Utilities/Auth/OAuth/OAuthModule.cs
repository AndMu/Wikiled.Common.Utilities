using Microsoft.Extensions.DependencyInjection;
using System;
using Wikiled.Common.Utilities.Modules;

namespace Wikiled.Common.Utilities.Auth.OAuth
{
    public class OAuthModule : IModule
    {
        private readonly OAuthConfig config;

        public OAuthModule(OAuthConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(config);
            services.AddTransient<IOAuthHelper, OAuthHelper>();
            services.AddTransient(typeof(IAuthentication<>), typeof(OAuthAuthentication<>));
            return services;
        }
    }
}
