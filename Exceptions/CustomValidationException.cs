namespace PennyPal.Exceptions
{
    public class CustomValidationException : Exception
    {
        public CustomValidationException() : base() { }
        public CustomValidationException(string message) : base(message){ }
    }
}