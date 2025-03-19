using System.Net;

namespace MagicVilla_WebApi.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }

    }
}
