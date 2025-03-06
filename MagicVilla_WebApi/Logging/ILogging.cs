namespace MagicVilla_WebApi.Logging
{
    public interface ILogging
    {
        void Log(string message, string? type = "");
    }
}
