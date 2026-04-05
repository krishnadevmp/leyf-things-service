namespace LeyfThings.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public string ServiceName { get; }

        public ExternalServiceException(string serviceName, string message, Exception? inner = null)
            : base(message, inner)
        {
            ServiceName = serviceName;
        }
    }
}
