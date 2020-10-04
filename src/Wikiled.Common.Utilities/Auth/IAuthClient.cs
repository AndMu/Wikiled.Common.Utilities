using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Auth
{
    public interface IAuthClient<T>
        where T : class
    {
        Task<string> BuildAuthorizeUrl();
        
        Task<T> GetToken(string code);

        Task<T> RefreshToken(T token);
    }
}
