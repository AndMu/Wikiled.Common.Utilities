using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Auth;

public class PersistedAuthentication<T> : IAuthentication<T>
    where T : class
{
    private readonly ILogger<PersistedAuthentication<T>> log;

    private readonly IAuthentication<T> underlying;

    public PersistedAuthentication(ILogger<PersistedAuthentication<T>> log, IAuthentication<T> underlying)
    {
        this.underlying = underlying ?? throw new ArgumentNullException(nameof(underlying));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public string AuthFile { get; set; } = "key.auth";

    public async Task<T> Authenticate()
    {
        if (File.Exists(AuthFile))
        {
            log.LogInformation("Found saved credentials. Loading...");
            var json = File.ReadAllText(AuthFile);
            return await Refresh(JsonSerializer.Deserialize<T>(json)).ConfigureAwait(false);
        }

        T credentials = await underlying.Authenticate().ConfigureAwait(false);
        Save(credentials);
        return credentials;
    }

    public async Task<T> Refresh(T old)
    {
        if (old == null)
        {
            throw new ArgumentNullException(nameof(old));
        }

        log.LogInformation("Refreshing credentials...");
        T credentials = await underlying.Refresh(old).ConfigureAwait(false);
        Save(credentials);
        return credentials;
    }

    private void Save(T credentials)
    {
        var json = JsonSerializer.Serialize(credentials, JsonSerializerOptions.Default);
        File.WriteAllText(AuthFile, json);
    }
}