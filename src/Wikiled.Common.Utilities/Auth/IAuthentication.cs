using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Auth
{
    public interface IAuthentication<T>
    {
        Task<T> Authenticate();

        Task<T> Refresh(T old);
    }
}
