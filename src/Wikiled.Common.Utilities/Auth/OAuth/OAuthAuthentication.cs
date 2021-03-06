﻿using System;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Auth.OAuth
{
    public class OAuthAuthentication<T> : IAuthentication<T>
        where T : class
    {
        private readonly IAuthClient<T> client;

        private readonly IOAuthHelper helper;

        public OAuthAuthentication(IAuthClient<T> client, IOAuthHelper helper)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public async Task<T> Authenticate()
        {
            var auth = await client.BuildAuthorizeUrl().ConfigureAwait(false);
            await helper.Start(auth).ConfigureAwait(false);
            var code = helper.Code;
            T token = await client.GetToken(code).ConfigureAwait(false);
            return token;
        }

        public Task<T> Refresh(T old)
        {
            return client.RefreshToken(old);
        }
    }
}
