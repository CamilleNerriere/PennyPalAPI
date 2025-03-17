
namespace PennyPal.Exceptions
{
    public class Unauthorized : Exception
    {
        public int StatusCode { get; set; }

        public Unauthorized(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }

}
