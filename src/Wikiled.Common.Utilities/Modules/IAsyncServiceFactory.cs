using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Modules
{
    public interface IAsyncServiceFactory<T>
    {
        Task<T> GetService(bool refresh=false);
    }
}
