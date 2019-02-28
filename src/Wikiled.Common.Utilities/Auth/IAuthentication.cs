namespace Wikiled.Common.Utilities.Auth
{
    public interface IAuthentication<T>
    {
        T Authenticate();

        T Refresh(T old);
    }
}
