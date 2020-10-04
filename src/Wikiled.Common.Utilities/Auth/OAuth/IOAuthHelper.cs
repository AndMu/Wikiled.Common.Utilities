using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Auth.OAuth
{
    public interface IOAuthHelper
    {
        string RedirectUri { get; set; }

        string Code { get; }

        bool IsSuccessful { get; }

        Task Start(string serviceUrl);
    }
}