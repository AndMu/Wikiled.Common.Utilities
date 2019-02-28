using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Auth
{
    public interface IAuthClient<T>
        where T : class
    {
        string BuildAuthorizeUrl(string callback = null);

        Task<T> GetToken(string code, string redirectUri = null);

        Task<T> RefreshToken(T token);
    }
}
