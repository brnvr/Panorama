using PanoramaApi.Extensions;

namespace PanoramaApi
{
    public class NotFoundException : Exception
    {
        public string ResourceName { get; }

        public NotFoundException(string resource, string message, Exception? innerException = null) : base(message, innerException)
        {
            ResourceName = resource;
        }
    }

    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message, Exception? innerException = null) : base(message, innerException) { }
    }

    public class DuplicateValueException : Exception
    {
        public object Value { get; }

        public DuplicateValueException(string message, object value, Exception? innerException = null) : base(message, innerException)
        {
            Value = value;
        }
    }
}
