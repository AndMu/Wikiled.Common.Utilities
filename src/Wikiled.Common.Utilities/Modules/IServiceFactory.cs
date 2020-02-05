namespace Wikiled.Common.Utilities.Modules
{
    public interface IServiceFactory<out T>
    {
        T GetService();
    }
}
