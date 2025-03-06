namespace MagicVilla_WebApi.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string? type = "")
        {
            if (type == "error")
            {
                Console.WriteLine($"Error - {message}");
            }
            Console.WriteLine(message);
        }
    }
}
