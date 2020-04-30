namespace GlobalElements.EventDispatcherLib.Exception
{
    /// <summary>
    /// Exception that is passed through in the event dispatcher
    /// </summary>
    public class PassThroughException : System.Exception
    {
        public PassThroughException()
        {
        }

        public PassThroughException(string message) : base(message)
        {
        }

        public PassThroughException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}