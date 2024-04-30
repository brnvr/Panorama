using PanoramaApi.Extensions;

namespace PanoramaApi
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string resource, string message, Exception? innerException = null) : base(message, innerException) { }
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
